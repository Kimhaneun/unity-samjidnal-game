using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _quitingApplication = false; 
    private static object _lock = new object(); 
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_quitingApplication) 
            {
                return null;
            }

            lock (_lock) 
            {
                if (_instance == null) 
                {
                    _instance = (T)FindObjectOfType(typeof(T)); 

                    if (_instance == null) 
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singletonObject); 
                    }
                    DontDestroyOnLoad(_instance);
                }
                return _instance; 
            }
        }
    }

    private void OnApplicationQuit() 
    {
        _quitingApplication = true;
    }

    private void OnDestroy()
    {
        _quitingApplication = true;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60; 
    }
}