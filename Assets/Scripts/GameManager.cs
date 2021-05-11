using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Puntuaciones
//Pantallas

public class GameManager : MonoBehaviour
{
    public enum _gameStates{
        START,
        PLAY,
        GAMEOVER
    };
    public _gameStates GameState;

    // Start is called before the first frame update
    void Start()
    {
        GameState = _gameStates.START;
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

    void StateChange()
    {
        switch(GameState)
        {
            case _gameStates.GAMEOVER:
                break;
            case _gameStates.START:
                break;
            case _gameStates.PLAY:
                break;
        }
    }
}
