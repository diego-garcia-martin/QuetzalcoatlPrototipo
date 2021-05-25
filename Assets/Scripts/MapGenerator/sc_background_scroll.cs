using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_background_scroll : MonoBehaviour
{
    public GameObject bg1;
    public GameObject bg2;
    public GameObject bg3;
    public float scrollSpeed = 1;
    private List<GameObject> bgs;
    // Start is called before the first frame update
    void Start()
    {
        bgs = new List<GameObject>();
        bgs.Add(bg1);
        bgs.Add(bg2);
        bgs.Add(bg3);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in bgs)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - (scrollSpeed * Time.deltaTime), 0);
            if (obj.transform.position.y <= -12)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, 12, 0);
            }
        }
    }
}
