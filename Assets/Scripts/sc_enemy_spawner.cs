﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_enemy_spawner : MonoBehaviour
{
    //Variables para el timer de spawn de enemigos
    public int enemySet;
    public float spawnTimer;
    private float elapsedTime;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public List<GameObject> l_enemies;
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
        enemySet = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > spawnTimer && l_enemies.Count < MAX_ENEMY_COUNT)
        {
            elapsedTime = 0;
            if (enemySet == 1)
            {
                l_enemies.Add(Instantiate(enemy1, new Vector3(Random.Range(MIN_POS_IN_X, MAX_POS_IN_X), MAX_POS_IN_Y, 0), Quaternion.identity));
                if(Random.Range(0,10) < 3)
                {
                    l_enemies.Add(Instantiate(enemy2, new Vector3(Random.Range(MIN_POS_IN_X, MAX_POS_IN_X), MAX_POS_IN_Y, 0), Quaternion.identity));
                }
            }
            else
            {
                l_enemies.Add(Instantiate(enemy3, new Vector3(Random.Range(MIN_POS_IN_X, MAX_POS_IN_X), MAX_POS_IN_Y, 0), Quaternion.identity));
                if(Random.Range(0,10) < 3)
                {
                    l_enemies.Add(Instantiate(enemy4, new Vector3(Random.Range(MIN_POS_IN_X, MAX_POS_IN_X), MAX_POS_IN_Y, 0), Quaternion.identity));
                }
            }
            
        }

        for(int index = l_enemies.Count -1; index >= 0; index --)
        {
            if (l_enemies[index] == null)
            {
                l_enemies.RemoveAt(index);
                continue;
            }
            if (l_enemies[index].transform.position.y < MIN_POS_IN_Y)
            {
                GameObject.Destroy(l_enemies[index]);
                l_enemies.RemoveAt(index);
            }
        }

    }
}
