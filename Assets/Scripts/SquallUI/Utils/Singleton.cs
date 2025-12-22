using System;

/// <summary>
/// 单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class, new()
{
    protected static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Activator.CreateInstance<T>();
            }
            return m_Instance;
        }
    }

    public virtual void Dispose()
    {
        m_Instance = null;
    }
}