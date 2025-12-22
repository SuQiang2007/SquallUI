using System.Collections.Generic;
using UnityEngine;

namespace SquallUI.Classes
{
    /// <summary>
    /// UI对象控件容器抽象
    /// </summary>
    public class IControlContainer
    {
        protected GameObject _uiGameObj = null;
        public GameObject UiGameObj { get => _uiGameObj; protected set => _uiGameObj = value; }
        // 增加逻辑事件监听, 请注意移除!
        public void AddLogicEventListener(int eventType)
        {
            UILogicEventDispatcher.Instance.AddListenerWithArg(eventType, OnReceiveLogicEvent, this);
        }

        // 移除逻辑事件监听
        public void RemoveLogicEventListener(int eventType)
        {
            UILogicEventDispatcher.Instance.RemoveListener(eventType, OnReceiveLogicEvent, this);
        }

        // 接收逻辑事件回调，需在此方法内处理分发
        public virtual void OnReceiveLogicEvent(UILogicEventDispatcher.EventPackage package)
        {

        }

        // 接收逻辑事件回调，需在此方法内处理分发
        public virtual void OnReceiveLogicEvent(object package)
        {

        }

        // 获取子类Obj
        public GameObject GetChildObj(string childPath)
        {
            if (this._uiGameObj == null)
                return null;

            var trans = this._uiGameObj.transform.Find(childPath);
            return trans != null ? trans.gameObject : null;
        }

        // 获取子类组件
        public Component GetChildCompByObj(string childPath, string comp)
        {
            if (this._uiGameObj == null)
                return null;

            return this._uiGameObj.transform.Find(childPath).GetComponent(comp);
        }

        public T GetChildCompByObj<T>(string childPath) where T : Component
        {
            if (this._uiGameObj == null)
                return null;

            var trans = this._uiGameObj.transform.Find(childPath);
            return trans != null ? trans.GetComponent<T>() : null;
        }

        // 克隆同级物体
        public GameObject CloneTemplate(GameObject template)
        {
            if (template == null)
                return null;

            GameObject cloneObj = UnityEngine.GameObject.Instantiate(template);
            cloneObj.transform.SetParent(template.transform.parent);
            cloneObj.transform.localPosition = template.transform.localPosition;
            cloneObj.transform.localScale = template.transform.localScale;
            return cloneObj;
        }

        // 克隆物体
        public GameObject ClonePrefab(GameObject template)
        {
            if (template == null)
                return null;

            GameObject cloneObj = GameObject.Instantiate(template);
            cloneObj.transform.localPosition = Vector3.zero;
            cloneObj.transform.localScale = Vector3.one;
            cloneObj.transform.localRotation = Quaternion.identity;
            return cloneObj;
        }

        public T CloneTemplate<T>(T template) where T : Component
        {
            if (template == null)
                return null;

            T cloneObj = UnityEngine.GameObject.Instantiate(template);
            cloneObj.transform.SetParent(template.transform.parent);
            cloneObj.transform.localPosition = template.transform.localPosition;
            cloneObj.transform.localScale = template.transform.localScale;
            return cloneObj;
        }

        private Dictionary<GameObject, List<GameObject>> groupPools = new Dictionary<GameObject, List<GameObject>>();
        private Dictionary<GameObject, GameObject> spawnedObject = new Dictionary<GameObject, GameObject>();

        // 克隆同级组容器
        public void CloneGroup(GameObject template, IGroup groupInstance)
        {
            List<GameObject> poolObjs;
            if (!groupPools.TryGetValue(template, out poolObjs))
            {
                poolObjs = new List<GameObject>();
                groupPools[template] = poolObjs;
            }

            GameObject cloneObj;
            if (poolObjs.Count > 0)
            {
                cloneObj = poolObjs[poolObjs.Count - 1];
                poolObjs.RemoveAt(poolObjs.Count - 1);
            }
            else
            {
                cloneObj = CloneTemplate(template);
            }

            if (cloneObj == null)
            {
                SLog.Log("Warning: Clone group error.cloneObj is null");
                return;
            }

            spawnedObject.Add(cloneObj, template);
            cloneObj.SetActive(true);
            cloneObj.transform.SetAsLastSibling();
            if (groupInstance != null)
                groupInstance.InitContainer(cloneObj);
        }

        public void RecycleGroup(GameObject obj, IGroup groupInstance)
        {
            if (groupInstance != null)
            {
                groupInstance.OnDestroy();
            }

            GameObject template;
            if (!spawnedObject.TryGetValue(obj, out template) || template == null)
            {
                Object.Destroy(obj);
                return;
            }

            spawnedObject.Remove(obj);
            List<GameObject> pool = new List<GameObject>();
            if (!groupPools.TryGetValue(template, out pool))
            {
                Object.Destroy(obj);
                return;
            }

            pool.Add(obj);
            obj.SetActive(false);
        }

        public void CloneGroupByContainer(IControlContainer ownerContainer, GameObject template, IGroup groupInstance)
        {
            GameObject cloneObj = CloneTemplate(template);
            if (cloneObj == null)
            {
                SLog.Log("Warning: Clone group error.cloneObj is null");
                return;
            }

            if (groupInstance != null)
                groupInstance.InitContainerByOwner(ownerContainer, cloneObj);

        }
    }
}
