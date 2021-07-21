using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_background_scroll : MonoBehaviour
{
    private bool transition;
    public float scrollSpeed = 1;
    private List<GameObject> bgs;
    public GameObject bg2;
    public GameObject bg3;


    // Sprites
    public Sprite transition1;
    public Sprite transition2;
    public Sprite transition3;
    public Sprite transition4;
    public Sprite stage1_1;
    public Sprite stage1_2;
    public Sprite stage2_1;
    public Sprite stage2_2;
    public Sprite stage3_1;
    public Sprite stage3_2;
    public Sprite stage4_1;
    public Sprite stage4_2;

    // Start is called before the first frame update
    void Start()
    {
        bgs = new List<GameObject>();
        bgs.Add(bg2);
        bgs.Add(bg3);
        Set_Objects(1);
    }

    void Set_Objects(int stage)
    {
        transition = true;
        if (stage == 1)
        {
            bg2.GetComponent<SpriteRenderer>().sprite = stage1_1;
            bg3.GetComponent<SpriteRenderer>().sprite = stage1_2;
        }
        if (stage == 2)
        {
            bg2.GetComponent<SpriteRenderer>().sprite = stage2_1;
            bg3.GetComponent<SpriteRenderer>().sprite = stage2_2;
        }
        if (stage == 3)
        {
            bg2.GetComponent<SpriteRenderer>().sprite = stage3_1;
            bg3.GetComponent<SpriteRenderer>().sprite = stage3_2;
        }
        if (stage == 4)
        {
            bg2.GetComponent<SpriteRenderer>().sprite = stage4_1;
            bg3.GetComponent<SpriteRenderer>().sprite = stage4_2;
        }

}

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in bgs)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - (scrollSpeed * Time.deltaTime), 0);
            if (obj.transform.position.y <= -12)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, 24, 0);

            }
        }  
    }
}
