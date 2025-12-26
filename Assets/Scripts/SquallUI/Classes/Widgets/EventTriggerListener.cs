using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void VoidDelegate(GameObject go);

public delegate void VoidDelegatePoint(GameObject go, PointerEventData data);

public class EventTriggerListener : EventTrigger
{
    public VoidDelegate onClick;
    public VoidDelegatePoint onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onBeginLongPress;
    public VoidDelegate onLongPress;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegatePoint onBeginDrag;
    public VoidDelegatePoint onDrag;
    public VoidDelegatePoint onEndDrag;

    private bool isDragging;

    private bool isPointerDown = false;
    private bool longPressTriggered = false;
    private float longPressStartTime = 0;

    public float longPressThreshold = 1.0f;
    public bool ignoreLongPressWhenDragging = false;

    // 消息传递
    private bool isPointerClickPassEvent = false;
    private bool isCallClickAfterPassEvent = false;
    // 是否传递拖拽事件
    private bool isPointerDragPassEvent = false;
    // 是否传递事件到父节点，而不是穿透
    private bool passEventToParent = false;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
            listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    static public EventTriggerListener Get(UIBehaviour behaviour)
    {
        EventTriggerListener listener = behaviour.GetComponent<EventTriggerListener>();
        if (listener == null)
            listener = behaviour.gameObject.AddComponent<EventTriggerListener>();
        return listener;
    }

    static public void Remove(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener != null)
        {
            Destroy(listener);
        }
    }

    static public void Remove(UIBehaviour behaviour)
    {
        EventTriggerListener listener = behaviour.GetComponent<EventTriggerListener>();
        if (listener != null)
        {
            Destroy(listener);
        }
    }

    static public void Clear(UIBehaviour go, bool isRemove = false)
    {
        var eventListeners = go.GetComponentsInChildren<EventTriggerListener>(true);
        foreach (var eventListener in eventListeners)
        {
            eventListener.Clear();
            if (isRemove)
                Destroy(eventListener);
        }
    }

    static public void Clear(GameObject go, bool isRemove = false)
    {
        var eventListeners = go.GetComponentsInChildren<EventTriggerListener>(true);
        foreach (var eventListener in eventListeners)
        {
            eventListener.Clear();
            if (isRemove)
                Destroy(eventListener);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!isCallClickAfterPassEvent && !isDragging && onClick != null)
            onClick(gameObject);
        if (this.isPointerClickPassEvent)
            PassEvent(eventData, ExecuteEvents.pointerClickHandler);
        if (isCallClickAfterPassEvent && !isDragging && onClick != null)
            onClick(gameObject);
    }

    static public void SetPointerClickPassEvent(GameObject go, bool isPassEvent, bool isCallAfterPassEvent = false)
    {
        var eventListeners = go.GetComponentsInChildren<EventTriggerListener>(true);
        foreach (var eventListener in eventListeners)
        {
            eventListener.isPointerClickPassEvent = isPassEvent;
            eventListener.isCallClickAfterPassEvent = isCallAfterPassEvent;
        }
    }

    static public void SetDragPassEvent(GameObject go, bool isPassEvent)
    {
        var eventListeners = go.GetComponentsInChildren<EventTriggerListener>(true);
        foreach (var eventListener in eventListeners)
        {
            eventListener.isPointerDragPassEvent = isPassEvent;
        }
    }

    static public void SetPassEventToParent(GameObject go, bool passToParent)
    {
        var eventListeners = go.GetComponentsInChildren<EventTriggerListener>(true);
        foreach (var eventListener in eventListeners)
        {
            eventListener.passEventToParent = passToParent;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        longPressStartTime = Time.time;
        isPointerDown = true;
        longPressTriggered = false;
        if (onDown != null)
        {
            onDown(gameObject, eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
        {
            onEnter(gameObject);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
        if (onExit != null)
        {
            onExit(gameObject);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (onUp != null)
        {
            onUp(gameObject);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null)
        {
            onSelect(gameObject);
        }
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null)
        {
            onUpdateSelect(gameObject);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        if (onBeginDrag != null)
        {
            onBeginDrag(gameObject, eventData);
        }
        if (isPointerDragPassEvent)
            PassEvent(eventData, ExecuteEvents.beginDragHandler);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        if (onDrag != null)
        {
            onDrag(gameObject, eventData);
        }
        if (isPointerDragPassEvent)
            PassEvent(eventData, ExecuteEvents.dragHandler);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (onEndDrag != null)
        {
            onEndDrag(gameObject, eventData);
        }
        if (isPointerDragPassEvent)
            PassEvent(eventData, ExecuteEvents.endDragHandler);
    }

    private void Update()
    {
        if (!(ignoreLongPressWhenDragging && isDragging) && isPointerDown && Time.time - longPressStartTime >= longPressThreshold)
        {
            if (!longPressTriggered)
            {
                longPressTriggered = true;
                if (onBeginLongPress != null)
                {
                    onBeginLongPress(gameObject);
                }
            }
            else if (onLongPress != null)
            {
                onLongPress(gameObject);
            }
        }
    }

    public void Clear()
    {
        onClick = null;
        onDown = null;
        onEnter = null;
        onExit = null;
        onUp = null;
        onBeginLongPress = null;
        onLongPress = null;
        onSelect = null;
        onUpdateSelect = null;
        onBeginDrag = null;
        onDrag = null;
        onEndDrag = null;
    }

    private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        if (passEventToParent)
        {
            if (transform.parent != null)
            {
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, data, function);
            }
        }
        else
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            foreach (var result in results)
            {
                if (result.gameObject == gameObject)
                    continue;

                if (ExecuteEvents.ExecuteHierarchy(result.gameObject, data, function))
                    break;
            }
        }
    }
}