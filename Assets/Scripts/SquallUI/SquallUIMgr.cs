using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


/// <summary>
/// UI根节点标识对象;
/// </summary>
public class SquallUIMgr : Singleton<SquallUIMgr>
{
    #region Loader

    public GameObject LoadUIPrefab()
    {
        //todo: Realize your load logic here
        Debug.LogError("You must realize yourselfs load logic here!");
        return null;
    }

    #endregion
    internal enum E_View_State
    {
        Show = 1,
        Hide = 2,
        Destroyed = 3,
    }

    public class ShowViewEvent
    {
        public string viewType;
        public UILogicEventDispatcher.EventPackage eventPackage;
    }

    internal class LoadingViewInfo
    {
        public Coroutine coroutine;
        public E_View_State viewState;
        public System.Action<IView> onLoaded;
    }

    private Dictionary<string, IView> viewList = new Dictionary<string, IView>();

    private Dictionary<string, LoadingViewInfo> loadingView = new Dictionary<string, LoadingViewInfo>();

    private IView currentView;

    private List<IView> orderViewStack = new();

    //界面堆栈，用于记录界面打开顺序 {index:viewName}
    private List<string> uiStack = new();
    private GameObject _screenBg;

    #region 界面层级

    private class LayerOrderInfo
    {
        /// <summary>
        /// 开始Order
        /// </summary>
        public int startSortingOrder;
        public List<UILayerInfo> layers = new List<UILayerInfo>();

        private IView m_CurrentView;

        public LayerOrderInfo(VIEW_LAYER layer)
        {
            startSortingOrder = (int)layer * 1000;
        }

        public void SetOrder(IView view)
        {
            RemoveOrder(view);
            UILayerInfo uiLayerInfo = new UILayerInfo() { view = view };
            if (layers.Count > 0)
            {
                uiLayerInfo.sortingOrder = layers[layers.Count - 1].sortingOrder + viewOrderOffset;
            }
            else
            {
                uiLayerInfo.sortingOrder = startSortingOrder;
            }
            layers.Add(uiLayerInfo);
            view.ApplySortingOrder(uiLayerInfo.sortingOrder);
        }

        public void RemoveOrder(IView view)
        {
            m_CurrentView = view;
            if (layers.Count > 0)
            {
                var index = layers.FindIndex(PredicateView);
                if (index >= 0)
                {
                    layers.RemoveAt(index);
                    if (index < (layers.Count - 1))
                        Compress();
                }
            }
            m_CurrentView = null;
        }

        private bool PredicateView(UILayerInfo layerInfo)
        {
            return m_CurrentView == layerInfo.view;
        }

        private void Compress()
        {
            for (int i = 0; i < layers.Count; ++i)
            {
                if (layers[i].view == null)
                    continue;

                int sorintOrder = startSortingOrder + i * viewOrderOffset;
                layers[i].view.ApplySortingOrder(sorintOrder);
            }
        }
    }

    private struct UILayerInfo
    {
        public int sortingOrder;
        public IView view;
    }

    private Dictionary<int, LayerOrderInfo> _panelOrder = new();
    private const int viewOrderOffset = 50;

    /// <summary>
    /// 设置界面到所在Layer的最顶层
    /// </summary>
    /// <param name="view"></param>
    /// <param name="layer"></param>
    public void SetOrder(IView view, int layer)
    {
        LayerOrderInfo layerOrderInfo;
        if (!_panelOrder.TryGetValue(layer, out layerOrderInfo))
        {
            layerOrderInfo = new LayerOrderInfo((VIEW_LAYER)layer);
            _panelOrder.Add(layer, layerOrderInfo);
        }
        layerOrderInfo.SetOrder(view);
    }

    /// <summary>
    /// 从Layer中移除View
    /// </summary>
    /// <param name="view"></param>
    /// <param name="layer"></param>
    public void RemoveOrder(IView view, int layer)
    {
        LayerOrderInfo layerOrderInfo;
        if (!_panelOrder.TryGetValue(layer, out layerOrderInfo))
        {
            return;
        }
        layerOrderInfo.RemoveOrder(view);
    }


    public int GetLayerOffset(VIEW_LAYER layer)
    {
        return (int)layer * 1000;
    }

    #endregion


    #region 引擎事件

    public void OnApplicationFocus(bool focus)
    {
        List<IView> tempList = ListPool<IView>.Get();
        tempList.AddRange(viewList.Values);
        try
        {
            foreach (var view in tempList)
            {
                if (view.IsVisible())
                {
                    view.OnApplicationFoucs(focus);
                }
            }
        }
        catch (Exception err)
        {
            Debug.LogError(err);
        }
        finally
        {
            ListPool<IView>.Release(tempList);
        }
    }

    public void OnApplicationPause(bool pause)
    {
        List<IView> tempList = ListPool<IView>.Get();
        tempList.AddRange(viewList.Values);
        try
        {
            foreach (var view in tempList)
            {
                if (view.IsVisible())
                {
                    view.OnApplicationPause(pause);
                }
            }
        }
        catch (Exception err)
        {
            Debug.LogError(err);
        }
        finally
        {
            ListPool<IView>.Release(tempList);
        }
    }

    #endregion


    private IView createViewInstance(string viewName)
    {
        IView viewInstance = ViewPackage.GetViewInstance(viewName);
        if (viewInstance != null)
            return viewInstance;

        LogManager.Instance.LogError("Warning: please add function in createViewInstance");
        return null;
    }

    /// <summary>
    /// 获取界面（界面可能未加载完，或者未显示，返回为false，且view为空）
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    public bool TryGetView(string viewName, out IView view)
    {
        return viewList.TryGetValue(viewName, out view);
    }

    // 创建界面
    private IView CreateView(string viewName, LruObj parentObj)
    {
        ResRefType resRefType = ResRefType.UIView;
        if (UINeedForeverKeep(viewName)) resRefType = ResRefType.RefForever;

        var result = ResManager.Instance.LoadUIManagerPrefab(viewName);
        GameObject prefab = (GameObject)result.Item1;
        string fullPath = result.Item2;

        if (prefab == null)
        {
            LogManager.Instance.Log("Warning: ui prefab name must be equal to view name..." + viewName, LogManager.LogContent.UIVIEW);
            return null;
        }
        ViewStatistics.Instance.EndLoad(viewName);

        GameObject viewObj = UnityEngine.Object.Instantiate(prefab);
        ViewStatistics.Instance.EndInstantiate(viewName);
        if (viewObj != null)
        {
            LruObj asset;
            //Lru
            if (parentObj != null)
            {
                asset = ResManager.Instance.AddChildLru(parentObj, fullPath, (lruObj) =>
                {
                    //因为有LruObj，所以这个View是其他东西创建的，销毁的时候会销毁资源，但ResManager不知道这是View还是Group
                    //所以这里要在回调里调用UImanager的DestroyView方法
                    ResManager.Instance.RefLog($"Lru调用Destroy（同步，子View） {viewName}", 2);
                    bool desSuc = Instance.DestroyView(viewName, true);
                    if (!desSuc) return false;

                    //不要在这里进行资源管理，资源的释放和子Lru的释放都在ResManager中统一进行，否则容易出现过释放情况

                    return true;
                });
            }
            else
            {
                asset = new LruObj();
                asset.FullPath = fullPath;
                asset.OnDestroy = (lruObj) =>
                {
                    ResManager.Instance.RefLog($"Lru调用Destroy（同步） {viewName}", 2);
                    bool suc = Instance.DestroyView(viewName, true);
                    if (!suc) return false;
                    //不要在这里进行资源管理，资源的释放和子Lru的释放都在ResManager中统一进行，否则容易出现过释放情况

                    return true;
                };
                asset.KeepInCache = UINeedKeep(viewName);

                ResManager.Instance.CheckAndRef(resRefType, asset);

                ResManager.Instance.PutLruCache(asset);
            }

            viewObj.name = viewName.ToString();
            IView view = createViewInstance(viewName);
            if (view != null)
                view.InitContainer(viewObj, viewName, asset);

            return view;
        }

        return null;
    }


    // 删除界面
    private bool destroyView(string viewName, bool isAll)
    {
        if (frozen)
        {
            LogManager.Instance.Log("销毁界面中...");
            return false;
        }

        if (!TryGetView(viewName, out var view))
        {
            if (loadingView.TryGetValue(viewName, out var loadViewInfo))
            {
                ResManager.Instance.RefLog($"无法销毁View！！！！！{viewName} 原因：正在加载", 2);
                loadViewInfo.viewState = E_View_State.Destroyed;
                return false;
            }

            ResManager.Instance.RefLog($"无法销毁View！！！！！{viewName} 原因：找不到了", 2);
            return true;
        }

        if (isAll)
        {
            for (int i = uiStack.Count - 1; i >= 0; i--)
            {
                if (uiStack[i] == viewName)
                {
                    uiStack.RemoveAt(i);
                }
            }

            for (int i = orderViewStack.Count - 1; i >= 0; i--)
            {
                if (orderViewStack[i] == view)
                    orderViewStack.Remove(view);
            }
        }

        if (view != null)
        {
            view.Destroy();
            ResManager.Instance.RefLog($"销毁View！！！！！{viewName}", 2);
            viewList.Remove(viewName);
        }
        else
        {
            Debug.LogError($"View {viewName} not found when destroy");
        }

        return true;
    }


    public void ShowScreenBackground(bool lockBlock = false)
    {
        BlockHelper.UIBlockBackgroundLocked = lockBlock;
        // Debug.LogError($"11111111  ShowScreenBackground _lockBlock:{lockBlock}");
        if (_screenBg == null)
        {
            _screenBg = UIRoot.Instance.gameObject.transform.parent.Find("ScreenBackground")?.gameObject;
            if (_screenBg == null)
            {
                GameObject prefab = ResManager.Instance.LoadPrefab("ScreenBackground", "UI/Prefab/System", null);
                _screenBg = Object.Instantiate(prefab, UIRoot.Instance.gameObject.transform.parent);
                _screenBg.name = "ScreenBackground";
                _screenBg.GetComponent<Canvas>().worldCamera = CameraManager.UICamera;
                GameObject.DontDestroyOnLoad(_screenBg.gameObject);
            }
        }
        _screenBg.SetActive(true);
    }

    public void HideScreenBackground(bool unlockBlock = false)
    {
        if (BlockHelper.UIBlockBackgroundLocked && !unlockBlock) return;

        // Debug.LogError($"11111111  HideScreenBackground {BlockHelper.UIBlockBackgroundLocked} {unlockBlock}");
        BlockHelper.UIBlockBackgroundLocked = false;

        if (_screenBg == null)
        {
            _screenBg = UIRoot.Instance.gameObject.transform.parent.Find("ScreenBackground")?.gameObject;
            if (_screenBg == null) return;
        }
        _screenBg.SetActive(false);
    }

    public void ShowView(string viewName, LruObj parentLru)
    {
        ShowView(viewName, null, false, parentLru);
    }
    // 显示界面
    public void ShowView(string viewName, Action<IView> onShow = null, bool forceSync = false, LruObj parentLru = null)
    {
        if (frozen)
        {
            LogManager.Instance.Log("销毁界面中...");
            return;
        }

        if (!UIFilter.Instance.Filter(viewName))
        {
            return;
        }

        GuideStaticData.TryGetID(viewName, out var viewId);
        if (viewId > 0)
        {
            if (GuideManager.Instance.OnEvent(GuideEvent.View, viewId, GuideConst.VIEW_OP_BEFORE_SHOW))
            {
                LogManager.Instance.LogFormat("[引导] 打开界面{0}中断", viewName);
                return;
            }
        }

        // HideScreenBackground();
        LogManager.Instance.Log("打开面板" + viewName.ToString(), LogManager.LogContent.UIVIEW);
        // InputManager.Instance.ClearDownCache();

        if (viewName != string.NetBlockView)
        {
            UIStackWatcher.Instance.PushUI(viewName.ToString());
        }

        UILogicEventDispatcher.Instance.Send(E_UILogicEventType.Game_ShowView);
        IView view = viewList.GetValueOrDefault(viewName);
        if (view == null)
        {
            LoadView(viewName, view =>
            {
                if (view == null) return;
                if (view.IsVisible())
                {
                    view.showCount++;
                    J2Statistics.SendClickFunction(view.UiGameObj.name, view.showCount);
                    onShow?.Invoke(view);
                    if (viewId > 0)
                    {
                        GuideManager.Instance.OnEvent(GuideEvent.View, viewId, GuideConst.VIEW_OP_SHOW);
                    }
                }
            }, forceSync, parentLru);
        }
        else
        {
            ResManager.Instance.RefLog($"使用旧的View：{viewName}", 4);
            ViewStatistics.Instance.BeginOpen(viewName);
            if (!view.IsVisible())
                view.Show();
            ViewStatistics.Instance.EndOpen(viewName);

            view.showCount++;
            J2Statistics.SendClickFunction(view.UiGameObj.name, view.showCount);
            onShow?.Invoke(view);

            if (viewId > 0)
            {
                GuideManager.Instance.OnEvent(GuideEvent.View, viewId, GuideConst.VIEW_OP_SHOW);
            }
        }
    }

    // 界面是否使用同步加载
    private Dictionary<string, bool> m_ViewLoadType = new Dictionary<string, bool>()
    {
        { string.InitView, true },
        { string.LoadingView, true },
        { string.LadderLoadingView, true },
        { string.CustomRoom1v1LoadingView, true },
        { string.ResourcePVP1v1LoadingView, true },
        { string.CustomRoom3v3LoadingView, true },
        { string.PeakFightLoadingView, true },
        { string.TeamBattleLoadingView, true },
        { string.NetBlockView, true },
        { string.NetConnectView, true },
        { string.MaskView, true },
        { string.BattleView, true },
        { string.LoginView, true },
        { string.MainCityView, true },
        { string.SelectPlayerView, true },
        { string.NewFinishView, true },
        { string.RevivePopView, true },
        { string.FullScreenVideoView, true },
        { string.BlackDialogueView, true },
        { string.DialogView, true },
        { string.MainFBView, true },
        { string.SpecialFlipCardView, true },
        { string.FlipCardView, true },
        { string.TipsView, true },
        { string.BackpackView, true },             // 引导需要
        { string.MainFBFinishView, true },             // 引导需要
        { string.EquipCompareView, true },         // 引导需要
        { string.SystemMessageView, true },        // 网络报错时，界面如果位于后台可能不能活动，这个界面会由于异步报错初始化不出来
    };
    private void LoadView(string viewType, Action<IView> onLoaded = null, bool forceSync = false, LruObj parentObj = null)
    {
        bool isSync;
        if (forceSync || m_ViewLoadType.TryGetValue(viewType, out isSync))
        {
            isSync = true;
        }

        if (isSync && loadingView.ContainsKey(viewType))
        {
            // 异步加载过程中
            isSync = false;
            LogManager.Instance.LogWarning(viewType + " 正在异步加载过程中，不能使用同步加载");
        }

        if (isSync)
        {
            IView view = CreateView(viewType, parentObj);
            if (view != null)
            {
                viewList[view.Name] = view;
                view.Show();
            }
            onLoaded?.Invoke(view);
        }
        else
        {
            LoadingViewInfo loadViewInfo;
            if (!loadingView.TryGetValue(viewType, out loadViewInfo))
            {
                loadViewInfo = new LoadingViewInfo();
                loadingView[viewType] = loadViewInfo;
                loadViewInfo.viewState = E_View_State.Show;
                loadViewInfo.coroutine = CoroutineInstance.BeginCoroutine(LoadViewCoroutine(viewType, view =>
                {
                    var loadingViewInfo = loadingView[viewType];
                    if (view != null)
                    {
                        viewList[view.Name] = view;
                        switch (loadingViewInfo.viewState)
                        {
                            case E_View_State.Hide:
                                view.Hide();
                                break;
                            case E_View_State.Destroyed:
                                viewList.Remove(view.Name);
                                view.Destroy();
                                break;
                            case E_View_State.Show:
                                view.Show();
                                break;
                        }
                    }
                    loadingViewInfo.onLoaded?.Invoke(view);
                    loadingView.Remove(viewType);
                }, parentObj));
            }
            loadViewInfo.onLoaded += onLoaded;
        }
    }

    public bool UINeedKeep(string eViewType)
    {
        return eViewType == string.MainCityView
            || eViewType == string.GMButtonView
            || eViewType == string.LoadingView;
    }

    private string[] _foreverKeepUI = new[]
    {
        string.LoadingView,
        string.MainCityView,//因为在非战斗<->主城切换时（如主城<->主城切换时，不会触发UIManager的释放方法，所以这些不会被释放，再次调用就是用旧的，旧的就不能保证他的资源不被释放）
        string.SelectPlayerView
    };
    public bool UINeedForeverKeep(string eViewType)
    {
        foreach (string viewType in _foreverKeepUI)
        {
            if (viewType == eViewType) return true;
        }

        return false;
    }

    private bool IsCrossSceneUI(string eViewType)
    {
        return false;
        return eViewType == string.LoadingView;
    }

    private IEnumerator LoadViewCoroutine(string viewType, Action<IView> onLoaded, LruObj parentObj = null)
    {
        GameObject prefab = null;
        string fullPath = "default";
        ResRefType resRefType = ResRefType.UIView;
        if (UINeedForeverKeep(viewType))
        {
            resRefType = ResRefType.RefForever;
        }
        yield return ResManager.Instance.LoadUIManagerPrefabAsync(viewType, (obj, loadFullPath) =>
        {
            prefab = obj;
            fullPath = loadFullPath;
        });

        if (prefab == null)
        {
            LogManager.Instance.Log(viewType, LogManager.LogContent.UIVIEW);
            LogManager.Instance.Log("Warning: ui prefab name must be equal to view name...", LogManager.LogContent.UIVIEW);
            onLoaded?.Invoke(null);
            yield break;
        }

        GameObject viewObj = UnityEngine.Object.Instantiate(prefab);
        if (viewObj != null)
        {
            LruObj asset;
            if (parentObj != null)
            {
                asset = ResManager.Instance.AddChildLru(parentObj, fullPath, (lruObj) =>
                {
                    //因为有LruObj，所以这个View是其他东西创建的，销毁的时候会销毁资源，但ResManager不知道这是View还是Group
                    //所以这里要在回调里调用UImanager的DestroyView方法
                    ResManager.Instance.RefLog($"Lru调用Destroy（异步，子View） {viewType}", 2);
                    bool destroySuc = Instance.DestroyView(viewType, true);
                    if (!destroySuc) return false;

                    return true;
                });
            }
            else
            {
                asset = new LruObj();
                asset.FullPath = fullPath;
                asset.OnDestroy = (lruObj) =>
                {
                    ResManager.Instance.RefLog($"Lru调用Destroy（异步) {viewType}", 2);
                    if (loadingView.TryGetValue(viewType, out var loadViewInfo))
                    {
                        ResManager.Instance.RefLog($"Lru调用Destroy,而{viewType}正在Loading，同时移除Loading列表中的它", 2);
                    }
                    bool suc = Instance.DestroyView(viewType, true);
                    if (!suc) return false;

                    return true;
                };
                //在外面Ref，这样能统一管理
                ResManager.Instance.CheckAndRef(resRefType, asset);
                //Put的时候，可能会释放其他LruObj
                //如果在调用他们的onDestroy方法时报错，那么会卡到这一句，UI就永远在LoadingView中后面就加载不出来了
                ResManager.Instance.PutLruCache(asset);
                //所以要确保Keep状态要在Put成功之后加，否则LruCache也Pop不出来它了
                asset.KeepInCache = UINeedKeep(viewType);
            }

            viewObj.name = viewType.ToString();
            IView view = createViewInstance(viewType);
            if (view != null)
                view.InitContainer(viewObj, viewType, asset);

            onLoaded?.Invoke(view);
            yield break;
        }

        onLoaded(null);
    }


    //缓存有序界面
    public void PushOrderStack(IView view)
    {
        if (GameUtil.UNITY_IOS()) return;
        currentView = view;

        if (view != null && getOrderStackIndex(view) == -1)
        {
            orderViewStack.Add(view);
        }
    }

    public int getOrderStackIndex(IView view)
    {
        for (int i = 0; i < orderViewStack.Count; i++)
        {
            IView orderView = orderViewStack[i];
            if (orderView == view)
            {
                return i;
            }
        }
        return -1;
    }

    public void PushView(IView view)
    {
        if (view == null) return;

        if (view.CurStackMode == VIEW_STACK.OverLay) return;

        bool haveView = uiStack.Contains(view.Name);
        if (view.CurStackMode == VIEW_STACK.FullOnly)
        {
            if (uiStack.Count > 0)
            {
                string topViewName = uiStack[uiStack.Count - 1];
                if (topViewName != view.Name)
                {
                    IView topView = viewList.ContainsKey(topViewName) ? viewList[topViewName] : null;
                    if (topView != null)
                    {
                        topView.ActiveHide(false);
                    }
                    if (!haveView)
                        uiStack.Add(view.Name);
                    else
                    {
                        while (uiStack.Count > 0 && uiStack[uiStack.Count - 1] != view.Name)
                        {
                            uiStack.RemoveAt(uiStack.Count - 1);
                        }
                    }
                }
            }
            else
            {
                uiStack.Add(view.Name);
            }
        }
        else if (view.CurStackMode == VIEW_STACK.OverMain)
        {
            if (uiStack.Count > 0)
            {
                string topViewName = uiStack[uiStack.Count - 1];
                if (topViewName != view.Name && UIHelper.IsMainCityView(topViewName))
                {
                    IView topView = viewList[topViewName];
                    if (topView != null)
                    {
                        topView.ActiveHide(false);
                    }
                    //if (!haveView)
                    //    uiStack.Add(view.Name);
                    //else
                    //{
                    //    while (uiStack.Count > 0 && uiStack[uiStack.Count - 1] != view.Name)
                    //    {
                    //        uiStack.RemoveAt(uiStack.Count - 1);
                    //    }
                    //}
                }
            }
        }
    }

    public void PopOrderStack(IView view)
    {
        if (GameUtil.UNITY_IOS()) return;

        if (view != null)
        {
            int index = getOrderStackIndex(view);
            if (index != -1)
            {
                orderViewStack.Remove(view);
            }
        }

        if (orderViewStack.Count > 0)
        {
            currentView = orderViewStack[orderViewStack.Count - 1];
        }
        else
        {
            if (uiStack != null && uiStack.Count > 0)
            {
                string topViewName = uiStack[uiStack.Count - 1];
                if (viewList.ContainsKey(topViewName))
                {
                    currentView = viewList[topViewName];
                }
            }
        }
    }


    public void PopView(IView view)
    {
        if (view == null) return;

        if (view.CurStackMode == VIEW_STACK.OverLay || uiStack.Count == 0) return;

        string topViewName = uiStack[uiStack.Count - 1];
        bool popNext = false;
        if (view.CurStackMode == VIEW_STACK.OverMain && UIHelper.IsMainCityView(topViewName))
        {
            popNext = true;
        }
        else if (view.Name == topViewName)
        {
            popNext = true;
            uiStack.RemoveAt(uiStack.Count - 1);
        }
        if (popNext && uiStack.Count > 0)
        {
            string nextStack = uiStack[uiStack.Count - 1];
            GuideStaticData.TryGetID(nextStack, out var viewId);
            if (viewId > 0)
            {
                if (GuideManager.Instance.OnEvent(GuideEvent.View, viewId, GuideConst.VIEW_OP_BEFORE_SHOW))
                {
                    LogManager.Instance.LogFormat("[引导] 打开界面{0}中断", nextStack);
                    return;
                }
            }
            getAndCreateView(nextStack, nextView =>
            {
                if (nextView != null && frozen == false)
                {
                    nextView.ActiveShow(false);
                    if (viewId > 0)
                    {
                        GuideManager.Instance.OnEvent(GuideEvent.View, viewId, GuideConst.VIEW_OP_SHOW);
                    }
                }
            });
        }
    }

    public void getAndCreateView(string viewName, System.Action<IView> onLoaded)
    {
        IView view = viewList.ContainsKey(viewName) ? viewList[viewName] : null;
        if (view == null)
        {
            LoadView(viewName, onLoaded);
        }
        else
        {
            ResManager.Instance.RefLog($"使用旧的View：{viewName}", 4);
            onLoaded?.Invoke(view);
        }
    }

    // 删除界面
    public bool DestroyView(string viewName, bool isAll = false)
    {
        return destroyView(viewName, isAll);
    }

    private bool frozen = false;
    // 删除所有界面
    public void DestroyAllView()
    {
        LogManager.Instance.Log("销毁所有界面");
        frozen = true;
        try
        {
            foreach (var kv in viewList)
            {
                kv.Value.ActiveHide(false);
                kv.Value.Destroy();
            }
            foreach (var kvp in loadingView)
            {
                kvp.Value.viewState = E_View_State.Destroyed;
            }
            ClearOrderViewStack();
            ClearViewList();
            currentView = null;
        }
        catch (Exception err)
        {
            LogManager.Instance.LogError(err);
        }
        finally
        {
            frozen = false;
        }
    }


    private void ClearViewList()
    {
        Dictionary<string, IView> keepViews = new Dictionary<string, IView>();
        foreach (KeyValuePair<string, IView> keyValuePair in viewList)
        {
            if (IsCrossSceneUI(keyValuePair.Key)) keepViews.Add(keyValuePair.Key, keyValuePair.Value);
        }

        viewList.Clear();
        foreach (KeyValuePair<string, IView> keyValuePair in keepViews)
        {
            viewList.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }

    private void ClearOrderViewStack()
    {
        List<IView> keepViews = new List<IView>();
        foreach (IView view in orderViewStack)
        {
            if (IsCrossSceneUI(view.Name)) keepViews.Add(view);
        }

        orderViewStack.Clear();
        orderViewStack.AddRange(keepViews);
    }

    public void HideAllView()
    {
        frozen = true;
        try
        {
            foreach (var kv in viewList)
            {
                kv.Value.Hide();
            }
            foreach (var kvp in loadingView)
            {
                kvp.Value.viewState = E_View_State.Hide;
            }
        }
        catch (Exception err)
        {
            SLog.LogError(err);
        }
        finally
        {
            frozen = false;
        }
    }

    // 界面是否在显示
    public bool IsViewVisible(string viewName, bool defaultIfNotExist = false)
    {
        if (viewList.TryGetValue(viewName, out var view) && view != null)
        {
            return view.IsVisible();
        }
        return defaultIfNotExist;
    }

    public bool IsViewShowLoading(string viewName)
    {
        if (loadingView.TryGetValue(viewName, out var loadingInfo))
        {
            return loadingInfo.viewState == E_View_State.Show;
        }
        return false;
    }

    // 显示界面携带事件
    public void ShowViewByEvent(string viewName, UILogicEventDispatcher.EventPackage eventPackge, bool syncLoad = false)
    {
        ShowView(viewName, view =>
        {
            if (view != null && view.IsVisible())
            {
                view.OnReceiveLogicEvent(eventPackge);
            }
        }, syncLoad);
    }

    public void ShowViewByEvent(string viewName, object eventPackge)
    {
        ShowView(viewName, view =>
        {
            if (view != null && view.IsVisible())
            {
                view.OnReceiveLogicEvent(eventPackge);
            }
        });
    }


    // 接受逻辑事件
    public void ReceiveLogicEvent(string viewName, UILogicEventDispatcher.EventPackage eventPackage)
    {
        IView view = viewList.ContainsKey(viewName) ? viewList[viewName] : null;
        if (view != null)
        {
            view.OnReceiveLogicEvent(eventPackage);
        }
    }

    // 接收键盘事件
    public void ReceiveKeycodeEvent(InputCmd cmd, InputStatus status)
    {
        if (TryGetView(string.BattleView, out var outView))
        {
            outView.OnReceiveKeycodeEvent(cmd, status);
        }
        if (currentView == null)
            return;

        currentView.OnReceiveKeycodeEvent(cmd, status);

        //if (cmd == InputCmd.BackOrSetting && status == InputStatus.Up)
        //    ShowExitGame();
    }

    private bool canResponseShowExitGame = true;
    // 显示退出游戏提示
    public void ShowExitGame()
    {
        if (canResponseShowExitGame)
        {
            if (GameUtil.IsLeDouPackage())
            {
                if (CommonDefine.MLSDSDKChannel == 0) // MLSDChannelPlatform.None)
                    ShowDefaultExitGame();
                else
                    ShowSDKExitGame();
            }
            else
                ShowDefaultExitGame();
        }
    }

    private bool isShowExitGame = false;
    public void ShowDefaultExitGame()
    {
        if (!isShowExitGame)
        {
            isShowExitGame = true;

            UIHelper.Instance.ShowSystemMessageView(FunctionUtility.GetText("ExitGameTips"),
                () =>
                {
                    isShowExitGame = false;

                    if (!LuaMain.Instance.IsGameState(GameState.GAMESTATE_DEFINE.GameLogin))
                    {
                        //LogReportTool.RoleSnap()

                        //LogReportTool.RoleExitGame()
                    }

                    LuaMain.Instance.ExitGame();

                },


                () =>
                {
                    isShowExitGame = false;
                },
                FunctionUtility.GetText("ExitGameOK"),
                FunctionUtility.GetText("ExitGameCancel"),
                false,
                false
                );
        }

    }

    public void ShowSDKExitGame()
    {
        if (GameSdkManager.IsSupportExitGame())
        {
            GameSdkManager.SDKExitGame((result) =>
            {
                if (result)
                {
                    if (!LuaMain.Instance.IsGameState(GameState.GAMESTATE_DEFINE.GameLogin))
                    {
                        //    LogReportTool.RoleSnap()

                        //LogReportTool.RoleExitGame()
                    }

                    LuaMain.Instance.ExitGame();
                }
            });
        }
        else
            UIManager.Instance.ShowDefaultExitGame();
    }


    // 隐藏界面
    public void HideView(string viewName)
    {
        IView view = viewList.ContainsKey(viewName) ? viewList[viewName] : null;
        if (view != null && view.IsVisible())
        {
            view.Hide();
        }
        else
        {
            if (loadingView.TryGetValue(viewName, out var loadViewInfo))
            {
                loadViewInfo.viewState = E_View_State.Hide;
            }
        }
    }
    // UnloadUnusedView 卸载无用界面
    public void UnloadUnusedView()
    {
        List<string> unloadViewKeys = new List<string>();

        foreach (var kv in viewList)
        {
            if (kv.Value != null && !kv.Value.IsVisible() && kv.Value.CanUnload)
            {
                if (!IsCrossSceneUI(kv.Key))
                {
                    unloadViewKeys.Add(kv.Key);
                }
            }
        }

        for (int i = 0; i < unloadViewKeys.Count; i++)
        {
            DestroyView(unloadViewKeys[i], false);
        }

    }

    // UI栈
    public void ClearUIStack(bool forceAll = false)
    {
        uiStack.Clear();
        if (!forceAll)
        {
            foreach (string eViewType in _foreverKeepUI)
            {
                uiStack.Add(eViewType);
            }
        }
    }

    public override void Dispose()
    {
        ClearUIStack();
        DestroyAllView();
        //释放UI共享的RenderTexture资源
        //LuaUtil.DestroyWhiteBlackRenderTexture()
        base.Dispose();
    }

    // 隐藏所有OverLay界面
    public void HideAllOverLay(bool includeChat = true)
    {
        foreach (var kv in viewList)
        {
            var view = kv.Value;
            if (view != null && view.IsVisible())
            {
                if (!includeChat && view.Name == string.ChatView)
                    continue;
                if (view.Name == string.TeamMatchView)
                    continue;
                if (view.CurStackMode == VIEW_STACK.OverLay)
                {
                    view.Hide();
                }
            }
        }
    }

    // 隐藏所有层级大于0的界面
    public void HideAllLayerMoreThanZero()
    {
        foreach (var kv in viewList)
        {
            var view = kv.Value;
            if (view != null && view.IsVisible())
            {
                if (view.Layer > VIEW_LAYER.Bottom)
                    view.Hide();
            }

        }

    }

    public bool HasTheSameLayerView(string view, VIEW_LAYER layer)
    {
        bool isHave = false;
        foreach (var viewInfo in viewList)
        {
            if (viewInfo.Key == view)
            {
                continue;
            }
            if (viewInfo.Value.Layer == layer && viewInfo.Value.IsVisible())
            {
                isHave = true;
                break;
            }
        }
        return isHave;
    }

    public void AdaptScreenArea()
    {
        var offset = PlayerPrefsManager.Instance.GetAdaptScreenArea();
        foreach (var kv in viewList)
        {
            var view = kv.Value;
            if (view != null)
            {
                view.AutoAdaptScreenSafeArea();
            }
        }
    }

    private List<string> loadViewList = new(){
        string.LoadingView ,
        string.LadderLoadingView ,
        string.CustomRoom1v1LoadingView,
        string.ResourcePVP1v1LoadingView,
        string.PeakFightLoadingView,
        string.CustomRoom3v3LoadingView,
        string.TeamBattleLoadingView
    };

    public bool IsLoadViewShow()
    {
        foreach (var kv in viewList)
        {
            if (loadViewList.Contains(kv.Key) && kv.Value != null && kv.Value.IsVisible())
            {
                return true;
            }
        }
        return false;
    }

    private List<string> tempViewKeys = new List<string>();
    public void LateUpdate()
    {
        tempViewKeys.Clear();
        tempViewKeys.AddRange(viewList.Keys);
        for (int i = 0; i < tempViewKeys.Count; ++i)
        {
            if (!viewList.TryGetValue(tempViewKeys[i], out var view))
            {
                continue;
            }

            if (view != null && view.IsVisible() && view.CanUpdate())
            {
                view.DoUpdate();
            }
        }
        TeamManager.Instance.Update();
        FPSCounter.Instance.Update();
    }

    /// 同步战斗的UpdateLogic
    public void UpdateLogic(int delta)
    {
        foreach (var kv in viewList)
        {
            if (kv.Value != null && kv.Value.IsVisible() && kv.Value.CanUpdate())
                kv.Value.UpdateLogic(delta);
        }
    }

    public void UpdateFixedLogic(int delta)
    {
        foreach (var kv in viewList)
        {
            if (kv.Value != null && kv.Value.IsVisible() && kv.Value.CanUpdate())
                kv.Value.UpdateFixedLogic(delta);
        }
    }

    public void UpdateLogicWithoutVisible(int delta)
    {
        foreach (var kv in viewList)
        {
            if (kv.Value != null && kv.Value.CanUpdate())
                kv.Value.UpdateLogicWithoutVisible(delta);
        }
    }

    public string GetTopView()
    {
        if (uiStack.Count <= 0)
            return string.None;

        string topViewName = uiStack[uiStack.Count - 1];
        return topViewName;
    }

    public List<string> GetUiStack()
    {
        return uiStack;
    }

    public int GetVisibleViewCount()
    {
        int count = 0;
        foreach (var kv in viewList)
        {
            var view = kv.Value;
            if (view != null && view.IsVisible() &&
                view.Name != string.GMButtonView && view.Name != string.MarqueeView)
            {
                count++;
            }
        }
        return count;
    }
}

public class ViewPackage
{
    public static IView GetViewInstance(string viewName)
    {
        IView ret = null;
        switch (viewName)
        {
            case string.LoadingView:
                ret = new LoadingView();
                break;
            case string.InitView:
                ret = new InitView();
                break;
            case string.MainCityView:
                ret = new MainCityView();
                break;
            case string.QuestView:
                ret = new QuestView();
                break;
            case string.SelectJobGroupView:
                ret = new SelectJobGroupView();
                break;
            case string.CreateJobView:
                ret = new CreateJobView();
                break;
            case string.DeletePlayerView:
                ret = new DeletePlayerView();
                break;
            case string.MainBtnSetView:
                ret = new MainBtnSetView();
                break;
            case string.RankView:
                ret = new RankView();
                break;
            case string.MilitaryRankView:
                ret = new MilitaryRankView();
                break;
            default:
                Type t = Type.GetType(viewName.ToString());
                if (t != null)
                {
                    ret = TypeFactory.Create(t) as IView;
                }
                break;

        }

        return ret;
    }
}

#if UNITY_EDITOR

public enum EPrefabProblem
{
    None = 0,
    /// <summary>
    /// GameObject数量过多
    /// </summary>
    OverLimitObjects = 0x1,
    /// <summary>
    /// 包含动态资源
    /// </summary>
    ContainDynamicObject = 0x2,
    /// <summary>
    /// 包含ItemComponent
    /// </summary>
    ContainPooledObject = 0x4,
    /// <summary>
    /// 包含旧版本资源
    /// </summary>
    ContainOldVersionRes = 0x8,
    /// <summary>
    /// 包含其他对象
    /// </summary>
    ContainOtherPrefab = 0x10,
    /// <summary>
    /// 包含透明度接近0的Graphic
    /// </summary>
    ContainerTransparentGraphic = 0x20,
}

public class UIPrefabStatistics
{
    public GameObject prefab;
    public string assetPath;
    public bool foldOut;
    public Dictionary<EPrefabProblem, bool> typeFoldOut = new Dictionary<EPrefabProblem, bool>();
    public Dictionary<EPrefabProblem, List<UIPrefabProblem>> problems = null;

    public bool HasProblem => problems != null && problems.Count > 0;

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("UI存在问题(可通过菜单栏 J2->性能检测->UI预制检测 更方便的定位)：");
        builder.Append(assetPath);
        builder.AppendLine();

        foreach (var kvp in problems)
        {
            switch (kvp.Key)
            {
                case EPrefabProblem.ContainDynamicObject:
                    builder.AppendLine("----------------");
                    builder.AppendLine("包含动态对象");
                    builder.AppendLine("动态资源为会通过代码加载并修改，请判断是否需要保留，如果不需要保留，在做完预制后，应当删除");
                    builder.AppendLine("----------------");
                    foreach (var problem in kvp.Value)
                    {
                        builder.AppendLine(GetPath(problem.target.transform, prefab.transform));
                    }
                    break;
                case EPrefabProblem.ContainOldVersionRes:
                    builder.AppendLine("----------------");
                    builder.AppendLine("包含旧版本资源");
                    builder.AppendLine("旧版本资源在替换界面后应当删除");
                    builder.AppendLine("----------------");
                    foreach (var problem in kvp.Value)
                    {
                        builder.AppendLine(GetPath(problem.target.transform, prefab.transform));
                    }
                    break;
                case EPrefabProblem.ContainPooledObject:
                    builder.AppendLine("----------------");
                    builder.AppendLine("包含对象池节点");
                    builder.AppendLine("包含已使用对象池的对象（使用代码加载解决）");
                    builder.AppendLine("----------------");
                    foreach (var problem in kvp.Value)
                    {
                        builder.AppendLine(GetPath(problem.target.transform, prefab.transform));
                    }
                    break;
                case EPrefabProblem.OverLimitObjects:
                    builder.AppendLine("----------------");
                    builder.Append("物体数量超过");
                    builder.AppendLine(MaxObjectCount.ToString());
                    builder.AppendLine($"单个预制数量不要超过{MaxObjectCount}，可通过拆分界面、动态加载等方式减少物体数量");
                    builder.AppendLine("----------------");
                    builder.Append("物体数量:");
                    builder.AppendLine(kvp.Value[0].intParam1.ToString());
                    break;
                case EPrefabProblem.ContainOtherPrefab:
                    builder.AppendLine("----------------");
                    builder.AppendLine("包含非UI预制");
                    builder.AppendLine("直接包含特效预制、模型预制等等");
                    builder.AppendLine("----------------");
                    foreach (var problem in kvp.Value)
                    {
                        builder.AppendLine(GetPath(problem.target.transform, prefab.transform));
                    }
                    break;
                case EPrefabProblem.ContainerTransparentGraphic:
                    builder.AppendLine("----------------");
                    builder.AppendLine("包含全透明组件");
                    builder.AppendLine("使用Empty4Raycast代替（减少Drawcall）");
                    builder.AppendLine("----------------");
                    foreach (var problem in kvp.Value)
                    {
                        builder.AppendLine(GetPath(problem.target.transform, prefab.transform));
                    }
                    break;
                default:
                    break;
            }
        }

        return builder.ToString();
    }

    private static Stack<Transform> stack = new Stack<Transform>();
    private static StringBuilder getPathBuilder = new StringBuilder();
    private static string GetPath(Transform target, Transform root)
    {
        stack.Clear();
        Transform parent = target;
        while (parent != null && parent != root)
        {
            stack.Push(parent);
            parent = parent.parent;
        }

        getPathBuilder.Length = 0;
        while (stack.Count > 0)
        {
            getPathBuilder.Append(stack.Pop().name);
            getPathBuilder.Append('/');
        }
        if (getPathBuilder.Length > 0)
        {
            getPathBuilder.Length -= 1;
        }
        return getPathBuilder.ToString();
    }

    private static List<string> pooledObject = new List<string>()
    {
        "Assets/Resources/UI/Prefab/PrefabComponent/ItemComponent.prefab"
    };

    public static int MaxObjectCount => UnityEditor.EditorPrefs.GetInt("__editor_ui_max_objects_count__", 300);

    public static UIPrefabStatistics AnalysisUIPrefab(GameObject obj)
    {
        UIPrefabStatistics statistics = new UIPrefabStatistics()
        {
            prefab = obj,
            assetPath = UnityEditor.AssetDatabase.GetAssetPath(obj),
        };

        List<UIPrefabProblem> problems = new List<UIPrefabProblem>();

        Dictionary<System.Type, List<Component>> componentDict = new Dictionary<System.Type, List<Component>>();

        // 统计Component
        Component[] components = obj.GetComponentsInChildren<Component>(true);
        foreach (var component in components)
        {
            var componentType = component.GetType();
            List<Component> typeComponents;
            if (!componentDict.TryGetValue(componentType, out typeComponents))
            {
                typeComponents = new List<Component>();
                componentDict.Add(componentType, typeComponents);
            }
            typeComponents.Add(component);
        }

        if (componentDict.TryGetValue(typeof(RectTransform), out var transformList))
        {
            // 分析数量
            if (transformList.Count > MaxObjectCount)
            {
                problems.Add(new UIPrefabProblem()
                {
                    problemType = EPrefabProblem.OverLimitObjects,
                    target = obj,
                    intParam1 = transformList.Count
                });
            }

            // 分析包含已使用对象池的预制
            foreach (var transform in transformList)
            {
                string prefabPath = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(transform);
                if (prefabPath != statistics.assetPath)
                {
                    GameObject prefabInstance = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(transform);
                    if (prefabInstance == transform.gameObject)
                    {
                        if (pooledObject.Contains(prefabPath))
                        {
                            problems.Add(new UIPrefabProblem()
                            {
                                problemType = EPrefabProblem.ContainPooledObject,
                                target = transform.gameObject
                            });
                        }
                        else if (!prefabPath.StartsWith("Assets/Resources/UI/Prefab") && !prefabPath.StartsWith("Assets/Resources/UI/View"))
                        {
                            problems.Add(new UIPrefabProblem()
                            {
                                problemType = EPrefabProblem.ContainOtherPrefab,
                                target = transform.gameObject
                            });
                        }
                    }
                }
            }
        }

        foreach (var kvp in componentDict)
        {
            // 分析Image中包含旧版本资源或者动态加载的资源
            if (typeof(Image).IsAssignableFrom(kvp.Key))
            {
                foreach (var comp in kvp.Value)
                {
                    Image image = comp as Image;
                    string assetPath = UnityEditor.AssetDatabase.GetAssetPath(image.sprite);
                    if (assetPath.StartsWith("Assets/Arts/"))
                    {
                        problems.Add(new UIPrefabProblem()
                        {
                            problemType = EPrefabProblem.ContainOldVersionRes,
                            target = comp.gameObject
                        });
                    }
                    else if (assetPath.StartsWith("Assets/Arts2/UIDynamic/"))
                    {
                        problems.Add(new UIPrefabProblem()
                        {
                            problemType = EPrefabProblem.ContainDynamicObject,
                            target = comp.gameObject
                        });
                    }
                }
            }

            // 分析RawImage中包含旧版本资源或者动态加载的资源
            if (typeof(RawImage).IsAssignableFrom(kvp.Key))
            {
                foreach (var comp in kvp.Value)
                {
                    RawImage image = comp as RawImage;
                    string assetPath = UnityEditor.AssetDatabase.GetAssetPath(image.mainTexture);
                    if (assetPath.StartsWith("Assets/Arts/"))
                    {
                        problems.Add(new UIPrefabProblem()
                        {
                            problemType = EPrefabProblem.ContainOldVersionRes,
                            target = comp.gameObject
                        });
                    }
                    else if (assetPath.StartsWith("Assets/Arts/UIDynamic/"))
                    {
                        problems.Add(new UIPrefabProblem()
                        {
                            problemType = EPrefabProblem.ContainDynamicObject,
                            target = comp.gameObject
                        });
                    }
                }
            }

            // 分析Text中包含旧版本资源或者动态加载的资源
            if (typeof(TMPro.TMP_Text).IsAssignableFrom(kvp.Key))
            {
                foreach (var comp in kvp.Value)
                {
                    TMPro.TMP_Text text = comp as TMPro.TMP_Text;
                    string assetPath = UnityEditor.AssetDatabase.GetAssetPath(text.font);
                    if (assetPath.StartsWith("Assets/Arts/"))
                    {
                        problems.Add(new UIPrefabProblem()
                        {
                            problemType = EPrefabProblem.ContainOldVersionRes,
                            target = comp.gameObject
                        });
                    }
                }
            }

            if (typeof(Graphic).IsAssignableFrom(kvp.Key))
            {
                foreach (var comp in kvp.Value)
                {
                    Graphic graphic = comp as Graphic;
                    if (graphic.color.a < 0.01f)
                    {
                        problems.Add(new UIPrefabProblem()
                        {
                            problemType = EPrefabProblem.ContainerTransparentGraphic,
                            target = comp.gameObject
                        });
                    }
                }
            }
        }

        statistics.problems = new Dictionary<EPrefabProblem, List<UIPrefabProblem>>();
        foreach (var problem in problems)
        {
            List<UIPrefabProblem> list;
            if (!statistics.problems.TryGetValue(problem.problemType, out list))
            {
                list = new List<UIPrefabProblem>();
                statistics.problems.Add(problem.problemType, list);
            }
            list.Add(problem);
        }

        return statistics;
    }
}

public class UIPrefabProblem
{
    public EPrefabProblem problemType = EPrefabProblem.None;
    public GameObject target;
    public int intParam1;
}

#endif

public class ViewStatistics : Singleton<ViewStatistics>
{
    public class ViewDebugInfo
    {
        public string viewType;
        public int firstOpenTime;
        public string header;
        public ViewOpenDebugInfo openningInfo;
        public List<ViewOpenDebugInfo> viewOpenList = new List<ViewOpenDebugInfo>();
    }

    public class ViewOpenDebugInfo
    {
        /// <summary>
        /// 打开开始时间
        /// </summary>
        public long openTime_Begin;
        /// <summary>
        /// 加载结束时间
        /// </summary>
        public long openTime_Loaded;
        /// <summary>
        /// 实例化结束时间
        /// </summary>
        public long openTime_Instantiated;
        /// <summary>
        /// 打开结束时间
        /// </summary>
        public long openTime_End;
    }

    public Dictionary<string, ViewDebugInfo> viewDebugInfos = new Dictionary<string, ViewDebugInfo>();

    [System.Diagnostics.Conditional("UI_DEBUG")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void BeginOpen(string viewType)
    {
        ViewDebugInfo viewDebugInfo;
        ViewOpenDebugInfo viewOpenDebugInfo;
        if (!viewDebugInfos.TryGetValue(viewType, out viewDebugInfo))
        {
            viewDebugInfo = new ViewDebugInfo();
            viewDebugInfo.viewType = viewType;
            viewDebugInfos.Add(viewType, viewDebugInfo);
        }
        viewOpenDebugInfo = new ViewOpenDebugInfo();
        viewDebugInfo.openningInfo = viewOpenDebugInfo;
        viewOpenDebugInfo.openTime_Begin = GameUtil.GetMsTimestamp();
        viewOpenDebugInfo.openTime_Loaded = -1;
        viewOpenDebugInfo.openTime_Instantiated = -1;
        viewOpenDebugInfo.openTime_End = -1;
    }

    [System.Diagnostics.Conditional("UI_DEBUG")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void EndLoad(string viewType)
    {
        long endTime = GameUtil.GetMsTimestamp();
        ViewDebugInfo viewDebugInfo;
        if (!viewDebugInfos.TryGetValue(viewType, out viewDebugInfo) || viewDebugInfo == null || viewDebugInfo.openningInfo == null)
        {
            Debug.LogWarning("错误：没有打开，直接结束");
            return;
        }
        viewDebugInfo.openningInfo.openTime_Loaded = endTime;
    }

    [System.Diagnostics.Conditional("UI_DEBUG")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void EndInstantiate(string viewType)
    {
        long endTime = GameUtil.GetMsTimestamp();
        ViewDebugInfo viewDebugInfo;
        if (!viewDebugInfos.TryGetValue(viewType, out viewDebugInfo) || viewDebugInfo == null || viewDebugInfo.openningInfo == null)
        {
            Debug.LogWarning("错误：没有打开，直接结束");
            return;
        }
        viewDebugInfo.openningInfo.openTime_Instantiated = endTime;
    }

    [System.Diagnostics.Conditional("UI_DEBUG")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void EndOpen(string viewType)
    {
        long endTime = GameUtil.GetMsTimestamp();
        ViewDebugInfo viewDebugInfo;
        if (!viewDebugInfos.TryGetValue(viewType, out viewDebugInfo) || viewDebugInfo == null || viewDebugInfo.openningInfo == null)
        {
            Debug.LogWarning("错误：没有打开，直接结束");
            return;
        }
        viewDebugInfo.openningInfo.openTime_End = endTime;
        viewDebugInfo.viewOpenList.Add(viewDebugInfo.openningInfo);
        viewDebugInfo.openningInfo = null;
    }

    [System.Diagnostics.Conditional("UI_DEBUG")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void Save(string filepath)
    {
        string text = Newtonsoft.Json.JsonConvert.SerializeObject(viewDebugInfos);
        System.IO.File.WriteAllText(filepath, text);
    }
}