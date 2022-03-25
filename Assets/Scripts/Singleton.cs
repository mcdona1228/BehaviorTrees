using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T this_Instance;
    public static bool this_currentlyQuitting;

    public static T Instance
    {
        get
        {
            if(this_Instance == null)
            {
                this_Instance = FindObjectOfType<T>();

                if(this_Instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    this_Instance = obj.AddComponent<T>();
                }
            }
            return this_Instance;
        }
    }

    public virtual void Awake()
    {
        if(this_Instance == null)
        {
            this_Instance = this as T;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
