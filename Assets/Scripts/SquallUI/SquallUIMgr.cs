using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Pool;
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

        public LayerOrderInfo(ViewLayer layer)
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
            layerOrderInfo = new LayerOrderInfo((ViewLayer)layer);
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


    public int GetLayerOffset(ViewLayer layer)
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

        SLog.LogError("Warning: please add function in createViewInstance");
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
    private IView CreateView(string viewName)
    {
        var prefab = LoadUIPrefab();

        if (prefab == null)
        {
            SLog.Log("Warning: ui prefab name must be equal to view name..." + viewName);
            return null;
        }
        GameObject viewObj = UnityEngine.Object.Instantiate(prefab);
        if (viewObj != null)
        {
            
            viewObj.name = viewName.ToString();
            IView view = createViewInstance(viewName);
            if (view != null)
                view.InitContainer(viewObj, viewName);

            return view;
        }

        return null;
    }


    // 删除界面
    private bool destroyView(string viewName, bool isAll)
    {
        if (frozen)
        {
            SLog.Log("销毁界面中...");
            return false;
        }

        if (!TryGetView(viewName, out var view))
        {
            if (loadingView.TryGetValue(viewName, out var loadViewInfo))
            {
                SLog.Log($"无法销毁View！！！！！{viewName} 原因：正在加载");
                loadViewInfo.viewState = E_View_State.Destroyed;
                return false;
            }

            SLog.Log($"无法销毁View！！！！！{viewName} 原因：找不到了");
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
            SLog.Log($"销毁View！！！！！{viewName}");
            viewList.Remove(viewName);
        }
        else
        {
            SLog.Log($"View {viewName} not found when destroy");
        }

        return true;
    }


    public static bool UIBlockBackgroundLocked;
    public void ShowScreenBackground(bool lockBlock = false)
    {
        UIBlockBackgroundLocked = lockBlock;
        // Debug.LogError($"11111111  ShowScreenBackground _lockBlock:{lockBlock}");
        if (_screenBg == null)
        {
            _screenBg = UIRoot.Instance.gameObject.transform.parent.Find("ScreenBackground")?.gameObject;
            if (_screenBg == null)
            {
                GameObject prefab = LoadUIPrefab();
                _screenBg = Object.Instantiate(prefab, UIRoot.Instance.gameObject.transform.parent);
                _screenBg.name = "ScreenBackground";
                // _screenBg.GetComponent<Canvas>().worldCamera = CameraManager.UICamera;
                GameObject.DontDestroyOnLoad(_screenBg.gameObject);
            }
        }
        _screenBg.SetActive(true);
    }

    public void HideScreenBackground(bool unlockBlock = false)
    {
        if (UIBlockBackgroundLocked && !unlockBlock) return;

        UIBlockBackgroundLocked = false;

        if (_screenBg == null)
        {
            _screenBg = UIRoot.Instance.gameObject.transform.parent.Find("ScreenBackground")?.gameObject;
            if (_screenBg == null) return;
        }
        _screenBg.SetActive(false);
    }

    public void ShowView(string viewName)
    {
        ShowView(viewName, null, false);
    }
    // 显示界面
    public void ShowView(string viewName, Action<IView> onShow = null, bool forceSync = false)
    {
        if (frozen)
        {
            SLog.Log("销毁界面中...");
            return;
        }
        
        SLog.Log("打开面板" + viewName.ToString());

        UILogicEventDispatcher.Instance.Send((int)UILogicEvents.Game_ShowView);
        IView view = viewList.GetValueOrDefault(viewName);
        if (view == null)
        {
            LoadView(viewName, view =>
            {
                if (view == null) return;
                if (view.IsVisible())
                {
                    view.showCount++;
                    onShow?.Invoke(view);
                }
            }, forceSync);
        }
        else
        {
            SLog.Log($"使用旧的View：{viewName}");
            if (!view.IsVisible())
                view.Show();

            view.showCount++;
            onShow?.Invoke(view);
        }
    }

    // 界面是否使用同步加载
    private Dictionary<string, bool> m_ViewLoadType = new Dictionary<string, bool>()
    {
        // { string.InitView, true }
    };
    private void LoadView(string viewType, Action<IView> onLoaded = null, bool forceSync = false)
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
            SLog.Log(viewType + " 正在异步加载过程中，不能使用同步加载");
        }

        if (isSync)
        {
            IView view = CreateView(viewType);
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
                }));
            }
            loadViewInfo.onLoaded += onLoaded;
        }
    }

    private IEnumerator LoadViewCoroutine(string viewType, Action<IView> onLoaded)
    {
        GameObject prefab = LoadUIPrefab();
        if (prefab == null)
        {
            onLoaded?.Invoke(null);
            yield break;
        }

        GameObject viewObj = UnityEngine.Object.Instantiate(prefab);
        if (viewObj != null)
        {
            viewObj.name = viewType.ToString();
            IView view = createViewInstance(viewType);
            if (view != null)
                view.InitContainer(viewObj, viewType);

            onLoaded?.Invoke(view);
            yield break;
        }

        onLoaded(null);
    }


    //缓存有序界面
    public void PushOrderStack(IView view)
    {
        if (UNITY_IOS()) return;
        currentView = view;

        if (view != null && getOrderStackIndex(view) == -1)
        {
            orderViewStack.Add(view);
        }
    }

    private static bool UNITY_IOS()
    {
#if UNITY_IOS
        return true;
#else
        return false;
#endif
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

        if (view.CurStackMode == ViewStack.OverLay) return;

        bool haveView = uiStack.Contains(view.Name);
        if (view.CurStackMode == ViewStack.FullOnly)
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
        else if (view.CurStackMode == ViewStack.OverMain)
        {
            if (uiStack.Count > 0)
            {
                string topViewName = uiStack[uiStack.Count - 1];
                if (topViewName != view.Name)
                {
                    IView topView = viewList[topViewName];
                    if (topView != null)
                    {
                        topView.ActiveHide(false);
                    }
                }
            }
        }
    }

    public void PopOrderStack(IView view)
    {
        if (UNITY_IOS()) return;

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

        if (view.CurStackMode == ViewStack.OverLay || uiStack.Count == 0) return;

        string topViewName = uiStack[uiStack.Count - 1];
        bool popNext = false;
        if (view.CurStackMode == ViewStack.OverMain)
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
            getAndCreateView(nextStack, nextView =>
            {
                if (nextView != null && frozen == false)
                {
                    nextView.ActiveShow(false);
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
            SLog.Log($"使用旧的View：{viewName}");
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
        SLog.Log("销毁所有界面");
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
            SLog.LogError(err);
        }
        finally
        {
            frozen = false;
        }
    }


    private void ClearViewList()
    {
        viewList.Clear();
    }

    private void ClearOrderViewStack()
    {
        orderViewStack.Clear();
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
                unloadViewKeys.Add(kv.Key);
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
    public void HideAllOverLay()
    {
        foreach (var kv in viewList)
        {
            var view = kv.Value;
            if (view != null && view.IsVisible())
            {
                if (view.CurStackMode == ViewStack.OverLay)
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
                if (view.Layer > ViewLayer.Bottom)
                    view.Hide();
            }

        }

    }

    public bool HasTheSameLayerView(string view, ViewLayer layer)
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

    private List<string> loadViewList = new(){
        // string.LoadingView ,
        // string.LadderLoadingView ,
        // string.CustomRoom1v1LoadingView,
        // string.ResourcePVP1v1LoadingView,
        // string.PeakFightLoadingView,
        // string.CustomRoom3v3LoadingView,
        // string.TeamBattleLoadingView
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
            return E_View_Type.None.ToString();

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
            if (view != null && view.IsVisible())
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
            // case string.LoadingView:
            //     ret = new LoadingView();
            //     break;
            // case string.InitView:
            //     ret = new InitView();
            //     break;
            // case string.MainCityView:
            //     ret = new MainCityView();
            //     break;
            // case string.QuestView:
            //     ret = new QuestView();
            //     break;
            // case string.SelectJobGroupView:
            //     ret = new SelectJobGroupView();
            //     break;
            // case string.CreateJobView:
            //     ret = new CreateJobView();
            //     break;
            // case string.DeletePlayerView:
            //     ret = new DeletePlayerView();
            //     break;
            // case string.MainBtnSetView:
            //     ret = new MainBtnSetView();
            //     break;
            // case string.RankView:
            //     ret = new RankView();
            //     break;
            // case string.MilitaryRankView:
            //     ret = new MilitaryRankView();
            //     break;
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