using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLog
{
    public static void LogError(object message)
    {
        // string trackStr = new System.Diagnostics.StackTrace().ToString();
        Debug.LogError(message);
    }

    public static void Log(string message)
    {
        Debug.Log(message);
    }
}
