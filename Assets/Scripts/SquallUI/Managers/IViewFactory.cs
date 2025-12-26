using System;
using System.Collections;
using SquallUI.Classes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SquallUI
{
    public class IViewFactory
    {
        /// <summary>
        /// 加载界面预制体。默认返回序列化配置，可根据项目接入资源管理器。
        /// </summary>
        public virtual GameObject LoadUIPrefab(string viewName)
        {
            GameObject prefab = Interfaces.LoadUIPrefab(viewName);
            if (prefab == null)
            {
                Debug.LogError($"未实现 UI Prefab，无法创建：{viewName}");
                return null;
            }

            return prefab;
        }

        /// <summary>
        /// 同步创建界面实例。
        /// </summary>
        public virtual IView CreateView(string viewName)
        {
            var prefab = LoadUIPrefab(viewName);
            if (prefab == null)
                return null;

            GameObject viewObj = Object.Instantiate(prefab);
            viewObj.name = viewName;

            IView view = CreateViewInstance(viewName);
            if (view != null)
            {
                view.InitContainer(viewObj, viewName);
                return view;
            }

            Object.Destroy(viewObj);
            return null;
        }

        internal T CreateGroup<T>(string viewName, IControlContainer parentView, Transform parentTrans) where T : IGroup, new()
        {
            GameObject prefab = LoadUIPrefab(viewName);
            GameObject uiObj = Object.Instantiate(prefab, parentTrans);
            T panel = new T();
            panel.InitContainerByOwner(parentView, uiObj);
            return panel;
        }

        internal IGroup CreateGroup(Type groupType, string viewName, IControlContainer parentView)
        {
            GameObject uiObj = LoadUIPrefab(viewName);
            if (TypeFactory.Create(groupType) is IGroup panel)
            {
                panel.InitContainerByOwner(parentView, uiObj);
                return panel;
            }

            SLog.LogError($"Create group failed:{viewName}");
            return null;
        }

        /// <summary>
        /// 异步创建界面实例（默认延迟一帧，便于替换为真正的异步加载）。
        /// </summary>
        public virtual IEnumerator LoadViewAsync(string viewName, Action<IView> onLoaded)
        {
            // 保留一帧延迟作为占位，实际接入异步资源系统时可替换。
            yield return null;

            var prefab = LoadUIPrefab(viewName);
            if (prefab == null)
            {
                onLoaded?.Invoke(null);
                yield break;
            }

            GameObject viewObj = Object.Instantiate(prefab);
            viewObj.name = viewName;

            IView view = CreateViewInstance(viewName);
            if (view != null)
            {
                view.InitContainer(viewObj, viewName);
                onLoaded?.Invoke(view);
            }
            else
            {
                SLog.LogError($"Create view controller failed:{viewName}");
                Object.Destroy(viewObj);
                onLoaded?.Invoke(null);
            }
        }

        protected virtual IView CreateViewInstance(string viewName)
        {
            return GetViewInstance(viewName);
        }
        
        private static IView GetViewInstance(string viewName)
        {
            IView ret = null;
            switch (viewName)
            {
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
}
