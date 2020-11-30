using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_base : MonoBehaviour
{
    [SerializeField] float LifeTime = 3;

    [SerializeField] int _Damage = 10;
    public int Damage => _Damage;

    TrailRenderer Trail;
    private void Awake()
    {
        Trail = this.GetComponent<TrailRenderer>();
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("DeActive", LifeTime);
        Trail.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DeActive()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
            collision.GetComponent<Enemy_base>().GetDamage(this);
        this.gameObject.SetActive(false);
    }
}
