using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_nubes_spawner : MonoBehaviour
{
    private const float MIN_LENGTH = -12f;
    public GameObject Nube;
    private List<GameObject> l_nubes;
    public float cloud_timer;
    private float ct;
    void Start()
    {
        ct = cloud_timer;
        l_nubes = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ct -= Time.deltaTime;
        if (ct <= 0)
        {
            ct = cloud_timer;
            l_nubes.Add(Instantiate(Nube, new Vector3(12f, Random.Range(-2, 6), 0), Quaternion.identity));
        }
        for(int i = 0; i < l_nubes.Count; i++)
        {
            if (l_nubes[i].transform.position.x < MIN_LENGTH)
            {
                GameObject.Destroy(l_nubes[i]);
                l_nubes.RemoveAt(i);
            }
        }
    }
}
