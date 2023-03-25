using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject()
                    {
                        name = typeof(T).Name
                    };
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected void CreateInstance() { }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            Destroy(instance);
            instance = null;
        }
    }
}
