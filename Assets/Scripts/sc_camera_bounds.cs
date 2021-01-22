using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_camera_bounds : MonoBehaviour
{
    private Camera cam;
    private Vector3 limitsMin;
    private Vector3 limitsMax;
    private Transform tr;
    private Vector2 spriteSize;
    public bool limitYmovement;
        // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        cam = Camera.main;
        limitsMin = cam.ScreenToWorldPoint(new Vector3(0, 0, tr.position.z));
        limitsMax = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, tr.position.z));
        spriteSize.x = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        spriteSize.y = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = tr.position;
        viewPos.x = Mathf.Clamp(viewPos.x, limitsMin.x + spriteSize.x, limitsMax.x - spriteSize.x);
        if (limitYmovement) viewPos.y = Mathf.Clamp(viewPos.y, limitsMin.y, limitsMax.y);
        tr.position = viewPos;
    }
}
