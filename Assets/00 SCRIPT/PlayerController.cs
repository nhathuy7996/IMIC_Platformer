using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 Speed;
    Rigidbody2D Rigi;
    [SerializeField] bool IsGround = true, IsDoubleJump = false;
    Animator Anim_Control;
    [SerializeField] PlayerState STATE = PlayerState.IDLE;
    [SerializeField] GunController Gun;
    string _current_Ground = "";
    public string current_ground => _current_Ground;

    // Start is called before the first frame update
    void Start()
    {
        Rigi = this.GetComponent<Rigidbody2D>();
        Anim_Control = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DefineState();
        Control_MOve();
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
            this.transform.position += new Vector3(Speed.x * Time.deltaTime, 0, 0);
            this.transform.localScale = new Vector3(1, 1, 0);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            //Di chuyen trai
            this.transform.position += new Vector3(-Speed.x * Time.deltaTime, 0, 0);
            this.transform.localScale = new Vector3(-1, 1, 0);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGround)
            {
                //Jump lan 1
                Rigi.AddForce(new Vector2(0, Speed.y));
                IsGround = false;
                return;
            }

            if (!IsDoubleJump)
            {
                Rigi.AddForce(new Vector2(0, Speed.y));
                IsDoubleJump = true;
            }

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
        if (collision.gameObject.tag == "Ground")
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
