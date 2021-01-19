using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float tileSpeed;
    public GameObject tile;
    public List<GameObject> l_tile;
    // Start is called before the first frame update
    void Start()
    {
        l_tile.Add(Instantiate(tile, new Vector3(0, 0, 0), Quaternion.identity));

        for(int i = 1; i <= 10; i++)
        {
            l_tile.Add(Instantiate(tile, new Vector3(Random.Range(-8, 8), 3*i, 0), Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < l_tile.Count; i++)
        {
            l_tile[i].transform.position = l_tile[i].transform.position + new Vector3(0, -1f, 0) * Time.deltaTime * tileSpeed;
            if(l_tile[i].transform.position.y < -6)
            {
               l_tile[i].transform.position = new Vector3(Random.Range(-8, 8), 27, 0);
            }
        }
    }
}
