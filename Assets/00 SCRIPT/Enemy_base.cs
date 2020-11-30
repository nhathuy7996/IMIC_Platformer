using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour
{
    [SerializeField] float HP = 100;
    [SerializeField] Vector2 Speed;
    GameObject Player;
    float dir = 1;
    Rigidbody2D Rigi;
    [SerializeField] Vector2 offset = new Vector2(0,0.5f);
    Collider2D Colli;
    private void OnEnable()
    {
        HP = 100;
    }
    // Start is called before the first frame update
    void Start()
    {
        Rigi = this.GetComponent<Rigidbody2D>();
        Player = GameController.instant.Player;
        Colli = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }    

    void Move()
    {
        //dir = this.transform.position.x > Player.transform.position.x ? -1 : 1;
        offset.x = -dir;
        Debug.DrawRay(this.transform.position - (Vector3)offset,Vector2.down,Color.red);
        RaycastHit2D hit =  Physics2D.Raycast(this.transform.position - (Vector3)offset, Vector2.down, 1);
        if (hit.collider == null)
        {
            dir = -dir;
        }
        else
        {
            Debug.Log(hit.collider.name);
        }

        this.Rigi.velocity = new Vector2(dir * Speed.x * Time.deltaTime,this.Rigi.velocity.y);
    }

    public void GetDamage(Bullet_base B)
    {
        Debug.Log("GetHit!! " + this.HP);
        this.HP -= B.Damage;
        if (this.HP <= 0)
        {
            //Dead
            this.gameObject.SetActive(false);
        }
    }
}
