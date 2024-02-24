using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null) { 
            Instance = FindObjectOfType<T>();
            DontDestroyOnLoad(gameObject);
        }
        else {
            Debug.LogError($"Singleton {name} already exists. Destroying instance with id {GetInstanceID()}.");
            Destroy(gameObject);
        }
    }
}
