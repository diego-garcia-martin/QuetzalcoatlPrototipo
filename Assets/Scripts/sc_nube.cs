using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_nube : MonoBehaviour
{
    // Start is called before the first frame update
    public float vidaNube;
    private float timer;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    void Start()
    {
        timer = vidaNube;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer - Time.deltaTime;

        float alpha = timer / vidaNube;

        sr.color = new Color(1f, 1f, 1f, alpha);

        if (alpha < 0.3f)
        {
            col.enabled = false;
        } 

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
