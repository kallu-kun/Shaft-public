using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreInput : MonoBehaviour
{
    [SerializeField]
    private ScoreList scoreList;
    [SerializeField]
    private HighScoreUIManager highScoreUIManager;

    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private ScoreCalculator scoreCal;

    public void InputScore()
    {
        int score = scoreCal.score;
        string name = nameInput.text;

        nameInput.text = "";

        scoreList.AddScore(name, score);
        highScoreUIManager.SetScores();
    }
}