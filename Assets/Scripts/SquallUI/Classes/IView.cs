using System;
using System.Collections.Generic;
using SquallUI.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

/// <summary>
/// UI对象控件容器抽象
/// </summary>
public class IView : IControlContainer
{
	public string Name;
	protected RectTransform _CachedTrans;
	private bool _Visible;
	public ViewLayer Layer;
	public ViewStack CurStackMode;
	protected bool IsAudio;
	protected bool NeedUpdate;
	public float ViewOffset { get; protected set; } = 0;
	public bool CanUnload;
	public int showCount = 0;

	protected Dictionary<Type, IGroup> panelDict;

	// 内部构造，界面对象请勿调用
	public void InitContainer(GameObject obj, string viewName)
	{
		this.Name = viewName;
		this._uiGameObj = obj;
		this._CachedTrans = obj.GetComponent<RectTransform>();
		this._CachedTrans.SetParent(UIRoot.Instance.Trans);
		
		// 设置 RectTransform 为全屏拉伸，确保适配正确
		this._CachedTrans.anchorMin = Vector2.zero;
		this._CachedTrans.anchorMax = Vector2.one;
		this._CachedTrans.offsetMax = Vector2.zero;
		this._CachedTrans.offsetMin = Vector2.zero;
		this._CachedTrans.localScale = Vector3.one;
		this._Visible = false;
		this.Layer = ViewLayer.Default;
		this.CurStackMode = ViewStack.FullOnly;
		this.IsAudio = true;
		this.NeedUpdate = false;
		
		// 处理 GraphicRaycaster：删除根节点的，确保根 Canvas 上有且只有一个
		GraphicRaycaster graphicRaycaster = obj.GetComponent<GraphicRaycaster>();
		Canvas canvas = obj.GetComponent<Canvas>();
		
		if (canvas != null)
		{
			// 确保 Canvas 使用 Screen Space - Overlay，这样所有 Canvas 共享同一个 EventSystem
			if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			}
			
			// 如果根节点有 GraphicRaycaster，删除它（因为会在 Canvas 上统一管理）
			if (graphicRaycaster != null)
			{
				Object.Destroy(graphicRaycaster);
			}
			
			// 确保 Canvas 上有 GraphicRaycaster（如果没有则添加）
			if (canvas.GetComponent<GraphicRaycaster>() == null)
			{
				canvas.gameObject.AddComponent<GraphicRaycaster>();
			}
		}
		else
		{
			// 如果没有 Canvas，删除 GraphicRaycaster（避免干扰）
			if (graphicRaycaster != null)
			{
				Object.Destroy(graphicRaycaster);
			}
		}

		// 适配屏幕安全区域（在设置 anchor 后调用，确保适配正确）
		AutoAdaptScreenSafeArea();
		this.ViewOffset = this._CachedTrans.sizeDelta.x / 2;
		// 能否卸载，表示随时可以直接删除
		this.CanUnload = false;

		OnInit();
	}

	private Canvas m_Canvas;
	public void ApplySortingOrder(int sortingOrder)
	{
		if (this.UiGameObj == null)
			return;

		if (m_Canvas == null)
		{
			m_Canvas = this.UiGameObj.GetComponent<Canvas>();
			if (m_Canvas == null)
			{
				return;
			}
		}
		m_Canvas.overrideSorting = true;
		m_Canvas.sortingOrder = sortingOrder;
	}

	protected Canvas GetCanvas()
	{
		return m_Canvas;
	}

	// 适配忽略的UI
	private static Dictionary<string, int> ignoreAdaptView = new Dictionary<string, int>()
	{
		// {string.MainCityView, 0x1 }
	};

	// 自动适配屏幕安全区域
	public void AutoAdaptScreenSafeArea()
	{
		/* 1. 横向适配(上下贴边）
		 * 2. 左右Padding适配：
		 *			- 通用界面：最大值(最小值(根节点画布宽度-1920, 150), 屏幕区域调整偏移, 刘海屏宽度))
		 */

		// 刘海屏
		float paddingHorizontal = Math.Max(Screen.safeArea.xMin, Screen.width - Screen.safeArea.xMax);
		// 指定定机型适配
		//todo
		// 屏幕贴边适配
		if (!ignoreAdaptView.TryGetValue(Name, out int v) || (v & 0x1) != 0x1)
		{
			float rootWidth = UIRoot.Instance.RectTransform.rect.width;
			// 修复：Mathf.Max 需要两个参数，这里应该是限制在 0 到 150 之间
			paddingHorizontal = Mathf.Max(0f, Mathf.Min((rootWidth - UIRoot.DESIGN_WIDTH) * 0.5f, 150f));
		}
		SetScreenArea(paddingHorizontal);
	}
	
	public void SetScreenArea(float offset)
	{
		this._CachedTrans.offsetMin = new Vector2(offset, 0);
		this._CachedTrans.offsetMax = -new Vector2(offset, 0);
		AutoAdaptCloseButton("CloseButton");
	}
	
	protected void AutoAdaptCloseButton(string closeBtnPath)
	{
		if (this.CurStackMode == ViewStack.FullOnly)
		{
			var btnClose = this.GetChildObj(closeBtnPath);
			if (btnClose != null)
			{
				RectTransform imgCloseRect = btnClose.transform.Find("img_close")?.GetComponent<RectTransform>();
				RectTransform labCloseRect = btnClose.transform.Find("lab_title")?.GetComponent<RectTransform>();
				TextMeshProUGUI labClose = btnClose.transform.Find("lab_title")?.GetComponent<TextMeshProUGUI>();
				if (this._CachedTrans.offsetMin.x >= 120)
				{
					if (imgCloseRect != null)
						imgCloseRect.anchoredPosition = new Vector2(-68, 0);
					if (labCloseRect != null)
						labCloseRect.anchoredPosition = new Vector2(8, 0);
					if (labClose != null)
						labClose.alignment = TextAlignmentOptions.Center;
				}
				else
				{
					if (imgCloseRect != null)
						imgCloseRect.anchoredPosition = new Vector2(-5, 0);
					if (labCloseRect != null)
						labCloseRect.anchoredPosition = new Vector2(80, 0);
					if (labClose != null)
						labClose.alignment = TextAlignmentOptions.Left;
				}
			}
		}
	}

	// 初始化，界面需重写此方法
	protected virtual void OnInit()
	{
	}

	//当显示时回调
	protected virtual void OnShow()
	{
		//不是所有View都调用了base.Onshow()，不能倚仗这个回调
		// ResManager.Instance.KeepLruCache(Name);
	}

	protected virtual void AfterShow(bool isDequeue)
	{
	}

	protected virtual void OnHide()
	{
		// UIStackWatcher.Instance.PopUI(Name.ToString());
		// ResManager.Instance.UnkeepLruCache(Name);
	}
	protected virtual void OnDestroy()
	{
	}

	public void DoUpdate()
	{
		Update();
	}

	protected virtual void Update()
	{
	}

	// 同步战斗UpdateLogic的算法
	// int delta = (int)(Time.deltaTime * GlobalConst.VALUE_1000);
	public virtual void UpdateLogic(int delta)
	{

	}

	//使用帧同步的步长，现在用来算技能cd
	public virtual void UpdateFixedLogic(int delta)
	{

	}

	public virtual void UpdateLogicWithoutVisible(int delta)
	{

	}


	public bool CanUpdate()
	{
		return NeedUpdate;
	}

	public void Show()
	{
		ActiveShow(true);
	}

	public void Hide()
	{
		ActiveHide(true);
	}

	public void Destroy()
	{
		_Visible = false;
		UILogicEventDispatcher.Instance.RemoveListenerByHandler(this.OnReceiveLogicEvent, this);
		//TimerManager.Instance.DestroyByTarget(this);
		OnDestroy();
	}

	//具体界面请勿调用
	public void ActiveShow(bool isActive)
	{
		SquallUIMgr.Instance.PushOrderStack(this);
		if (isActive)
		{
			SquallUIMgr.Instance.PushView(this);
		}

		_Visible = true;

		_uiGameObj.SetActive(true);

		AutoAdaptScreenSafeArea();
		UIRoot.OnAspectChanged.AddListener(OnAspectChanged);

		OnShow();
		SquallUIMgr.Instance.SetOrder(this, (int)Layer);
		
		UILogicEventDispatcher.Instance.SendWithArgs((int)UILogicEvents.OnViewShow, this.Name);

		AfterShow(!isActive);
	}

	public void ActiveHide(bool isActive)
	{
		UIRoot.OnAspectChanged.RemoveListener(OnAspectChanged);
		if (_CachedTrans == null) return;
		SquallUIMgr.Instance.PopOrderStack(this);

		if (_Visible)
		{
			// 避免销毁时重复调用OnHide
			_Visible = false;
			OnHide();

			if (_uiGameObj != null)
			{
				_uiGameObj.SetActive(false);
			}
			UILogicEventDispatcher.Instance.SendWithArgs((int)UILogicEvents.OnViewHide, this.Name);
		}

		//发送界面隐藏引导事件
		//SendGuideEvent(GuideEvent.AfterViewOnHide, self.Name)

		if (_uiGameObj != null && _uiGameObj.activeSelf)
		{
			_uiGameObj.SetActive(false);
		}

		if (isActive)
		{
			SquallUIMgr.Instance.PopView(this);
		}

		SquallUIMgr.Instance.RemoveOrder(this, (int)Layer);
	}

	private void OnAspectChanged()
	{
		if (this._CachedTrans == null)
		{
			UIRoot.OnAspectChanged.RemoveListener(OnAspectChanged);
			return;
		}
		AutoAdaptScreenSafeArea();
	}


	public bool IsVisible()
	{
		return _Visible;
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
	protected T ShowPanel<T>(Transform subRoot, string path, bool resetMatrix = false, string replacePrefabName = null, System.Action<T> executeBeforeShow = null) where T : IGroup, new()
	{
		if (panelDict == null)
		{
			panelDict = new Dictionary<Type, IGroup>();
		}

		// 获取UI预制名称
		var t = typeof(T);
		if (!panelDict.TryGetValue(t, out var panel))
		{
			panel = SquallUIMgr.Instance.CreateGroup<T>(t.Name, this, subRoot);
			panelDict.Add(t, panel);
		}

		executeBeforeShow?.Invoke((T)panel);
		panel.Show();
		panel.showCount++;

		return (T)panel;
	}

	protected IGroup ShowPanel(string panelName, Transform subRoot, string path, bool resetMatrix = false, string replacePrefabName = null, System.Action<IGroup> executeBeforeShow = null)
	{
		Type panelType = typeof(SquallUIMgr).Assembly.GetType(panelName);

		if (!typeof(IGroup).IsAssignableFrom(panelType))
		{
			return null;
		}

		if (panelDict == null)
		{
			panelDict = new Dictionary<Type, IGroup>();
		}

		if (!panelDict.TryGetValue(panelType, out var panel))
		{
			panel = SquallUIMgr.Instance.CreateGroup(panelType, panelName, this);
			panelDict.Add(panelType, panel);
		}

		executeBeforeShow?.Invoke(panel);
        panel.Show();
		panel.showCount++;

		return panel;
	}

	protected T ShowAndHideOtherPanel<T>(Transform subRoot, string path, bool resetMatrix = false, string replacePrefabName = null, System.Action<T> executeBeforeShow = null) where T : IGroup, new()
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
		return ShowPanel<T>(subRoot, path, resetMatrix, replacePrefabName, executeBeforeShow);
	}

	protected IGroup ShowAndHideOtherPanel(Type panelType, Transform subRoot, string path, bool resetMatrix = false, string replacePrefabName = null, System.Action<IGroup> executeBeforeShow = null)
	{
		if (!typeof(IGroup).IsAssignableFrom(panelType))
		{
			SLog.LogError("必须为IGroup类型");
			return null;
		}

        if (panelDict != null)
        {
            foreach (var kvp in panelDict)
            {
                if (kvp.Key == panelType)
                {
                    continue;
                }
                kvp.Value.Hide();
            }
        }
        return ShowPanel(panelType.Name, subRoot, path, resetMatrix, replacePrefabName, executeBeforeShow);
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

	public bool IsPanelVisible<T>()
	{
		return IsPanelVisible(typeof(T));
	}

	public bool IsPanelVisible(System.Type t)
	{
		var panel = GetPanel(t);
		return panel != null && panel.IsVisible();
	}

	#endregion

	public virtual void OnApplicationFoucs(bool focus)
	{
	}


	public virtual void OnApplicationPause(bool pause)
	{

	}
}
