using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] float FireDelay = 0.3f,LifeTime = 3,Daamage = 10;
    float CountDownTimer = 0;
    [SerializeField] Vector2 ForceBullet = Vector2.zero;
    
    private void Update()
    {
        if (CountDownTimer > 0)
            CountDownTimer -= Time.deltaTime;
    }
    // Start is called before the first frame update
    public void Fire(float direction)
    {
        if (CountDownTimer > 0)
            return;
        GameObject tmp = Instantiate(Bullet,this.transform.position,Quaternion.identity);
        tmp.GetComponent<Rigidbody2D>().AddForce(direction * ForceBullet);
        tmp.GetComponent<Bullet>().DMG = Daamage;
        Destroy(tmp, LifeTime);
        CountDownTimer = FireDelay;
    }
}
