using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] float FireDelay = 0.3f,Daamage = 10;
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
        
        GameObject tmp = BulletPooling.instant.GetObj();
        tmp.transform.position = this.transform.position;
        //tmp.GetComponent<Bullet>().DMG = Daamage;
        tmp.SetActive(true);

        tmp.GetComponent<Rigidbody2D>().velocity = direction * ForceBullet;
        CountDownTimer = FireDelay;
    }
}
