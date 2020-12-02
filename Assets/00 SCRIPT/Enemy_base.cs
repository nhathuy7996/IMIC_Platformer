using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour
{
    [SerializeField] float HP = 100, Atk_range = 10;
    [SerializeField] Vector2 Speed;
    GameObject Player;
    [SerializeField]float dir = 1;
    Rigidbody2D Rigi;
    [SerializeField] Vector2 offset = new Vector2(0,0.5f);
    Collider2D Colli;
    [SerializeField] bool IsChasePlayer = false;

    private void OnEnable()
    {
        HP = 100;
        IsChasePlayer = false;
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
        CheckTarget();
        Move();

    }    

    void Move()
    {
        if(IsChasePlayer)
            this.dir = this.transform.position.x > Player.transform.position.x ? -1 : 1;

        if (!CheckIsGround(this.dir))
        {
            this.dir = -this.dir;
        }

        this.Rigi.velocity = new Vector2(this.dir * Speed.x * Time.deltaTime,this.Rigi.velocity.y);
    }

    bool CheckIsGround(float dir)
    {
        offset.x = -dir;
        Debug.DrawRay(this.transform.position - (Vector3)offset, Vector2.down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position - (Vector3)offset, Vector2.down, 1);
        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void CheckTarget()
    {
        if (Vector2.Distance(Player.transform.position, this.transform.position) > Atk_range)
        {
            IsChasePlayer = false;
            return;
        }
        float dir = this.transform.position.x > Player.transform.position.x ? -1 : 1;
        Debug.Log(dir + "||" + CheckIsGround(dir));
        if (CheckIsGround(dir))
        {
            IsChasePlayer = true;
        }
        else
        {
            IsChasePlayer = false;
        }
       
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

    private void OnDrawGizmos()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, Atk_range);
    }
}
