﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_player : MonoBehaviour
{
    private const string IDLE = "Anim_Quetzalcoatl_idle";
    private const string RUN = "Anim_Quetzalcoatl_run";
    private const string JUMP = "Anim_Quetzalcoatl_jump";

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
    }

    // Update is called once per frame
    void Update()
    {
        //Esto es para pruebas, si se sale de la pantalla el mono que vuelva a entrar y mostrar las velocidades y valores de interes
        if(debugMode){
            //debugInfo();
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
        float dify = tr.position.y - collision.transform.position.y;
        if(collision.gameObject.tag == "Ground" && dify > 0)
        {
            jumping = 0;
            grounded = true;
        }
    }

    private void updateAnimations()
    {
        if(grounded || r2d.velocity.y <= 0)
        {
            if(r2d.velocity.x != 0)
            {
                changeAnimation(RUN);
                if(r2d.velocity.x < 0) tr.localScale = new Vector3(1, 1, 1);
                else tr.localScale = new Vector3(-1, 1, 1);
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
        
        if((Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && touchEnable)) && jumping < maxJumps)
        {
            r2d.velocity = new Vector2( r2d.velocity.x, jumpForce);
            jumping++;
            changeAnimation(JUMP);
            grounded = false;
            touchEnable = false;
        }

        dirx = Input.acceleration.x * moveSpeed * 2.5f;

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
}
