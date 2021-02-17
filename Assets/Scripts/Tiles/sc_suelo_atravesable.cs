using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_suelo_atravesable : MonoBehaviour
{

    private Transform target;
    private Transform transformObject;
    private BoxCollider2D box_Collider;
    // Start is called before the first frame update
    void Start()
    {
        transformObject = GetComponent<Transform>();
        box_Collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Player").transform;
        if (target.position.y - 0.8f < transformObject.position.y)
        {
            box_Collider.enabled = false;
        }
        else 
        {
            box_Collider.enabled = true;
        }
    }
}
