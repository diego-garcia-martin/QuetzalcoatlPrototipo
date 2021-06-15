using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_pichoncin : MonoBehaviour
{
    //Variables para los otros componentes
    private Animator animator;
    private Rigidbody2D r2d;
    private Transform tr;
    private Camera cam;
    private Vector3 limitsMin;
    private Vector3 limitsMax;
    //Variables de control para ver que le pasa al pichoncin
    private bool isGrounded;
    private bool isFlying;
    public int segundosAccion;
    //Las variables que tienen que ver con las animaciones
    private string currentAnim;
    private const string IDLE = "Anim_Pichoncin_idle";
    private const string FLY = "Anim_Pichoncin_vuelo";
    private const string EAT = "Anim_Pichoncin_picando";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        cam = Camera.main;
        limitsMin = cam.ScreenToWorldPoint(new Vector3(0, 0, tr.position.z));
        limitsMax = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, tr.position.z));
        currentAnim = "";
        changeAnimation(FLY);
        isGrounded = false;
        StartCoroutine("waiter");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            changeAnimation(FLY);
            if (isFlying) flying();
            if (r2d.velocity.y < -10)
            {
                r2d.AddForce(new Vector2(Random.Range(-300, 300), 300));
                isFlying = true;
                isGrounded = false;
            }
        }
        if (r2d.velocity.x != 0)
        {
            if (r2d.velocity.x < 0) tr.localScale = new Vector3(-1, 1, 1);
            else tr.localScale = new Vector3(1, 1, 1);
        }

        if (tr.position.x >= limitsMax.x - 1)
        {
            r2d.velocity = new Vector3(-2, r2d.velocity.x, 0);
        }
        else if (tr.position.x <= limitsMin.x + 1)
        {
            r2d.velocity = new Vector3(2, r2d.velocity.x, 0);
        }

        if (r2d.velocity.y < -10 && isGrounded)
        {
            r2d.AddForce(new Vector2(Random.Range(-300, 300), 300));
            isFlying = true;
            isGrounded = false;
        }
        else if (r2d.velocity.y < -3)
        {
            isGrounded = false;
        }

    }

    private void flying()
    {
        if (r2d.velocity.y <= 0)
        {
            r2d.velocity = new Vector3(r2d.velocity.x, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Ice")
        {
            isGrounded = true;
            r2d.velocity = new Vector2(0, 0);
            changeAnimation(IDLE);
        }
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
            if (isGrounded && !isFlying)
            {
                if (Random.Range(0, 10) < 5)
                {
                    changeAnimation(EAT);
                }
                else if (Random.Range(0, 10) < 6)
                {
                    r2d.AddForce(new Vector2(Random.Range(-300, 300), 600));
                    isFlying = true;
                    isGrounded = false;
                    sc_audioManager.PlaySound("aleteoPichon");
                }
                else
                {
                    changeAnimation(IDLE);
                }
            }
            else if (isFlying)
            {
                if (Random.Range(0, 10) < 7)
                {
                    r2d.AddForce(new Vector2(Random.Range(-300, 300), 0));
                    sc_audioManager.PlaySound("aleteoPichon");
                }
                else
                {
                    isFlying = false;
                }
            }
            yield return new WaitForSeconds(segundosAccion);
        }
    }
}
