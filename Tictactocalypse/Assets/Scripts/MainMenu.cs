using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button[] nbPlayersButtons;
    [SerializeField] private Button[] scoresGoalButtons;

    [SerializeField] private Toggle eventRestrictions;

    public int nbOfPlayers;
    public int scoreToWin;

    private void Start()
    {
        nbOfPlayers = 2;
        UpdatePlayersButton();

        scoreToWin = 10;
        UpdateScoreButtons();

        eventRestrictions.isOn = true;
    }

    public void Play()
    {
        SceneController.sc.nbOfPlayers = nbOfPlayers;
        SceneController.sc.scoreToWin = scoreToWin;

        SceneController.sc.areEventRestricted = eventRestrictions.isOn;

        SceneController.sc.LoadMainGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetNbOfPlayers(int nbOfPlayers)
    {
        this.nbOfPlayers = nbOfPlayers;
        UpdatePlayersButton();
    }

    public void UpdatePlayersButton()
    {
        for (int i = 0; i < nbPlayersButtons.Length; i++)
        {
            if (i == nbOfPlayers - 2)
            {
                nbPlayersButtons[i].interactable = false;
            }
            else
            {
                nbPlayersButtons[i].interactable = true;
            }
        }
    }

    public void SetScoreToWin(int score)
    {
        scoreToWin = score;
        UpdateScoreButtons();
    }

    public void UpdateScoreButtons()
    {
        for (int i = 0; i < scoresGoalButtons.Length; i++)
        {
            if (scoreToWin == i * 10 + 10)
            {
                scoresGoalButtons[i].interactable = false;
            }
            else
            {
                scoresGoalButtons[i].interactable = true;
            }
        }
    }
}
