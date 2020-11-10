using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _DMG;
    public float DMG
    {
        get { return _DMG; }
        set
        {
            if (DMG < 0)
                _DMG = 0;
            else
                _DMG = value;
        }

    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}

