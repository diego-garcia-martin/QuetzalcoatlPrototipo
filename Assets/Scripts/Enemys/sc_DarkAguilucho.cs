using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_DarkAguilucho : MonoBehaviour
{
    //Variables de componentes que tienen que ver con el movimiento
    private Rigidbody2D r2d;
    private Transform tr;
    private Transform target;
    public float flightSpeed;
    public float attackSpeed;
    public float lifetime;
    private float cooldown;
    //Variables relacionadas con las animaciones del personaje
    private Animator animator;
    private string currentAnim;
    private const string IDLE = "Anim_DarkAguilucho_idle";
    private const string ATTACK = "Anim_DarkAguilucho_attack";
    // Start is called before the first frame update
    void Start()
    {
        currentAnim = "";
        animator = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        changeAnimation(IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            normalBehavior();
        }
        else
        {
            tr.position = Vector3.MoveTowards(tr.position, new Vector3(0, -20, 0), (flightSpeed * Time.deltaTime));
        }
        lifetime = lifetime - Time.deltaTime;
    }

    private void normalBehavior()
    {
        target = GameObject.FindWithTag("Player").transform;
        if (target.position.x < tr.position.x) tr.localScale = new Vector3(-1, 1, 1);
        else if (target.position.x > tr.position.x) tr.localScale = new Vector3(1, 1, 1);


        if (cooldown > 0)
        {
            cooldown = cooldown - Time.deltaTime;
        }

        //Decidimos si vamos a atacar
        if (Vector3.Distance(transform.position, target.position) < 3f && tr.position.y > target.position.y && cooldown <= 0)
        {
            if (Random.Range(0, 10) < 1)
            {
                changeAnimation(ATTACK);
                cooldown = 3f;
            }
        }
        if (currentAnim == IDLE)
        {
            idleFollow();
        }
        else if (currentAnim == ATTACK)
        {
            attack();
        }
        //Debug.Log("Target: " + target.position);
    }

    private void idleFollow()
    {
        changeAnimation(IDLE);
        //Para seguir al player en el eje de las y

        if (tr.position.y > target.position.y + 4f)
        {
            r2d.velocity = new Vector3(r2d.velocity.x, -2, 0);
        }
        else if (tr.position.y < target.position.y + 2f)
        {
            r2d.velocity = new Vector3(r2d.velocity.x, 3, 0);
        }
        // Para seguir al player en el eje de las x
        if (target.position.x > tr.position.x + 2)
        {
            r2d.velocity = new Vector3(flightSpeed, r2d.velocity.y, 0);
        }
        else if (target.position.x < tr.position.x - 2)
        {
            r2d.velocity = new Vector3(-flightSpeed, r2d.velocity.y, 0);
        }
        else
        {
            r2d.velocity = new Vector3(0, r2d.velocity.y, 0);
        }

    }

    private void attack()
    {
        changeAnimation(ATTACK);
        tr.position = Vector3.MoveTowards(tr.position, target.position, (attackSpeed * Time.deltaTime));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        changeAnimation(IDLE);
        if (collision.gameObject.tag == "Player")
        {
            lifetime = 0;
        }
    }

    private void changeAnimation(string newAnim)
    {
        if (currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}
