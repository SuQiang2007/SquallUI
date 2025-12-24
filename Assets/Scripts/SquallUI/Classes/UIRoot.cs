using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// UI根节点标识对象;
/// </summary>
public class UIRoot : MonoBehaviour
{
    public Camera uiCamera;
    
    public const int DESIGN_WIDTH = 1920;
    public const int DESIGN_HEIGHT = 1080;

    public const int BG_DESIGN_WIDTH = 2480;
    public const int BG_DESIGN_HEIGHT = 1080;

    /// <summary>
    /// UI根节点实例;
    /// </summary>
    private static UIRoot m_Instance = null;

    private static Canvas _mCanvas = null;

    private EventSystem _eventSystem;
    
    //可能用于坐标转换
    public Canvas RootCanvas => _mCanvas;

    public static UIRoot Instance => m_Instance;

    /// <summary>
    /// UI根节点缓存组件;
    /// </summary>
    private Transform _mTrans;

    public Transform Trans => _mTrans;

    private CanvasScaler _scaler;

    public CanvasScaler Scaler => _scaler;


    private Vector2 _mLastRectSize;

    public static UnityEvent OnAspectChanged { get; private set; } = new UnityEvent();

    //供外部获取屏幕宽高
    private RectTransform _rt;
    public RectTransform RectTransform => _rt;

    private void Awake()
    {
        //解决UGUI的拖动和点击的阈值问题，避免高分辨率设备拖动过于灵敏
        // EventSystem.current.pixelDragThreshold = Screen.height / 50;
        if (UIRoot.Instance != null)
        {
            throw new UnityException("UIRoot can't duplicate!");
        }
        m_Instance = this;
        _eventSystem = transform.Find("EventSystem").GetComponent<EventSystem>();
        _mTrans = transform;
        _mCanvas = transform.GetComponent<Canvas>();
        _rt = GetComponent<RectTransform>();
        _scaler = transform.GetComponent<CanvasScaler>();
        _mLastRectSize = _rt.sizeDelta;

        
        //Adjust();
    }

    private void OnEnable()
    {
        Canvas.preWillRenderCanvases += BeforeRenderer;
        //RenderPipelineManager.beginContextRendering += BeforeRenderer;
    }

    private void OnDisable()
    {
        Canvas.preWillRenderCanvases -= BeforeRenderer;
        //RenderPipelineManager.beginContextRendering -= BeforeRenderer;
    }

    private void BeforeRenderer()
    {
        // 不用Screen.width
        // 因为FullBackImage和FullScreenImage中用的是rt.sizeDelta计算
        // 屏幕发生变化时，rt的大小还没变
        if (_mLastRectSize != _rt.sizeDelta)
        {
            OnAspectChanged.Invoke();
            _mLastRectSize = _rt.sizeDelta;
        }
    }

    public void SetEventSystemEnabled(bool isEnabled)
    {
        _eventSystem.enabled = isEnabled;
    }
}