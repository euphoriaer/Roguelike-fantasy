using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object syncLock = new object();

    public static T Instate
    {
        get
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        var manager = GameObject.FindObjectOfType<T>();
                        if (manager == null)
                        {
                            var gameObject = new GameObject();
                            manager = gameObject.AddComponent<T>();
                        }
                        instance = manager;
                    }
                }
            }
            return instance;
        }
    }
}