using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineInstance : MonoBehaviour
{
    private static CoroutineInstance m_Instance;

    public static Coroutine BeginCoroutine(IEnumerator routine)
    {
        if (m_Instance == null)
        {
            GameObject routineHandlerObject = new GameObject("CoroutineInstance");
            routineHandlerObject.hideFlags = HideFlags.HideAndDontSave;
            m_Instance = routineHandlerObject.AddComponent<CoroutineInstance>();
            GameObject.DontDestroyOnLoad(routineHandlerObject);
        }

        return m_Instance.StartCoroutine(routine);
    }

    public static void EndCoroutine(Coroutine coroutine)
    {
        if (m_Instance == null)
            return;

        m_Instance.StopCoroutine(coroutine);
    }

    public static void EndCoroutine(IEnumerator coroutine)
    {
        if (m_Instance == null)
            return;

        m_Instance.StopCoroutine(coroutine);
    }
}