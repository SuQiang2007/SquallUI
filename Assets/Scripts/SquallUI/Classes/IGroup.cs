using System;
using System.Collections.Generic;
using SquallUI.Classes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// UI对象控件容器抽象
/// </summary>
public class IGroup : IControlContainer
{
    public RectTransform _cacheTrans;
    protected bool _visible;
    protected Dictionary<Type, IGroup> panelDict;
    public int showCount = 0;
    public void InitContainer(GameObject obj)
    {
        this._uiGameObj = obj;
        this._cacheTrans = obj.GetComponent<RectTransform>();
        OnInit();
    }

    public void InitContainerByOwner(IControlContainer ownerContainer, GameObject obj)
    {
        this._uiGameObj = obj;
        this._cacheTrans = obj.transform.GetComponent<RectTransform>();
        OnInitByOwner(ownerContainer);
        OnInit();
    }

    // 显示
    public void Show()
    {
        this._visible = true;
        this._uiGameObj.SetActive(true);
        OnShow();
    }


    // 隐藏
    public void Hide()
    {
        this._visible = false;
        OnHide();
        if (_uiGameObj != null)
            this._uiGameObj.SetActive(false);
    }

    private bool _destroyed = false;
    // 删除，但其不会删除group的Obj
    public void Destroy()
    {
        _destroyed = true;
        if (this._visible)
            Hide();

        //---TimerManager.DestroyByTarget(self);
        //---LogicEventDispatcher.RemoveListener(self);
        string name = _uiGameObj == null ? "null" : _uiGameObj.name;
        // ResManager.Instance.RefLog($"IGroup 被销毁{name}", 2);
        OnDestroy();
    }

    // 设置界面可见性
    public void SetVisible(bool visible)
    {
        if (visible)
            this.Show();
        else
            this.Hide();
    }

    public void DoUpdate()
    {
        Update();
    }

    protected virtual void Update()
    {
    }

    public virtual void UpdateLogic(int delta)
    {

    }

    public virtual void UpdateLogicWithoutVisible(int delta)
    {
    }

    // 返回界面是否可见
    public bool IsVisible()
    {
        return this._visible;
    }

    protected virtual void OnInit()
    {

    }

    // 当初始化时回调，包含所属容器，方便调用容器方法
    public virtual void OnInitByOwner(IControlContainer ownerContainer)
    {

    }

    // 当显示时回调
    protected virtual void OnShow()
    {

    }

    // 当隐藏时回调
    protected virtual void OnHide()
    {

    }

    // 当删除时调用
    public virtual void OnDestroy()
    {

    }

    public bool IsValid()
    {
        return !_destroyed;
    }


    #region 子界面

    /// <summary>
    /// 显示子界面
    /// </summary>
    /// <typeparam name="T">子界面类型（如果replacePrefabName为空，则子界面的名字为类型名字）</typeparam>
    /// <param name="subRoot">子界面挂载节点</param>
    /// <param name="path">子界面所在路径</param>
    /// <param name="resetMatrix">是否重置位置，旋转，位移</param>
    /// <param name="replacePrefabName">替换的预制名字</param>
    protected T ShowPanel<T>(Transform subRoot, string path, string eViewType, bool resetMatrix = false, string replacePrefabName = null, System.Action<T> executeBeforeShow = null) where T : IGroup, new()
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<Type, IGroup>();
        }

        // 获取UI预制名称
        var t = typeof(T);

        IGroup panel;
        if (!panelDict.TryGetValue(t, out panel))
        {
            GameObject prefab = SquallUIMgr.Instance.LoadUIPrefab();
            GameObject uiObj = GameObject.Instantiate(prefab, subRoot);
            if (resetMatrix)
            {
                uiObj.transform.localPosition = Vector3.zero;
                uiObj.transform.localRotation = Quaternion.identity;
                uiObj.transform.localScale = Vector3.one;
            }
            panel = new T();
            panel.InitContainerByOwner(this, uiObj);
            panelDict.Add(t, panel);
        }

        executeBeforeShow?.Invoke((T)panel);
        panel.Show();
        panel.showCount++;

        return (T)panel;
    }

    protected IGroup ShowPanel(string panelName, Transform subRoot, string path, string eViewType, bool resetMatrix = false, string replacePrefabName = null)
    {
        var panelType = typeof(SquallUIMgr).Assembly.GetType(panelName);

        if (!typeof(IGroup).IsAssignableFrom(panelType))
        {
            return null;
        }

        if (panelDict == null)
        {
            panelDict = new Dictionary<System.Type, IGroup>();
        }

        // 获取UI预制名称
        string uiPrefabName;
        if (string.IsNullOrEmpty(replacePrefabName))
        {
            uiPrefabName = panelType.Name;
        }
        else
        {
            uiPrefabName = replacePrefabName;
        }

        IGroup panel;
        if (!panelDict.TryGetValue(panelType, out panel))
        {
            ResRefObj resRefObj = new ResRefObj();
            resRefObj.resRefType = ResRefType.DynamicUI;
            resRefObj.parentUIClassName = eViewType.ToString();
            GameObject prefab = ResManager.Instance.loadAsset(typeof(GameObject), uiPrefabName, path, resRefObj) as GameObject;
            GameObject uiObj = GameObject.Instantiate(prefab, subRoot);
            if (resetMatrix)
            {
                uiObj.transform.localPosition = Vector3.zero;
                uiObj.transform.localRotation = Quaternion.identity;
                uiObj.transform.localScale = Vector3.one;
            }
            panel = TypeFactory.Create(panelType) as IGroup;
            panel.InitContainerByOwner(this, uiObj);
            panelDict.Add(panelType, panel);
        }
        panel.Show();
        panel.showCount++;

        return panel;
    }

    protected T ShowAndHideOtherPanel<T>(Transform subRoot, string path, string eViewType, bool resetMatrix = false, string replacePrefabName = null, System.Action<T> executeBeforeShow = null) where T : IGroup, new()
    {
        if (panelDict != null)
        {
            Type t = typeof(T);
            foreach (var kvp in panelDict)
            {
                if (kvp.Key == t)
                {
                    continue;
                }
                kvp.Value.Hide();
            }
        }
        return ShowPanel<T>(subRoot, path, eViewType, resetMatrix, replacePrefabName, executeBeforeShow);
    }

    /// <summary>
    /// 隐藏子界面
    /// </summary>
    protected void HidePanel<T>()
    {
        HidePanel(typeof(T));
    }

    /// <summary>
    /// 隐藏子界面
    /// </summary>
    /// <param name="panelType"></param>
    protected void HidePanel(Type panelType)
    {
        if (panelDict == null)
            return;

        IGroup panel;
        if (!panelDict.TryGetValue(panelType, out panel))
        {
            return;
        }
        panel.Hide();
    }

    /// <summary>
    /// 隐藏所有子界面
    /// </summary>
    protected void HideAllPanel()
    {
        if (panelDict == null) return;
        foreach (var kvp in panelDict)
        {
            kvp.Value.Hide();
        }
    }

    /// <summary>
    /// 销毁所有子界面（如果子界面没有隐藏，则先隐藏，然后销毁）
    /// </summary>
    protected void DestroyAllPanel()
    {
        if (panelDict == null) return;
        foreach (var kvp in panelDict)
        {
            if (kvp.Value.IsVisible())
            {
                kvp.Value.Hide();
            }
            kvp.Value.Destroy();
        }
        panelDict.Clear();
    }

    protected T GetPanel<T>() where T : IGroup
    {
        return GetPanel(typeof(T)) as T;
    }

    protected IGroup GetPanel(Type t)
    {
        if (panelDict == null)
        {
            return null;
        }

        if (panelDict.TryGetValue(t, out var group))
        {
            return group;
        }

        return null;
    }
    #endregion
}
