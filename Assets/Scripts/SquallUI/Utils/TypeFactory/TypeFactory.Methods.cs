using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TypeFactory
{
    private static Dictionary<System.Type, System.Func<object>> m_Creators = new Dictionary<System.Type, System.Func<object>>();

    public static void RegisterCreator<T>() where T : new()
    {
        var t = typeof(T);
        if (!m_Creators.TryGetValue(typeof(T), out var creator))
        {
            creator = () =>
            {
                return new T();
            };
            m_Creators.Add(t, creator);
        }
    }

    public static bool Contain(System.Type t)
    {
        return m_Creators.ContainsKey(t);
    }

    public static object Create(System.Type t)
    {
        if (m_Creators.TryGetValue(t, out var creator) && creator != null)
        {
            return creator();
        }
        return null;
        //return Activator.CreateInstance(t);
    }
}