using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float LifeTIme = 3;
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

    TrailRenderer Trail;

    private void Awake()
    {
        Trail = this.GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        Invoke("Deactive",LifeTIme);
    }

    private void OnDisable()
    {
        
    }

    void Deactive()
    {

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy_base>().GetDamage(this);
        }
        this.gameObject.SetActive(false);
        
    }
}

