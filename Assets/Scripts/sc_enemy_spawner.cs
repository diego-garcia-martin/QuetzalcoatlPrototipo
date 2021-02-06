using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_enemy_spawner : MonoBehaviour
{
    //Variables para el timer de spawn de enemigos
    public float spawnTimer;
    private float elapsedTime;
    public GameObject enemy1;
    public GameObject enemy2;
    private List<GameObject> l_enemies;
    private const float MIN_POS_IN_Y = -6;
    private const float MAX_POS_IN_Y = 6;
    private const float MIN_POS_IN_X = -8;
    private const float MAX_POS_IN_X = 8;
    public int MAX_ENEMY_COUNT;
    // Start is called before the first frame update
    void Start()
    {
        l_enemies = new List<GameObject>();
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > spawnTimer && l_enemies.Count < MAX_ENEMY_COUNT)
        {
            elapsedTime = 0;
            l_enemies.Add(Instantiate(enemy1, new Vector3(Random.Range(MIN_POS_IN_X, MAX_POS_IN_X), MAX_POS_IN_Y, 0), Quaternion.identity));
            if(Random.Range(0,10) < 3)
            {
                l_enemies.Add(Instantiate(enemy2, new Vector3(Random.Range(MIN_POS_IN_X, MAX_POS_IN_X), MAX_POS_IN_Y, 0), Quaternion.identity));
            }
        }

        for(int index = 0; index < l_enemies.Count; index ++)
        {
            if (l_enemies[index].transform.position.y < MIN_POS_IN_Y || l_enemies[index].transform.position.y > MAX_POS_IN_Y)
            {
                GameObject.Destroy(l_enemies[index]);
                l_enemies.RemoveAt(index);
            }
        }
    }
}
