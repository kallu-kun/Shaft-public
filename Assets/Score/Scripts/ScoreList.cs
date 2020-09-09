using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreList : MonoBehaviour
{
    public List<Score> scoreList { get; set; }

    private void Awake()
    {
        SortScores();
    }

    public void AddScore(string name, int score)
    {
        scoreList.Add(new Score(name.Trim(), score));
        SortScores();
    }

    public void SortScores()
    {
        if (scoreList != null)
        {
            for (int i = 0; i < scoreList.Count; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    Score checkedScore = scoreList[j];
                    if (scoreList[j - 1].score < checkedScore.score)
                    {
                        scoreList[j] = scoreList[j - 1];
                        scoreList[j - 1] = checkedScore;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void ResetList()
    {
        scoreList = new List<Score>();
    }
}
