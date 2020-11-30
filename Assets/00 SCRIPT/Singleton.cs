using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>: MonoBehaviour where T: MonoBehaviour
{
    private static T _instant = null;
    public static T instant
    {
        get
        {
            if (_instant == null)
            {
                T[] Ts = FindObjectsOfType<T>();
                if (Ts.Length > 0)
                    _instant = Ts[0];
                else
                {
                    GameObject g = new GameObject();
                    g.AddComponent<T>();
                    _instant = g.GetComponent<T>();
                }
            }
            return _instant;
        }
    }

    
}
