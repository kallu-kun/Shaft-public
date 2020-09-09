using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ScoreCalculator : ScriptableObject
{
    public float startX;
    public float currentX;
    public int tracks;
    public int score;

    public void CalculateScore()
    {
        score = Mathf.RoundToInt(currentX - startX) + (tracks - 6);
    }
}
