using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_nube_bg : MonoBehaviour
{
    public float speed;
    private float sp;
    void Start()
    {
        sp = Random.Range(speed/2, speed);
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - sp, transform.position.y - sp/2, 0);
    }
}
