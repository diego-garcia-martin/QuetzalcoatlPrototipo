using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_spider : MonoBehaviour
{
    //Animation states
    private const string WALK = "Anim_Spider_walk";
    private const string ATTACK = "Anim_Spider_attack";
    private Animator animator;
    private string currentAnim;

    //Rigid 2D to move around, as well as walk speed
    private Rigidbody2D rb;
    public float walkSpeed = 2;
    private bool walkRight;
    private bool isWalking;
    public float segundosAccion = 3;
    public Transform groundDetection;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAnim = WALK;
        rb = GetComponent<Rigidbody2D>();

        if (Random.value<0.5f) walkRight = true;
        else walkRight = false;

        isWalking = true;

        if (walkRight)
        {
            transform.localScale = new Vector3(-1,  1, 1);
        }
        else
        {
            walkSpeed = -walkSpeed;
        }
        StartCoroutine("waiter");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWalking) walk();
    }

    void walk()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        if(groundInfo.collider == false)
        {
            if (walkRight)
            {
                walkRight = false;
                transform.localScale = new Vector3(1, 1, 1);
                walkSpeed = -walkSpeed;
            }
            else
            {
                walkRight = true;
                transform.localScale = new Vector3(-1, 1, 1);
                walkSpeed = -walkSpeed;
            }
        }
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
    }

    private void changeAnimation(string newAnim)
    {
        if (currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }

    IEnumerator waiter()
    {
        for (; ; )
        {
            float segundos = segundosAccion;
            if (isWalking)
            {
                if (Random.Range(0, 10) < 5)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    changeAnimation(ATTACK);
                    isWalking = false;
                    segundos = 1;
                }
            }
            else{
                isWalking = true;
                changeAnimation(WALK);
            }
            yield return new WaitForSeconds(segundos);
        }
    }
}
