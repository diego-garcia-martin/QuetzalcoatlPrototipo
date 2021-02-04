using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_aguilucho : MonoBehaviour
{
    //Variables de componentes que tienen que ver con el movimiento
    private Rigidbody2D r2d;
    private Transform tr;
    private Transform target;
    public float flightSpeed;
    public float attackSpeed;
    private BoxCollider2D col;
    //Variables relacionadas con las animaciones del personaje
    private Animator animator;
    private string currentAnim;
    private const string IDLE = "Anim_Aguilucho_idle";
    private const string ATTACK = "Anim_Aguilucho_attack";
    // Start is called before the first frame update
    void Start()
    {
        currentAnim = "";
        animator = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        col = GetComponent<BoxCollider2D>();
        changeAnimation(IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Player").transform;
        if (r2d.velocity.x < 0) tr.localScale = new Vector3(-1, 1, 1);
        else if (r2d.velocity.x > 0) tr.localScale = new Vector3(1, 1, 1);

        if (currentAnim == IDLE) col.enabled = false;
        else col.enabled = true;

        if (target.position.x > tr.position.x + 1)
        {
            r2d.velocity = new Vector3(flightSpeed, r2d.velocity.y, 0);
        }
        else if (target.position.x < tr.position.x - 1)
        {
            r2d.velocity = new Vector3(-flightSpeed, r2d.velocity.y, 0);
        }
        else
        {
            r2d.velocity = new Vector3(0, r2d.velocity.y, 0);
        }
    }

    private void changeAnimation(string newAnim)
    {
        if (currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}
