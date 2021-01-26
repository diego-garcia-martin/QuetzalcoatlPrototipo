using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_pichoncin : MonoBehaviour
{
    //Variables para los otros componentes
    private Animator animator;
    private Rigidbody2D r2d;
    private Transform tr;
    //Variables de control para ver que le pasa al pichoncin
    private bool isGrounded;
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
        currentAnim = "";
        changeAnimation(FLY);
        isGrounded = false;
        StartCoroutine("waiter");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGrounded) changeAnimation(FLY);
        if(r2d.velocity.x != 0)
            {
                if(r2d.velocity.x < 0) tr.localScale = new Vector3(-1, 1, 1);
                else tr.localScale = new Vector3(1, 1, 1);
            }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            r2d.velocity = new Vector2(0, 0);
            changeAnimation(IDLE);
        }
    }

    private void changeAnimation(string newAnim)
    {
        if(currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }

    IEnumerator waiter()
    {
        for(;;)
        {
            if(isGrounded)
            {
                if(Random.Range(0, 10) < 5)
                {
                    changeAnimation(EAT);
                }
                else if(Random.Range(0, 10) < 6)
                {
                    r2d.AddForce(new Vector2(Random.Range(-300, 300), 300));
                    isGrounded = false;
                }
                else
                {
                    changeAnimation(IDLE);
                }
            }
            yield return new WaitForSeconds(2); 
        }
    }
}
