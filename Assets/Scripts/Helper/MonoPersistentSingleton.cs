
using UnityEngine;

public class MonoPersistentSingleton<T> : MonoSingleton<T> where T : Component
{
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this as T)
        {
            DestroyImmediate(gameObject);
            return;
        }
    }
}
