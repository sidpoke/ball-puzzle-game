using UnityEngine;

/// <summary>
/// A Singleton base class that transforms a Monobehaviour into a globally accessible static class.
/// </summary>
/// <typeparam name="T">A Monobehaviour that needs to be referenced globally as a singleton.</typeparam>
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
            Debug.Log($"Singleton {name} already exists. Destroying instance with id {GetInstanceID()}.");
            Destroy(gameObject);
        }
    }
}
