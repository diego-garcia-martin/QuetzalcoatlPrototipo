using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Puntuaciones
//Pantallas

public class GameManager : MonoBehaviour
{
    public static float score;
    public enum _gameStates{
        START,
        PLAY,
        GAMEOVER
    };
    public _gameStates GameState;

    // Start is called before the first frame update
    void Start()
    {
        GameState = _gameStates.PLAY;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    { 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<sc_player>().health <= 0)
        {
            GameState = _gameStates.GAMEOVER;
            StateChange();
        }
        
    }

    void FixedUpdate()
    {
        score += Time.deltaTime;
    }

    void StateChange()
    {
        switch(GameState)
        {
            case _gameStates.GAMEOVER:
                PlayerPrefs.SetInt("score", (int)score);
                SceneManager.LoadScene("GameOver");
                break;
            case _gameStates.START:
                SceneManager.LoadScene("Init");
                break;
        }
    }
}
