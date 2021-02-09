using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainCanvas : MonoBehaviour
{
    public PlayerUI[] playersUI;

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject victoryTab;
    [SerializeField] private TextMeshProUGUI victoryText;

    [SerializeField] private Image clock; 
    [SerializeField] private TextMeshProUGUI clockText;

    private GameManager gm;

    private bool triggeredVictory = false;

    public void Initialize()
    {
        print("Main Canvas Initialized");

        gm = GameManager.gm;

        StartCoroutine(StartTimer(ConstantManager.gameTimeDuration));

        int i = 0;
        for (; i < gm.nbOfPlayers; i++)
        {
            playersUI[i].gameObject.SetActive(true);
            playersUI[i].Initialize(i);
        }

        for (; i < 4; i++)
        {
            playersUI[i].gameObject.SetActive(false);
        }
    }

    public void TriggerVictory(int playerIndex)
    {
        triggeredVictory = true;

        clock.gameObject.SetActive(false);

        victoryTab.SetActive(true);
        victoryText.text = "Player " + (playerIndex + 1).ToString() + " Wins!!!";

        for (int i = 0; i < gm.nbOfPlayers; i++)
        {
            if (i != playerIndex)
                playersUI[i].gameObject.SetActive(false);
        }
    }

    IEnumerator StartTimer(float timerCount)
    {
        while (!triggeredVictory && timerCount > 0)
        {
            timerText.text = SecondsToString((int)timerCount);
            yield return new WaitForSeconds(0.1f);
            timerCount -= 0.1f;

            float k = (gm.scoreEval - (timerCount % gm.scoreEval)) + 0.1f;

            float k2 = ((int)(k * 10f)) / 10f;

            clockText.text = k2.ToString() + ((int)k2 == k2 ? ".0" : "");

            clock.fillAmount = (float)(k) / (float)gm.scoreEval;

            if (k >= gm.scoreEval)
            {
                gm.EndRound();
            }
        }
    }

    private string SecondsToString(int seconds)
    {
        string second0 = (seconds % 60) < 10 ? "0" : "";
        return (seconds / 60).ToString() + ":" + second0 + (seconds % 60);
    }

    public void UpdateEventIcons(int playerIndex, int buttonIndex)
    {
        playersUI[playerIndex].UpdatePlayerEventsUI(buttonIndex);
    }

    public void UpdateScores()
    {
        for (int i = 0; i < gm.nbOfPlayers; i++)
        {
            playersUI[i].SetScore(Player.players[i].score);
        }
    }

    public void TriggerPlaceMode(int playerIndex, bool enable, int index)
    {
        playersUI[playerIndex].TriggerPlaceMode(enable, index);
    }

}
