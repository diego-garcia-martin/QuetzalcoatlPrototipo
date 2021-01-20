using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_player : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D r2d;
    private Transform tr;
    private int jumping;
    private string currentAnim;
    private Animator animator;
    public float jumpForce;
    public float moveSpeed;
    public int maxJumps;
    public bool debugMode;

    private const string IDLE = "Anim_Quetzalcoatl_idle";
    private const string RUN = "Anim_Quetzalcoatl_run";
    private const string JUMP = "Anim_Quetzalcoatl_jump";

    void Start()
    {
        currentAnim = "";
        r2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        jumping = 0;
        changeAnimation(IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        //Esto es para pruebas, si se sale de la pantalla el mono que vuelva a entrar y mostrar las velocidades y valores de interes
        if(debugMode){
            debugInfo();
            if(tr.position.y < -5)
            {
                tr.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                r2d.velocity = new Vector2(0, 0);
            }
        }

        updateMovement();
        updateAnimations();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            jumping = 0;
        }
    }

    private void updateAnimations()
    {
        if(r2d.velocity.x != 0)
        {
            changeAnimation(RUN);
            if(r2d.velocity.x < 0) tr.localScale = new Vector3(1, 1, 1);
            else tr.localScale = new Vector3(-1, 1, 1);
        }
        else changeAnimation(IDLE);
    }

    private void updateMovement()
    {
        float dirx = 0;
        
        if((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && jumping < maxJumps)
        {
            r2d.velocity = new Vector2( r2d.velocity.x, jumpForce);
            jumping++;
            changeAnimation(JUMP);
        }

        dirx = Input.acceleration.x * moveSpeed;

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            dirx = -moveSpeed;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            dirx = moveSpeed;
        }

        r2d.velocity = new Vector2(dirx, r2d.velocity.y);
    }

    private void changeAnimation(string newAnim)
    {
        if(currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }

    private void debugInfo()
    {
        print("Debug Mode On");
    }
}
