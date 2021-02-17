using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_nube : MonoBehaviour
{
    // Start is called before the first frame update
    public float vidaNube;
    private float timer;
    private BoxCollider2D col;

    
    private const string IDLE = "Anim_nube_idle";
    private const string END = "Anim_nube_end";
    private Animator animator;
    private string currentAnim;
    private bool timing;
    void Start()
    {
        timing = false;
        currentAnim = "";
        timer = vidaNube;
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        changeAnimation(IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (timing) timer = timer - Time.deltaTime;

        if (timer <= 1)
        {
            changeAnimation(END);
            col.enabled = false;
        }

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

        private void changeAnimation(string newAnim)
    {
        if(currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        timing = true;
    }
}
