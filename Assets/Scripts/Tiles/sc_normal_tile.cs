using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_normal_tile : MonoBehaviour
{
    public Sprite sp1, sp2, sp3, sp4;
    private List<Sprite> l_sprites;
    private SpriteRenderer sr;

    private Transform target;
    private Transform transformObject;
    private BoxCollider2D box_Collider;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 3);
        sr = GetComponent<SpriteRenderer>();
        l_sprites = new List<Sprite>();
        l_sprites.Add(sp1);
        l_sprites.Add(sp2);
        l_sprites.Add(sp3);
        l_sprites.Add(sp4);

        sr.sprite = l_sprites[rand];

        transformObject = GetComponent<Transform>();
        box_Collider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        target = GameObject.FindWithTag("Player").transform;
        if (target.position.y < transformObject.position.y - 1)
        {
            box_Collider.enabled = false;
        }
        else 
        {
            box_Collider.enabled = true;
        }
    }

}
