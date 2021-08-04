using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_background_scroll : MonoBehaviour
{
    private bool transition_begin;
    private bool transition_ongoing;
    private int level;
    public float scrollSpeed = 1;
    public int levelChange;
    private List<GameObject> bgs;
    public GameObject bg1;
    public GameObject bg2;


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
        bgs.Add(bg1);
        bgs.Add(bg2);
        bg1.GetComponent<SpriteRenderer>().sprite = transition1;
        bg2.GetComponent<SpriteRenderer>().sprite = stage1_2;
        bg1.transform.position = new Vector3(0, 0, 0);
        bg2.transform.position = new Vector3(0, 12, 0);
        transition_begin = false;
        transition_ongoing = false;
        SetLevel(1);
    }

    public void SetLevel(int new_level)
    {
        level = new_level;
        transition_begin = true;
        transition_ongoing = true;
        if (level == 4)
        {
            GameObject clouds = GameObject.Find("SpawnerNubes");
            clouds.GetComponent<sc_nubes_spawner>().GenerateClouds = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int scoreNum = (int)GameManager.score;
        if (scoreNum == levelChange) SetLevel(2);
        else if (scoreNum == levelChange*2) SetLevel(3);
        else if (scoreNum == levelChange*3) SetLevel(4);
        foreach (GameObject obj in bgs)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - (scrollSpeed * Time.deltaTime), 0);
            if (obj.transform.position.y <= -12)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, 12, 0);
                if (transition_begin && transition_ongoing)
                {
                    transition_begin = false;
                    switch(level){
                        case 2:
                            obj.GetComponent<SpriteRenderer>().sprite = transition2;
                            break;
                        case 3:
                            obj.GetComponent<SpriteRenderer>().sprite = transition3;
                            break;
                        case 4:
                            obj.GetComponent<SpriteRenderer>().sprite = transition4;
                            break;
                        default:
                            break;
                    }
                }

                else if (!transition_begin && transition_ongoing)
                {
                    transition_ongoing = false;
                    switch(level){
                        case 2:
                            obj.GetComponent<SpriteRenderer>().sprite = stage2_1;
                            break;
                        case 3:
                            obj.GetComponent<SpriteRenderer>().sprite = stage3_1;
                            break;
                        case 4:
                            obj.GetComponent<SpriteRenderer>().sprite = stage4_1;
                            break;
                        default:
                            break;
                    }
                }

                else if (!transition_begin && !transition_ongoing)
                {
                    transition_begin = true;
                    switch(level){
                        case 1:
                            obj.GetComponent<SpriteRenderer>().sprite = stage1_2;
                            break;
                        case 2:
                            obj.GetComponent<SpriteRenderer>().sprite = stage2_2;
                            break;
                        case 3:
                            obj.GetComponent<SpriteRenderer>().sprite = stage3_2;
                            break;
                        case 4:
                            obj.GetComponent<SpriteRenderer>().sprite = stage4_2;
                            break;
                        default:
                            break;
                    }
                }
                //Debug.Log("Level: " + level + " Transition_begin: " + transition_begin + " Transition_ongoing: " + transition_ongoing);

            }
        }  
    }
}
