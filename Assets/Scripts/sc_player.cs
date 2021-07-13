using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_player : MonoBehaviour
{
    //La vida del personaje
    public int health;
    private const string IDLE = "Anim_Quetzalcoatl_idle";
    private const string RUN = "Anim_Quetzalcoatl_run";
    private const string JUMP = "Anim_Quetzalcoatl_jump";

    private const float TOUCHMOVEADJUST = 2.5f;
    private const float OUTOFBOUNDS = -5f;

    private Rigidbody2D r2d;
    private Transform tr;
    private Animator animator;
    private string currentAnim;
    private int jumping;
    private bool grounded;
    private bool touchEnable;
    public float jumpForce;
    public float moveSpeed;
    public int maxJumps;
    public bool debugMode;
    private float xspeed;
    private bool sliding;
    private float pushTimer = 0.3f;
    private bool pushState = false;
    private string pushDirection = "left";

    void Start()
    {
        Input.multiTouchEnabled = false;
        touchEnable = true;
        currentAnim = "";
        r2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        jumping = 0;
        changeAnimation(IDLE);
        xspeed = 0;
        sliding = false;
        health = 4;
    }

    // Update is called once per frame
    void Update()
    {
        //Esto es para pruebas, si se sale de la pantalla el mono que vuelva a entrar y mostrar las velocidades y valores de interes
        if(debugMode && tr.position.y < OUTOFBOUNDS){
            //debugInfo();
            tr.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            r2d.velocity = new Vector2(0, 0);
        }
        else if (tr.position.y < OUTOFBOUNDS)
        {
            health = 0;
        }

        updateMovement();
        updateAnimations();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float dify = tr.position.y - collision.transform.position.y;
        if((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ice") && dify > 0)
        {
            jumping = 0;
            grounded = true;
        }
        if(collision.gameObject.tag == "Ice")
        {
            xspeed = r2d.velocity.x;
            sliding = true;
        }
        else
        {
            sliding = false;
        }

        //checamos las colisiones con los enemigos
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "HitBox")
        {
            health -= 1;
            pushState = true;
            if (collision.gameObject.transform.position.x < tr.position.x) pushDirection = "right";
            else if (collision.gameObject.transform.position.x >= tr.position.x) pushDirection = "left";
            sc_audioManager.PlaySound("hit");
        }
    }

    private void updateAnimations()
    {
        if(grounded || r2d.velocity.y <= 0)
        {
            if(r2d.velocity.x != 0)
            {
                changeAnimation(RUN);
                if(r2d.velocity.x < 0 && !pushState) tr.localScale = new Vector3(1, 1, 1);
                else if (!pushState) tr.localScale = new Vector3(-1, 1, 1);
            }
            else changeAnimation(IDLE);
        }
    }

    private void updateMovement()
    {
        float dirx = 0;

        if(Input.touchCount <= 0)
        {
            touchEnable = true;
        }

        if (pushState)
        {
            if (pushTimer > 0)
            {
                pushTimer -= Time.deltaTime;
                if (pushDirection == "left")
                {
                    r2d.velocity = new Vector2(-moveSpeed * 1.5f, r2d.velocity.y);
                }
                else
                {
                    r2d.velocity = new Vector2(moveSpeed * 1.5f, r2d.velocity.y);
                }
            }
            else
            {
                xspeed = (sliding || !grounded) ? xspeed + (dirx * Time.deltaTime) : 0;
                r2d.velocity = new Vector2(xspeed, r2d.velocity.y);
                pushTimer = 0.3f;
                pushState = false;
            }
        }
        else
        {
            if((Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && touchEnable)) && jumping < maxJumps)
            {
                r2d.velocity = new Vector2( r2d.velocity.x, jumpForce);
                jumping++;
                changeAnimation(JUMP);
                grounded = false;
                touchEnable = false;
                sliding = false;
                sc_audioManager.PlaySound("jump");
            }

            dirx = Input.acceleration.x * moveSpeed * TOUCHMOVEADJUST;

            if(Input.GetKey(KeyCode.LeftArrow)) dirx = -moveSpeed;
            if(Input.GetKey(KeyCode.RightArrow)) dirx = moveSpeed;

            xspeed = (sliding || !grounded) ? xspeed + (dirx * Time.deltaTime) : dirx;

            r2d.velocity = new Vector2(xspeed, r2d.velocity.y);
        }
    }

    private void changeAnimation(string newAnim)
    {
        if(currentAnim == JUMP && newAnim == JUMP)
        {
            animator.Play(newAnim, -1, 0f);
        }
        if(currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }

    private void debugInfo()
    {
        print("Debug Mode On");
    }

    public void setJumps(int j)
    {
        jumping = j;
    }

    public int getJumps(int j)
    {
        return jumping;
    }

    public int getmaxJumps()
    {
        return maxJumps;
    }

    public void setmaxJumps(int j)
    {
        maxJumps = j;
    }
}
