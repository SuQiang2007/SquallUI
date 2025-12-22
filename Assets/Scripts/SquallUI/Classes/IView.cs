using System;
using System.Collections.Generic;
using SquallUI.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// UI对象控件容器抽象
/// </summary>
public class IView : IControlContainer
{
	public string Name;
	protected RectTransform _CachedTrans;
	private bool _Visible;
	public VIEW_LAYER Layer;
	public VIEW_STACK CurStackMode;
	protected bool IsAudio;
	protected bool NeedUpdate;
	protected GameGraphicRaycaster graphicRaycaster;
	public float ViewOffset { get; protected set; } = 0;
	public bool CanUnload;
	public int showCount = 0;

#if UNITY_EDITOR
	private int m_OnShowItemComponentCount = 0;
#endif

	protected Dictionary<Type, IGroup> panelDict;

	// 内部构造，界面对象请勿调用
	public void InitContainer(GameObject obj, string viewName)
	{
		this.Name = viewName;
		this._uiGameObj = obj;
		this._CachedTrans = obj.GetComponent<RectTransform>();
		this._CachedTrans.SetParent(UIRoot.Instance.Trans);
		this._CachedTrans.offsetMax = Vector2.zero;
		this._CachedTrans.offsetMin = Vector2.zero;
		this._CachedTrans.localScale = Vector3.one;
		this._Visible = false;
		this.Layer = VIEW_LAYER.Default;
		this.CurStackMode = VIEW_STACK.FullOnly;
		this.IsAudio = true;
		this.NeedUpdate = false;
		GraphicRaycaster graphicRaycaster = obj.GetComponent<GraphicRaycaster>();
		if (graphicRaycaster != null)
			UnityEngine.GameObject.Destroy(graphicRaycaster);

		graphicRaycaster = obj.GetComponent<GameGraphicRaycaster>();
		if (graphicRaycaster == null)
			obj.AddComponent(typeof(GameGraphicRaycaster));

		this.graphicRaycaster = obj.GetComponent<GameGraphicRaycaster>();
		AutoAdaptScreenSafeArea();
		this.ViewOffset = this._CachedTrans.sizeDelta.x / 2;
		// 能否卸载，表示随时可以直接删除
		this.CanUnload = false;

		OnInit();
	}

	protected GameObject LoadIGroupAndManagerBySelf(string name, string path)
	{
		ResRefObj resRefObj = new ResRefObj();
		var tuple = ResManager.Instance.LoadAssetIGroup(typeof(GameObject), name, path, resRefObj, ViewLruObj);
		GameObject prefab = (GameObject)tuple.Item1;
		return prefab;
	}

	protected Sprite GetSprite(string name, string path, ResRefType resRefType = ResRefType.DynamicUI)
	{
		// Debug.Log($"GetSprite called name is {name} path is {path}");
		return ResManager.Instance.LoadSpritePrefab(name, path, ViewLruObj);
	}

	protected Texture GetTexture(string name, string path)
	{
		// Debug.Log($"GetSprite called name is {name} path is {path}");
		return ResManager.Instance.LoadTexturePrefab(name, path, ViewLruObj);
	}

	protected Sprite GetSpriteInAtlas(string atlas, string name, ResRefType resRefType = ResRefType.DynamicUI)
	{
		// Debug.Log($"GetSprite called name is {name} atlas is {atlas}");
		// LoadUIAtlas
		return ResManager.Instance.LoadSpriteAtlas(atlas, name, Name, resRefType);
	}


	protected SpriteAtlas GetAtlas(string atlas)
	{
		ResRefObj refObj = new ResRefObj();
		refObj.resRefType = ResRefType.UIView;
		var tuple = ResManager.Instance.LoadSpriteAtlas(atlas, ResRefType.UIView, ViewLruObj);
		return tuple;
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
		{string.MainCityView, 0x1 },
		{string.ChatView, 0x1 },
		{string.LinkPackageView, 0x1 },
		{string.MainFBFinishView, 0x1 },
		{string.BattleView, 0x1 },
		{string.BloodView, 0x2 },
		{string.SetAreaView, 0x1 },
		{string.SelectPlayerView, 0x1 },
		{string.MiniMapView, 0x1 },
		{string.DialogView, 0x1 },
		{string.ResourcePVPCityView, 0x1 },
		{string.AbyssEffectView, 0x1 },
	};

	private float setOffset;

	// 自动适配屏幕安全区域
	public void AutoAdaptScreenSafeArea()
	{
		/* 1. 横向适配(上下贴边）
		 * 2. 左右Padding适配：
		 *			- 通用界面：最大值(最小值(根节点画布宽度-1920, 150), 屏幕区域调整偏移, 刘海屏宽度))
		 */

		// 刘海屏
		float paddingHorizontal = Math.Max(Screen.safeArea.xMin, Screen.width - Screen.safeArea.xMax);
		// 屏幕区域调整（手动）
		setOffset = PlayerPrefsManager.Instance.GetAdaptScreenArea();
		paddingHorizontal = Mathf.Max(paddingHorizontal, setOffset);
		// 指定定机型适配
		if (DataManager.Instance.IsInit)
		{
			string deviceModel = SystemInfo.deviceModel;
			DeviceInfoCfg deviceInfo = DataManager.Instance.GetContainer<DeviceInfoCfgs>().GetData(deviceModel);
			Debug.Log($"Device info====>:{JsonConvert.SerializeObject(deviceInfo)}");
			if (deviceInfo != null)
			{
				paddingHorizontal = Mathf.Max(paddingHorizontal, Screen.safeArea.xMin + deviceInfo.width_offset, Screen.width - Screen.safeArea.xMax + deviceInfo.width_offset);
			}
		}
		// 屏幕贴边适配
		if (!ignoreAdaptView.TryGetValue(Name, out int v) || (v & 0x1) != 0x1)
		{
			float rootWidth = UIRoot.Instance.rectTransform.rect.width;
			paddingHorizontal = Mathf.Max(Mathf.Min((rootWidth - UIRoot.DESIGN_WIDTH) * 0.5f, 150f));
		}
		SetScreenArea(paddingHorizontal);
	}

	public float AdaptScreenArea(float offset)
	{
		if (offset == 0)
			return 0;

		float curWidth = this._CachedTrans.rect.width;
		float curHeight = this._CachedTrans.rect.height;
		var maxwidth = UIRoot.Instance.rectTransform.rect.width;
		var maxheight = UIRoot.Instance.rectTransform.rect.height;
		var width = curWidth + offset * 2;
		var height = curHeight;
		float curAspectRatio = curWidth / curHeight;
		float nextAspectRatio = width / height;
		var maxAspectRatio = (float)maxwidth / maxheight; // 最大宽高比
		var minAspectRatio = (float)16 / 9; // 最小宽高比
		if (curAspectRatio < nextAspectRatio)
		{
			if (curAspectRatio >= maxAspectRatio)
			{
				setOffset = 0;
			}
			else
			{
				setOffset = maxwidth - width;
			}
		}
		else
		{
			if (curAspectRatio <= minAspectRatio)
			{
				setOffset = maxwidth - curHeight * minAspectRatio;
			}
			else
			{
				setOffset = maxwidth - width;
			}
		}

		setOffset /= 2;
		this._CachedTrans.offsetMin = new Vector2(setOffset, 0);
		this._CachedTrans.offsetMax = new Vector2(-setOffset, 0);
		return setOffset;

		/*
        var maxwidth = UIRoot.Instance.rectTransform.rect.width;
		var maxheight = UIRoot.Instance.rectTransform.rect.height;
		var minAspectRatio = (float)16 / 9; // 最小宽高比
		var maxAspectRatio = (float)maxwidth / maxheight; // 最大宽高比
		var curAspectRatio = (float)width / height;   // 当前宽高比

		if (curAspectRatio >= minAspectRatio && curAspectRatio <= maxAspectRatio) // 大于默认宽高比
		{
			setOffset = (float)Math.Floor((maxwidth - width) / 2);
			this._CachedTrans.offsetMin = new Vector2(setOffset, 0);
			this._CachedTrans.offsetMax = -new Vector2(setOffset, 0);
		}
		*/
	}

	public void RecordScreenArea()
	{
		PlayerPrefsManager.Instance.SetAdaptScreenArea(setOffset);
	}

	public void SetScreenArea(float offset)
	{
		this._CachedTrans.offsetMin = new Vector2(offset, 0);
		this._CachedTrans.offsetMax = -new Vector2(offset, 0);
		AutoAdaptCloseButton("CloseButton");
	}


	protected void AutoAdaptCloseButton(string closeBtnPath)
	{
		if (this.CurStackMode == VIEW_STACK.FullOnly && this.Name != string.MainCityView)
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
		if (ViewLruObj != null)
			ResManager.Instance.KeepLruCache(ViewLruObj);
		else
			ResManager.Instance.KeepLruCache(Name.ToString());
		ActiveShow(true);
	}

	public void Hide()
	{
		//UIManager.Instance.RecoveryOrder((int)Layer);
		if (!UIManager.Instance.UINeedKeep(Name))
		{
			if (ViewLruObj != null)
				ResManager.Instance.UnkeepLruCache(ViewLruObj);
			else
				ResManager.Instance.UnkeepLruCache(Name.ToString());

			foreach (LruObj iGroup in _iGroups) ResManager.Instance.UnkeepLruCache(iGroup);
		}
		ActiveHide(true);

		//隐藏的时候自动移除功能评价注册
		if (!string.IsNullOrEmpty(EvaluateViewName))
		{
			UnRegisterEvaluateFunction();
		}
	}

	public void Destroy()
	{
		_Visible = false;
		UILogicEventDispatcher.Instance.RemoveListenerByHandler(this.OnReceiveLogicEvent, this);
		//TimerManager.Instance.DestroyByTarget(this);
		OnDestroy();
		GameUtil.DestroyUIObj(_uiGameObj);

	}

	//具体界面请勿调用
	public void ActiveShow(bool isActive)
	{
		UIManager.Instance.PushOrderStack(this);
		if (isActive)
		{
			UIManager.Instance.PushView(this);
		}


		if (IsAudio && isActive && Layer < VIEW_LAYER.Pop)
		{
			//播放音乐
			//PlayUISound(AudioDefine.Button_ViewOpen)
		}

		_Visible = true;

		_uiGameObj.SetActive(true);

		if (LuaMain.Instance.IsGameState(GameState.GAMESTATE_DEFINE.GameCity))
		{
			if (this.CurStackMode == VIEW_STACK.FullOnly
				&& this.Layer >= VIEW_LAYER.Default
				&& this.Name != string.MainCityView)
			{
				CameraManager.SetMainCameraActive(false);
			}
		}

		//TODO 特殊界面规则
		//if (Main.IsGameState(GAMESTATE_DEFINE.GameCity)) {
		//	if (CurStackMode == VIEW_STACK.FullOnly && Layer == VIEW_LAYER.Default && Name != VIEW.MainCityView) {
		//		//CityManager.SetCameraEnable(false)
		//	}

		//	if (CurStackMode == VIEW_STACK.FullOnly && Layer == VIEW_LAYER.Default){
		//		//&& Name != VIEW.MainCityView && Name != VIEW.LoadingView && Name != VIEW.QualifyingLoadingView
		//		//&& Name != VIEW.FBLoadingView && Name != VIEW.MatchTipsView && Name != VIEW.TeamBattleLoadingView) {
		//		//if (QualifyingManager.GetMatchTime() > 0) {
		//		//	UIManager.ShowView(VIEW.MatchTipsView)
		//		//}

		//		//if (PeakFightManager.GetInTheMatch())
		//		//{
		//		//	UIManager.ShowView(ViewPackage.string.MatchTipsView)
		//		//}

		//		//if (ArenaManager.GetInTheMatch())
		//		//{
		//		//	UIManager.ShowView(VIEW.MatchTipsView)
		//		//}
		//	}

		//}
		//else {
		//	//UIManager.HideView(VIEW.MatchTipsView);
		//}

#if UNITY_EDITOR
		m_OnShowItemComponentCount = ItemComponentPool.PoolItemCount;
#endif

		AutoAdaptScreenSafeArea();
		UIRoot.onAspectChanged.AddListener(OnAspectChanged);

		OnShow();
		UIManager.Instance.SetOrder(this, (int)Layer);
		//发送界面显示引导事件 todo
		//SendGuideEvent(GuideEvent.AfterViewOnShow, self.Name)
		UILogicEventDispatcher.Instance.SendWithArgs(E_UILogicEventType.OnViewShow, this.Name);

		AfterShow(!isActive);
	}

	public void ActiveHide(bool isActive)
	{
		UIRoot.onAspectChanged.RemoveListener(OnAspectChanged);
		if (_CachedTrans == null) return;
		UIManager.Instance.PopOrderStack(this);

		if (LuaMain.Instance.IsGameState(GameState.GAMESTATE_DEFINE.GameCity))
		{
			if (CurStackMode == VIEW_STACK.FullOnly && Layer >= VIEW_LAYER.Default && Name != string.MainCityView)
			{
				CameraManager.SetMainCameraActive(true);
			}
		}

		if (_Visible)
		{
			// 避免销毁时重复调用OnHide
			_Visible = false;
			OnHide();

			if (_uiGameObj != null)
			{
				_uiGameObj.SetActive(false);
			}
			UILogicEventDispatcher.Instance.SendWithArgs(E_UILogicEventType.OnViewHide, this.Name);
		}

		//发送界面隐藏引导事件
		//SendGuideEvent(GuideEvent.AfterViewOnHide, self.Name)

		if (_uiGameObj != null && _uiGameObj.activeSelf)
		{
			_uiGameObj.SetActive(false);
		}

		if (isActive)
		{
			UIManager.Instance.PopView(this);
		}

		if (IsAudio && isActive && Layer < VIEW_LAYER.Pop)
		{
			AudioUtility.PlayUISound(AudioDefine.Button_ViewClose);
		}

#if UNITY_EDITOR
		if (m_OnShowItemComponentCount != ItemComponentPool.PoolItemCount)
		{
			//LogManager.Instance.LogErrorFormat("ItemComponent 界面{0}可能存在对象池未释放 BeforeShow:{1} AfterHide:{2}", this.GetType().Name, m_OnShowItemComponentCount, ItemComponentPool.PoolItemCount);
		}
#endif
		UIManager.Instance.RemoveOrder(this, (int)Layer);
	}

	private void OnAspectChanged()
	{
		if (this._CachedTrans == null)
		{
			UIRoot.onAspectChanged.RemoveListener(OnAspectChanged);
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
			panelDict = new Dictionary<System.Type, IGroup>();
		}

		// 获取UI预制名称
		var t = typeof(T);
		string uiPrefabName;
		if (string.IsNullOrEmpty(replacePrefabName))
		{
			uiPrefabName = t.Name;
		}
		else
		{
			uiPrefabName = replacePrefabName;
		}

		IGroup panel;
		if (!panelDict.TryGetValue(t, out panel))
		{
			ResRefObj refObj = new ResRefObj();
			Tuple<UnityEngine.Object, string, LruObj> tuple = ResManager.Instance.LoadAssetIGroup(typeof(GameObject), uiPrefabName, path, refObj, ViewLruObj);
			GameObject prefab = (GameObject)tuple?.Item1;
			string fullPath = tuple?.Item2;
			LruObj groupLruObj = tuple?.Item3;

			_iGroups.Add(groupLruObj);
			GameObject uiObj = GameObject.Instantiate(prefab, subRoot);
			if (resetMatrix)
			{
				uiObj.transform.localPosition = Vector3.zero;
				uiObj.transform.localRotation = Quaternion.identity;
				uiObj.transform.localScale = Vector3.one;
			}
			panel = new T();
			panel.InitContainerByOwner(this, uiObj, groupLruObj);
			// groupLruObj!.OnDestroy += (lruObj) =>
			// {
			// 	panel.Destroy();
			// };
			panelDict.Add(t, panel);
		}

		executeBeforeShow?.Invoke((T)panel);
		panel.Show();
		panel.showCount++;
		J2Statistics.SendClickFunction(panel.UiGameObj.name, panel.showCount);

		return (T)panel;
	}

	protected IGroup ShowPanel(string panelName, Transform subRoot, string path, bool resetMatrix = false, string replacePrefabName = null, System.Action<IGroup> executeBeforeShow = null)
	{
		var panelType = typeof(UIManager).Assembly.GetType(panelName);

		if (!typeof(IGroup).IsAssignableFrom(panelType))
		{
			return null;
		}

		if (panelDict == null)
		{
			panelDict = new Dictionary<Type, IGroup>();
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
			ResRefObj refObj = new ResRefObj();
			Tuple<UnityEngine.Object, string, LruObj> tuple = ResManager.Instance.LoadAssetIGroup(typeof(GameObject), uiPrefabName, path, refObj, ViewLruObj);
			GameObject prefab = (GameObject)tuple?.Item1;
			string fullPath = tuple?.Item2;
			LruObj groupLruObj = tuple?.Item3;

			_iGroups.Add(groupLruObj);
			GameObject uiObj = GameObject.Instantiate(prefab, subRoot);
			if (resetMatrix)
			{
				uiObj.transform.localPosition = Vector3.zero;
				uiObj.transform.localRotation = Quaternion.identity;
				uiObj.transform.localScale = Vector3.one;
			}
			panel = TypeFactory.Create(panelType) as IGroup;
			panel.InitContainerByOwner(this, uiObj, groupLruObj);
			// groupLruObj!.OnDestroy += (lruObj) =>
			// {
			// 	if(panel != null && panel.IsValid()) panel?.Destroy();
			// };
			panelDict.Add(panelType, panel);
		}

		executeBeforeShow?.Invoke(panel);
        panel.Show();
		panel.showCount++;
		J2Statistics.SendClickFunction(panel.UiGameObj.name, panel.showCount);

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
			LogManager.Instance.LogError("必须为IGroup类型");
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
		//容错，先不加
		// foreach (LruObj iGroup in _iGroups) ResManager.Instance.UnkeepLruCache(iGroup);
		_iGroups.Clear();
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

	public virtual void OnReceiveKeycodeEvent(InputCmd cmd, InputStatus status)
	{

	}

	public virtual void OnApplicationFoucs(bool focus)
	{
	}


	public virtual void OnApplicationPause(bool pause)
	{

	}

	protected virtual void HandleHighlightControl(int panel, int subPanel, int controlId, bool highlight)
	{
		GameObject obj = GetGuideControl(controlId, panel, subPanel, out _);
		if (obj == null)
			return;

		Canvas canvas = obj.GetComponent<Canvas>();
		GraphicRaycaster rayCaster = obj.GetComponent<GraphicRaycaster>();
		if (canvas == null)
		{
			canvas = obj.AddComponent<Canvas>();
		}
		if (rayCaster == null)
		{
			rayCaster = obj.AddComponent<GraphicRaycaster>();
		}

		if (highlight)
		{
			canvas.overrideSorting = true;
			canvas.sortingOrder = UIManager.Instance.GetLayerOffset(VIEW_LAYER.GuideTip);
			rayCaster.enabled = false;
		}
		else
		{
			canvas.overrideSorting = false;
			rayCaster.enabled = true;
		}
	}

	protected virtual void HandleGuidePanel(int guideId, int panel, int subPanel, int controlId, bool isForce)
	{
		GameObject guideObj = GetGuideControl(controlId, panel, subPanel, out var btnControl);
		if (guideObj == null || btnControl == null)
		{
			LogManager.Instance.LogWarningFormat("{0} 找不到界面 {1} 的子界面{2} {3}的控件{4}", guideId, this.GetType().Name, panel, subPanel, controlId);
			return;
		}

		if (btnControl is Toggle toggle)
		{
			UnityEngine.Events.UnityAction<bool> onToggleValueChanged = null;
			onToggleValueChanged = isOn =>
			{
				toggle.onValueChanged.RemoveListener(onToggleValueChanged);
				HidePanel<GuidePanel>();
				GuideManager.Instance.Continue(guideId);
			};
			toggle.onValueChanged.AddListener(onToggleValueChanged);
		}
		else if (btnControl is Button button)
		{
			UnityEngine.Events.UnityAction onClick = null;
			onClick = () =>
			{
				button.onClick.RemoveListener(onClick);
				HidePanel<GuidePanel>();
				GuideManager.Instance.Continue(guideId);
			};
			button.onClick.AddListener(onClick);
		}

		HidePanel<GuidePanel>();
		ShowPanel<GuidePanel>(this._CachedTrans, RES_PATH.UI_VIEW_NEW, false, null, p =>
		{
			p.ShowGuide(guideObj, isForce, 0, 0, -120, true, false);
		}).ShowJump(guideId > 0);
	}

	protected virtual void HandleHighlightControlRect(int guideId, int panel, int subPanel, int controlId, string text, int audioId, bool showRect, bool isForce)
	{
		GameObject guideObj = GetGuideControl(controlId, panel, subPanel, out var btnControl);
		if (guideObj == null)// || btnControl == null)
		{
			LogManager.Instance.LogErrorFormat("{0} 找不到界面 {1} 的子界面{2} {3}的控件{4}", guideId, this.GetType().Name, panel, subPanel, controlId);
			return;
		}

		//if (btnControl is Toggle toggle)
		//{
		//    UnityEngine.Events.UnityAction<bool> onToggleValueChanged = null;
		//    onToggleValueChanged = isOn =>
		//    {
		//        toggle.onValueChanged.RemoveListener(onToggleValueChanged);
		//        HidePanel<GuidePanel>();
		//        GuideManager.Instance.Continue(guideId);
		//    };
		//    toggle.onValueChanged.AddListener(onToggleValueChanged);
		//}
		//else if (btnControl is Button button)
		//{
		//    UnityEngine.Events.UnityAction onClick = null;
		//    onClick = () =>
		//    {
		//        button.onClick.RemoveListener(onClick);
		//        HidePanel<GuidePanel>();
		//        GuideManager.Instance.Continue(guideId);
		//    };
		//    button.onClick.AddListener(onClick);
		//}
		UIHelper.ShowGuideHighlightRect(guideId, text, audioId, guideObj.transform as RectTransform, showRect, isForce);
	}

	protected virtual GameObject GetGuideControl(int controlId, int panel, int subPanel, out IPointerClickHandler button)
	{
		button = null;
		return null;
	}

	protected void ShowGuidePanel(GameObject go, bool last = true, bool force = false, float offsetX = 0, float offsetY = 0, float rotate = -120)
	{
		TimerManager.Instance.AddTimer("CheckWeakGuide", 0.001f, (obj) =>
		{
			if (go != null && go.activeInHierarchy)
			{
				ShowPanel<GuidePanel>(this._CachedTrans, RES_PATH.UI_VIEW_NEW, false, null, p =>
				{
					p.ShowGuide(go, force, offsetX, offsetY, rotate);
					p.LastStep = last;
					GuideManager.Instance.RecordWeakGuide();
				});
			}
			else
			{
				GuideManager.Instance.ClearWeakGuide();
			}
		});
	}

	protected void ShowGuidePanelAfterDialog(GameObject go, bool last = true, bool force = false, float offsetX = 0, float offsetY = 0, float rotate = -120)
	{
		if (GuideManager.Instance.weakGuidingQuestData == null)
		{
			LogManager.Instance.LogError("ShowGuidePanelAfterDialog weakGuidingQuestData 为 null");
			return;
		}
		var dialogId = GuideManager.Instance.weakGuidingQuestData.config.uncompleteDialogue;
		var dialogs = ConfigHelper.GetDialogCfg(dialogId);
		if (dialogs == null || dialogs.Count == 0)
			ShowGuidePanel(go, last, force, offsetX, offsetY, rotate);
		else
			UILogicEventDispatcher.Instance.SendToOpenViewWithArgs(E_UILogicEventType.ShowDialogView, string.DialogView, dialogId, 0, true);
	}
}
