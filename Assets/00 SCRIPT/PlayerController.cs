using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 Speed;
    Rigidbody2D Rigi;
    [SerializeField] bool IsGround = true, IsDoubleJump = false, IsSlope = false,IsMovingPlatfrom = false;
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
            Transform parent = this.transform.parent;
            this.transform.SetParent(null);
            this.transform.localScale = new Vector3(1, 1, 0);
            this.transform.SetParent(parent);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            //Di chuyen trai
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
            Debug.LogError(Rigi.velocity.y);
        }
    }

    void DetectPlatform()
    {
        RaycastHit2D[] hits = new RaycastHit2D[10];
        Colli.Cast(Vector2.down, hits,0.5f);

        CheckSlope(hits);
        CheckPlatform(hits);
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
            Movement.Set(Inputx * Speed.x, Rigi.velocity.y);
        }
        
        Rigi.velocity = Movement;
        if (Rigi.velocity.y < 0)
        {
            Rigi.velocity += Vector2.up * Physics2D.gravity.y * fallMultipler * Time.deltaTime;
        }
    }

    void DefineState()
    {
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
            STATE = PlayerState.JUMP;
        }
     
        //

        //
        for (int i = 0; i<= 5; i++)
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

    public enum PlayerState
    {
        IDLE,
        RUN,
        JUMP,
        FALL,
        D_JUMP
    }
}
