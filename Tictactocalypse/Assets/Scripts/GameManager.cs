using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Creation
    public static GameManager gm;
    private void Awake()
    {
        if (gm == null)
            gm = this;
        else if (gm != this)
            Destroy(this.gameObject);
    }
    #endregion

    #region Public Variables

    public Camera mainCamera;
    public Light mainDirectionalLight;
    public MainCanvas mainCanvas;

    public int nbOfPlayers = 2;
    public int gridSize = 4;

    public int scoreEval = 10;

    public int scoreToWin = 10;

    public PlayGrid grid;


    public PlayerData[] playersData;

    public Texture[] arrowsTextures;

    public Texture emptyTexture;

    #endregion

    #region Private Variables

    private InputManager inputManager;
    private EventManager eventManager;

    #endregion

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        print("Game Manager Initialized");

        scoreToWin = SceneController.sc.scoreToWin;
        nbOfPlayers = SceneController.sc.nbOfPlayers;

        Player.players = new Player[nbOfPlayers];

        for (int i = 0; i < nbOfPlayers; i++)
        {
            Player.players[i] = new Player(i);
        }

        mainCanvas.Initialize();
        inputManager = GetComponent<InputManager>();
        eventManager = GetComponent<EventManager>();
        inputManager.Initialize();

        GetComponent<GridManager>().Initialize();

        GetComponent<EventManager>().Initialize(); //need to execute after mainCanvas.Initialize(); 

        mainCanvas.UpdateScores();
    }

    public void TriggerVictory(int playerIndex)
    {
        inputManager.canPlay = false;
        mainCanvas.TriggerVictory(playerIndex);
    }

    public void CountScores()
    {
        int gwidth = 4;
        int gheight = 4;
        bool ok;
        int playerIndex;

        for (int p = 0; p < 2; p++)
        {
            for (int i = 0; i < gheight; i++)
            {
                for (int j = 0; j < gwidth - 3 + 1; j++)
                {
                    playerIndex = grid.GetSlot(i, j, p).playerIndex;
                    if (playerIndex != -1)
                    {
                        ok = playerIndex == grid.GetSlot(i, j + 1, p).playerIndex;
                        ok = ok && (playerIndex == grid.GetSlot(i, j + 2, p).playerIndex);

                        if (ok)
                        {
                            Player.players[playerIndex].score += 3;

                            grid.GetSlot(i, j, p).ScorePoint(playerIndex);
                            grid.GetSlot(i, j + 1, p).ScorePoint(playerIndex);
                            grid.GetSlot(i, j + 2, p).ScorePoint(playerIndex);
                        }

                    }

                    if (i < gheight - 3 + 1)
                    {
                        int k = p == 1 ? i : gheight - 1 - i;
                        int m = p == 1 ? 1 : -1;

                        playerIndex = grid.GetSlot(k, j, 0).playerIndex;
                        if (playerIndex != -1)
                        {
                            ok = playerIndex == grid.GetSlot(k + m, j + 1, 0).playerIndex;
                            ok = ok && (playerIndex == grid.GetSlot(k + 2 * m, j + 2, 0).playerIndex);

                            if (ok)
                            {
                                Player.players[playerIndex].score += 3;

                                grid.GetSlot(k, j, 0).ScorePoint(playerIndex);
                                grid.GetSlot(k + m, j + 1, 0).ScorePoint(playerIndex);
                                grid.GetSlot(k + 2 * m, j + 2, 0).ScorePoint(playerIndex);
                            }
                        }
                    }
                }
            }
        }
    }

    public void EndRound()
    {
        CountScores();
        mainCanvas.UpdateScores();

        for (int i = 0; i < nbOfPlayers; i++)
        {
            if (Player.players[i].score >= scoreToWin)
            {
                TriggerVictory(i);
                return;
            }
        }

        eventManager.AttributeEvents();
    }

    public void SetGeneralLightLevel(float intensity)
    {
        StartCoroutine(SetGeneralLightLevel(intensity, 0.1f));
    }

    IEnumerator SetGeneralLightLevel(float intensity, float coef)
    {
        while(Mathf.Abs(mainDirectionalLight.intensity - intensity) > 0.01f)
        {
            mainDirectionalLight.intensity = Mathf.Lerp(mainDirectionalLight.intensity, intensity, coef);
            yield return 0;
        }
    }
}

public enum Actions
{
    PlacePiece,
    BonusEvent,
    MalusEvent,
    Left,
    Right
}