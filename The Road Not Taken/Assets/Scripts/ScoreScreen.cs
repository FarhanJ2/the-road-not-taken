using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text currScore;
    [SerializeField] private TMP_Text highScore;

    private void Awake()
    {
        currScore.text = PlayerStats.Score.ToString();
        // highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        highScore.text = "N/a"; // grab from somewhere later
    }
}
