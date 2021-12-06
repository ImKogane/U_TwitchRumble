using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
{
    private static T _instance;

    public static T Instance
    {
        get => _instance;
    }

    public abstract bool DestroyOnLoad { get; }

    protected virtual void Awake()
    {
        if (!_instance)
        {
            _instance = (T)this;

            if (!DestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
