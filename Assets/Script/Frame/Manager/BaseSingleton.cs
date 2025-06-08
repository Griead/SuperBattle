using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance is null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}
