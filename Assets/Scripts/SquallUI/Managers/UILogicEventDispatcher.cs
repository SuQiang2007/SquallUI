using System;
using System.Collections.Generic;

/// <summary>
/// UI 逻辑事件派发
/// </summary>
public class UILogicEventDispatcher : Singleton<UILogicEventDispatcher>
{
    public class EventPackage
    {
        public int EventType;
        public object[] EventObject;

        public T Get<T>(int index)
        {
            T outValue;
            TryGet(index, out outValue);
            return outValue;
        }

        public bool TryGet<T>(int index, out T outValue)
        {
            if (EventObject == null || index >= EventObject.Length)
            {
                outValue = default(T);
                return false;
            }
            else
            {
                object obj = EventObject[index];
                if (obj == null)
                {
                    outValue = default(T);
                    return false;
                }

                System.Type t = typeof(T);
                System.Type objType = obj.GetType();

                if (t.IsAssignableFrom(objType))
                {
                    outValue = (T)obj;
                    return true;
                }
                else if(t == typeof(bool))
                {
                    if (obj is System.IConvertible convertible)
                    {
                        outValue = (T)(object)convertible.ToBoolean(null);
                        return true;
                    }
                }
                else if (IsIntegerNumberic(t) && IsIntegerNumberic(objType))
                {
                    if (obj is System.IConvertible convertible)
                    {
                        try
                        {
                            var convertedObj = convertible.ToType(t, null);
                            outValue = (T)convertedObj;
                            return true;
                        }
                        catch(Exception err)
                        {
                            SLog.LogError(err);
                            outValue = default(T);
                            return false;
                        }
                    }
                }

                outValue = default(T);
                return false;
            }
        }

        private bool IsIntegerNumberic(System.Type t)
        {
            if (t.IsEnum)
                return true;

            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }
    }

    public delegate void EventHandler(EventPackage a);
    public class EventHandlerData
    {
        public EventHandler handler;
        public bool removed;
        public object arg;
    }

    private Dictionary<int, List<EventHandlerData>> _dictEventList = new Dictionary<int, List<EventHandlerData>>();

    private List<List<EventHandlerData>> _tempListPoop = new List<List<EventHandlerData>>();

    private List<EventHandlerData> getList()
    {
        List<EventHandlerData> retValue = null;
        if (_tempListPoop.Count > 0)
        {
            retValue = _tempListPoop[0];
            _tempListPoop.RemoveAt(0);
        }
        else
            retValue = new List<EventHandlerData>();

        return retValue;

    }

    // 归还池子
    private void releaseList(List<EventHandlerData> list)
    {
        if (list == null)
            return;

        list.Clear();

        _tempListPoop.Add(list);
    }

    // 发送逻辑事件包
    private void sendPackage(EventPackage package)
    {
        List<EventHandlerData> eventTable = _dictEventList.ContainsKey(package.EventType) ?  _dictEventList[package.EventType] : null;
        if(eventTable != null)
        {
            List<EventHandlerData> tempList = getList();

            for(int i = 0; i < eventTable.Count; i++)
            {
                tempList.Add(eventTable[i]);
            }

    
            for (int i = 0; i < tempList.Count; i++)
            {
                EventHandlerData eventCallBack = tempList[i];
                if (!eventCallBack.removed && eventCallBack.handler != null)
                    eventCallBack.handler(package);
                //else
                //    eventCallBack(package);
            }

            releaseList(tempList);
       
        }
             
    }


   // 发送逻辑事件，只包含事件类型,但不传递参数和数据
    public void Send(int eType)
    {
        EventPackage eventPackage = new EventPackage();
        eventPackage.EventType = eType;
        sendPackage(eventPackage);
    }

    public void SendWithArgs(int eType, params object[] arags)
    {
        EventPackage eventPackage = new EventPackage();
        eventPackage.EventType = eType;
        eventPackage.EventObject = arags;
        sendPackage(eventPackage);
    }

    // 打开界面，并向界面发送逻辑事件包
    public void SendToOpenViewPackage(string viewName, EventPackage package, bool forcSync = false)
    {
        if (!SquallUIMgr.Instance.IsViewVisible(viewName))
            SquallUIMgr.Instance.ShowViewByEvent(viewName, package, forcSync);
        else
            SquallUIMgr.Instance.ReceiveLogicEvent(viewName, package);
    }

    // 打开界面，并向界面发送逻辑事件，但不传递参数和数据
    public void SendToOpenView(int eType, string viewName)
    {
        EventPackage eventPackage = new EventPackage();
        eventPackage.EventType = eType;
        SendToOpenViewPackage(viewName, eventPackage);
    }
    
    // 打开界面，并向界面发送逻辑事件，并可发送携带任意可变参数
    public void SendToOpenViewWithArgs(int eventType, string viewName, params object[] args)
    {
        EventPackage eventPackage = new EventPackage();
        eventPackage.EventType = eventType;
        eventPackage.EventObject = args;
        SendToOpenViewPackage(viewName, eventPackage);
    }

    public void SendToOpenViewSyncWithArgs(int eventType, string viewName, params object[] args)
    {
        EventPackage eventPackage = new EventPackage();
        eventPackage.EventType = eventType;
        eventPackage.EventObject = args;
        SendToOpenViewPackage(viewName, eventPackage, true);
    }

    // 逻辑事件监听，可对具体类型事件进行监听，监听后将在事件触发时调用回调方法，接受数据进行处理

    public void AddListener(int eventType, EventHandler handler)
    {
        List<EventHandlerData> eventTable = _dictEventList[eventType];
        if (eventTable == null)
        {
            eventTable = new List<EventHandlerData>();
            _dictEventList[eventType] = eventTable;
        }

        for(int i = 0; i < eventTable.Count; i++)
        {
            EventHandlerData eventCallBack = eventTable[i];
            if(eventCallBack.handler == handler)
                return;
        }

        EventHandlerData eventCallBackTable = new EventHandlerData();
        eventCallBackTable.handler = handler;
        eventTable.Add(eventCallBackTable);

    }

    // 逻辑事件监听，可对具体类型事件进行监听，监听后将在事件触发时调用回调方法，接受数据进行处理
    public void AddListenerWithArg(int eventType, EventHandler handler, object arg)
    {
        List<EventHandlerData> eventTable = _dictEventList.ContainsKey(eventType) ? _dictEventList[eventType] : null;
        if (eventTable == null) 
        {
            eventTable = new List<EventHandlerData>();
            _dictEventList[eventType] = eventTable;
        }

        for(int i = 0; i < eventTable.Count; i++)
        {
            EventHandlerData eventCallBack = eventTable[i];
            if (eventCallBack.handler == handler && eventCallBack.arg == arg)
                return;
            
        }

        EventHandlerData eventCallBackTable = new EventHandlerData();
        eventCallBackTable.handler = handler;
        eventCallBackTable.arg = arg;
        eventTable.Add(eventCallBackTable);
    }

    // 移除逻辑事件监听，对某个逻辑事件移除注册的回调方法
    public void RemoveListener(int eventType, EventHandler eventHandler, object arg)
    {
        List<EventHandlerData> eventTable = _dictEventList.ContainsKey(eventType) ? _dictEventList[eventType] : null;
        if (eventTable == null)
            return;

        for (int i = 0; i < eventTable.Count; i++)
        {
            EventHandlerData eventCallBack = eventTable[i];
            if (eventCallBack.handler == eventHandler && eventCallBack.arg == arg)
            {
                eventCallBack.removed = true;
                eventTable.RemoveAt(i);
                break;
            }
        
        }
    }


    // 移除与回调方法相关的所有逻辑事件注册
    public void RemoveListenerByHandler(EventHandler eventHandler, object arg)
    {
        foreach(var kv in _dictEventList)
        {
            if (kv.Value != null)
            {
                for(int i = kv.Value.Count - 1; i >= 0; i --)
                {
                    EventHandlerData eventCallBack = kv.Value[i];
                    if (eventCallBack != null)
                    {
                        if (eventCallBack.handler == eventHandler && eventCallBack.arg == arg)
                        {
                            eventCallBack.removed = true;
                            kv.Value.RemoveAt(i);
                        }
                    }
                }
            }
        }


    }
   

    // 清除所有注册逻辑事件表
    public void Clear()
    {
        _dictEventList = null;
        _tempListPoop = null;
    }
}