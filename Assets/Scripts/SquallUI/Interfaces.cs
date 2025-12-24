using UnityEngine;

namespace SquallUI
{
    public static class Interfaces
    {
        public static GameObject LoadUIPrefab(string viewName)
        {
            Debug.LogWarning("You need realize this function: LoadUIPrefab");
            //Demo
            string path = "TestUIs/" + viewName;
            GameObject uiPrefab = Resources.Load<GameObject>(path);
            return uiPrefab;
        }
    }
}
