using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI placement;

    [SerializeField]
    private TextMeshProUGUI scoreName;

    [SerializeField]
    private TextMeshProUGUI score;

    public void SetScore(int placement, string name, int score)
    {
        this.placement.text = placement + ".";
        scoreName.text = name;
        this.score.text = score + "";
    }
}
