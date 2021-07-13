using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_textScore : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreText;
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int scoreNum = (int)GameManager.score;
        string score = scoreNum.ToString();
        scoreText.text = score;
    }
}
