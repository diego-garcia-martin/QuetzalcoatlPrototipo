using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_trinchos : MonoBehaviour
{
    private const string IDLE = "Anim_trinchos_idle";
    private const string ATTACK = "Anim_trinchos_attack";
    private Animator animator;
    private string currentAnim;
    private bool attacking;
    public float attackTimer;
    public float playerRepel;
    private float timer;
    private BoxCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        currentAnim = "";
        animator = GetComponent<Animator>();
        attacking = false;
        timer = attackTimer;
        col = GetComponent<BoxCollider2D>();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D r2d = collision.gameObject.GetComponent<Rigidbody2D>();
            sc_player pl = collision.gameObject.GetComponent<sc_player>();
            r2d.velocity = new Vector3(r2d.velocity.x, playerRepel, 0);
            pl.setJumps(pl.getmaxJumps());
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; 
        if (timer <= 0 && !attacking)
        {
            timer = 1;
            attacking = true;
            changeAnimation(ATTACK);
        }
        else if (timer <= 0 && attacking)
        {
            timer = attackTimer;
            attacking = false;
            changeAnimation(IDLE);
        }

        if (attacking) col.enabled = true;
        else col.enabled = false;
    }

    private void changeAnimation(string newAnim)
    {
        if(currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}
