using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// UI根节点标识对象;
/// </summary>
public class UIRoot : MonoBehaviour
{
    public const int DESIGN_WIDTH = 1920;
    public const int DESIGN_HEIGHT = 1080;

    public const int BG_DESIGN_WIDTH = 2480;
    public const int BG_DESIGN_HEIGHT = 1080;

    /// <summary>
    /// UI根节点实例;
    /// </summary>
    private static UIRoot m_Instance = null;

    private static Canvas m_Canvas = null;

    private EventSystem eventSystem;
    public EventSystem EventSystem
    {
        get { return eventSystem; }
    }

    public Canvas rootCanvas
    {
        get { return m_Canvas; }
    }

    public static UIRoot Instance
    {
        get { return m_Instance; }
    }

    /// <summary>
    /// UI根节点缓存组件;
    /// </summary>
    private Transform m_Trans;

    public Transform Trans
    {
        get { return m_Trans; }
    }

    /// <summary>
    /// UI相机;
    /// </summary>
    private Camera m_UICamera;

    public Camera UICameara
    {
        get { return m_UICamera; }
    }

    private CanvasScaler scaler;

    public CanvasScaler Scaler
    {
        get
        {
            return scaler;
        }
    }

    public RectTransform rt;

    private Vector2 m_LastRectSize;

    public static UnityEvent onAspectChanged { get; private set; } = new UnityEvent();

    public RectTransform rectTransform
    {
        get
        {
            return rt;
        }
    }

    private void Awake()
    {
        //解决UGUI的拖动和点击的阈值问题，避免高分辨率设备拖动过于灵敏
        // EventSystem.current.pixelDragThreshold = Screen.height / 50;
        if (UIRoot.Instance != null)
        {
            throw new UnityException("UIRoot can't duplicate!");
        }
        m_Instance = this;
        eventSystem = transform.Find("EventSystem").GetComponent<EventSystem>();
        m_Trans = transform;
        m_UICamera = transform.Find("UICamera").GetComponent<Camera>();
        m_Canvas = transform.GetComponent<Canvas>();
        if (m_UICamera == null)
        {
            throw new UnityException("UICamera Not Found! Please Add UI Camera in UIRoot Child");
        }
        rt = GetComponent<RectTransform>();
        scaler = transform.GetComponent<CanvasScaler>();
        m_LastRectSize = rt.sizeDelta;

        
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
        if (m_LastRectSize != rt.sizeDelta)
        {
            onAspectChanged.Invoke();
            m_LastRectSize = rt.sizeDelta;
        }
    }
}