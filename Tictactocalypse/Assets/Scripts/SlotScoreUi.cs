using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SlotScoreUi : MonoBehaviour
{
    [SerializeField] private RawImage scoreBubble;
    [SerializeField] private TextMeshProUGUI scoreBubbleText;

    private int score = 0;

    public void ScorePoint(int playerIndexWinner)
    {
        if (score == 0)
        {
            scoreBubble.color = GameManager.gm.playersData[playerIndexWinner].playerColor;
            GetComponent<Animator>().SetTrigger("ShowScoreBubble");
        }
        score++;
        scoreBubbleText.text = "+" + score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }

}
