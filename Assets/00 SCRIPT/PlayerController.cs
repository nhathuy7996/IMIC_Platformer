using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 Speed, CheckWallOffset, OffsetClimb;

    Rigidbody2D Rigi;
    [SerializeField] bool IsGround = true, IsDoubleJump = false, IsSlope = false,IsMovingPlatfrom = false, IsWall = false,IsHang = false;
    Animator Anim_Control;
    [SerializeField] PlayerState STATE = PlayerState.IDLE;
    [SerializeField] GunController Gun;
    string _current_Ground = "";
    public string current_ground => _current_Ground;
    [SerializeField] float fallMultipler = 1.5f;
    Vector2 Movement, Perpend;
    float Inputx;
    Collider2D Colli;

    // Start is called before the first frame update
    void Start()
    {
        Rigi = this.GetComponent<Rigidbody2D>();
        Anim_Control = this.GetComponent<Animator>();
        Colli = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        DefineState();
        Control_MOve();
        BetterJUmp();
        DetectPlatform();
        IsWall = CheckWall();
        ReduceGravity();
        CheckHang();
        if (Input.GetMouseButton(0))
        {
            float dir = this.transform.localScale.x > 0 ? 1 : -1;
            Gun.Fire(dir);
        }
    }

    void Control_MOve()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            //Di chuyen phai
            if(IsHang)
                STATE = PlayerState.CLIMB;
            Transform parent = this.transform.parent;
            this.transform.SetParent(null);
            this.transform.localScale = new Vector3(1, 1, 0);
            this.transform.SetParent(parent);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            //Di chuyen trai
            if (IsHang)
                STATE = PlayerState.CLIMB;
            Transform parent = this.transform.parent;
            this.transform.SetParent(null);
            this.transform.localScale = new Vector3(-1, 1, 0);
            this.transform.SetParent(parent);
        }
        Inputx = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (IsGround)
            {
                //Jump lan 1
                Rigi.velocity = new Vector2(Rigi.velocity.x, Speed.y);
                IsGround = false;
                return;
            }

            if (!IsDoubleJump)
            {
                Rigi.velocity = new Vector2(Rigi.velocity.x, Speed.y * 1.2f);
                IsDoubleJump = true;
            } 
        }
    }

    void ReduceGravity()
    {
        if (IsHang)
        {
            Rigi.gravityScale = 0;
            return;
        }
            
        Rigi.gravityScale = 1;
        if (!IsWall)
            return;
        if (IsGround)
            return;

        Rigi.velocity = new Vector2(Rigi.velocity.x,0);
        Rigi.gravityScale = 0;
    }

    void DetectPlatform()
    {
        RaycastHit2D[] hits = new RaycastHit2D[10];
        Colli.Cast(Vector2.down, hits,0.5f);

        CheckSlope(hits);
        CheckPlatform(hits);
    }

    void CheckHang()
    {
           if(IsHang)
            return;
        IsHang = false;
        Vector2 pos;
        float inputX = 1;
        if (Inputx != 0)
            inputX = Inputx;
        pos.x = this.transform.position.x + inputX * this.CheckWallOffset.x;
        pos.y = this.transform.position.y +  this.CheckWallOffset.y;

#if UNITY_EDITOR
        Debug.DrawRay(pos, Vector2.right * inputX, Color.green);
        #endif
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.right * inputX, 1);
        if (hit.collider != null)
            return;
        if (!IsWall)
            return;
        if (IsGround)
            return;
        IsHang = true;
        Movement.Set(Rigi.velocity.x, 0);
        Rigi.velocity = new Vector2(Rigi.velocity.x,0);
       
    }

    bool CheckWall()
    {
        #if UNITY_EDITOR
        Debug.DrawRay((Vector2)this.transform.position + Inputx * this.CheckWallOffset * Vector2.right, Vector2.right * Inputx , Color.green);
        #endif
        RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position + Inputx *this.CheckWallOffset * Vector2.right, Vector2.right * Inputx, 1);
        if (hit.collider == null)
            return false;
        if (hit.normal != Vector2.left && hit.normal != Vector2.right)
            return false;
        return true;
    }

    void CheckPlatform(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D h in hits)
        {
            if (h.collider == null)
                continue;
            if (h.collider.tag != "MovingPlatform")
                continue;
            IsMovingPlatfrom = true;
            this.transform.SetParent(h.transform);
            return;
        }
        this.transform.SetParent(null);
        IsMovingPlatfrom = false;
    }

    void CheckSlope(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D h in hits)
        {
            if (h.collider == null)
                continue;
            if (h.normal == Vector2.up)
                continue;
            IsSlope = true;
            Perpend = Vector2.Perpendicular(h.normal).normalized;
            Debug.DrawRay(h.point, h.normal, Color.red);
            return;  
        }
        IsSlope = false;
    }

    void BetterJUmp()
    {
      
        if (IsGround)
        {
            if (!IsSlope)
            {
                Movement.Set(Inputx * Speed.x, Rigi.velocity.y);
            }
            else if (IsSlope)
            {
                Movement.Set(-Inputx * Speed.x * Perpend.x, -Inputx * Speed.y * Perpend.y);
            }
        }else if (!IsGround)
        {
            if(!IsWall && !IsHang)
                Movement.Set(Inputx * Speed.x, Rigi.velocity.y);
        }
        
        Rigi.velocity = Movement;
        if (IsWall || IsHang)
        {
            return;
        }
        if (Rigi.velocity.y < 0)
        {
            Rigi.velocity += Vector2.up * Physics2D.gravity.y * fallMultipler * Time.deltaTime;
        }
    }

    void DefineState()
    {
        if (STATE == PlayerState.CLIMB)
        {
            Anim_Control.SetBool(STATE.ToString(), true);
            return;
        }
           
        //State
        if (IsGround)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                STATE = PlayerState.RUN;
            }
            else
            {
                STATE = PlayerState.IDLE;
            }
        }
        else 
        {
            if(IsHang)
                STATE = PlayerState.HANG;
            else
                STATE = PlayerState.JUMP;
        }
     
        //

        //
        for (int i = 0; i<= 6; i++)
        {
            string anim = ((PlayerState)i).ToString(); 
            if((int)STATE == i)
                Anim_Control.SetBool(anim, true);
            else
                Anim_Control.SetBool(anim,false);
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MovingPlatform")
        {
            IsGround = true;
            IsDoubleJump = false;
            _current_Ground = collision.collider.name;
        }
    }

    public void ClimbDone()
    {
        
        this.transform.position += (Vector3)OffsetClimb;
        Rigi.velocity = Vector2.zero;
        Rigi.gravityScale = 1;
        IsHang = false;
        IsGround = true;
        STATE = PlayerState.IDLE;
    }

    public enum PlayerState
    {
        IDLE,
        RUN,
        JUMP,
        FALL,
        D_JUMP,
        HANG,
        CLIMB,
    }
}
