﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_GameOverScore : MonoBehaviour
{
    public Text scoreText;
    void Start()
    {
        scoreText = GetComponent<Text>();
        int score = PlayerPrefs.GetInt("score");
        string scoreMessage = "You got " + score.ToString() + " points!";
        scoreText.text = scoreMessage;
    }

}
