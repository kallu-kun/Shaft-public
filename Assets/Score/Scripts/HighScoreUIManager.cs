using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreUIManager : MonoBehaviour
{
    [SerializeField]
    private ScoreList scores;

    [SerializeField]
    private GameObject scoreUIEntries;
    private RectTransform viewPort;
    private RectTransform scoreUITransform;

    [SerializeField]
    private GameObject scoreUIEntryPrefab;

    private void Start()
    {
        scoreUITransform = scoreUIEntries.GetComponent<RectTransform>();
        viewPort = scoreUIEntries.transform.parent.GetComponent<RectTransform>();

        SetScores();
    }

    public void SetScores()
    {
        if (scores.scoreList != null)
        {
            ClearScores();

            for (int i = 0; i < scores.scoreList.Count; i++)
            {
                Score score = scores.scoreList[i];

                GameObject nScoreUIEntry = Instantiate(scoreUIEntryPrefab, scoreUIEntries.transform);
                ScoreEntry scoreEntry = nScoreUIEntry.GetComponent<ScoreEntry>();
                scoreEntry.SetScore(i + 1, score.name, score.score);
            }

            scoreUITransform.sizeDelta = new Vector2(viewPort.rect.width, 50 + 150 * scores.scoreList.Count);
        }
    }

    public void ClearScores()
    {
        foreach (Transform child in scoreUIEntries.transform)
        {
            Destroy(child.gameObject);
        }

        scoreUITransform.sizeDelta = new Vector2(viewPort.rect.width, 50);
    }
}
