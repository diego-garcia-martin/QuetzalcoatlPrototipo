using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_background_scroll : MonoBehaviour
{
    public GameObject transitionObj;
    public GameObject bg2;
    public GameObject bg3;
    public bool transition = false;
    public float scrollSpeed = 1;
    private List<GameObject> bgs;
    // Start is called before the first frame update
    void Start()
    {
        bgs = new List<GameObject>();
        bgs.Add(bg2);
        bgs.Add(bg3);
        transition = false;
        transitionObj.transform.position = new Vector3(0, 0, 0);
        bg2.transform.position = new Vector3(0, 12, 0);
        bg3.transform.position = new Vector3(0, 24, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transition)
        {
            transitionObj.transform.position = new Vector3(0, 0, 0);
            bg2.transform.position = new Vector3(0, 12, 0);
            bg3.transform.position = new Vector3(0, 24, 0);
        }
        if (transitionObj.transform.position.y > -12)
        {
            transitionObj.transform.position = new Vector3(0, transitionObj.transform.position.y - (scrollSpeed * Time.deltaTime), 0);
        }
        else
        {
            transition = false;
        }

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
