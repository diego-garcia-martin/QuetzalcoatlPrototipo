using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_player : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D r2d;
    private Transform tr;
    public float jumpForce;
    public float moveSpeed;
    private bool jumping;
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirx = 0;
        if(Input.touchCount == 0)
        {
            jumping = false;
        }
        if(Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && jumping == false))
        {
            r2d.AddForce(new Vector2(0, jumpForce));
            jumping = true;
        }

        dirx = Input.acceleration.x * moveSpeed;

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            dirx = -moveSpeed;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            dirx = moveSpeed;
        }

        r2d.velocity = new Vector2(dirx, r2d.velocity.y);

        if(tr.position.y < -5) tr.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
    }
}
