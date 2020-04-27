using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController sc;

    public int nbOfPlayers;
    public int scoreToWin;
    public bool areEventRestricted;

    private void Awake()
    {
        if (sc == null)
            sc = this;
        else if (sc != null)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
