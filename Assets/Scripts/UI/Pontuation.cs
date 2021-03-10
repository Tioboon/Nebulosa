using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pontuation : MonoBehaviour
{
    private TextMeshProUGUI text;
    public int highScore;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        highScore = PlayerPrefs.GetInt("HS");
        if (name == "HighScore")
        {
            text.text = highScore.ToString();
        }
    }

    public void UpdateScore(int score)
    {
        text.text = score.ToString();
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HS", score);
        }
    }
}
