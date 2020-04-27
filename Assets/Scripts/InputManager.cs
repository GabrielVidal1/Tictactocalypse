using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyBinding[] playersKeyBinding;


    private GameManager gm;
    private EventManager eventManager;

    private GridManager gridManager;

    public bool canPlay = true;

    public void Initialize()
    {
        print("Input Manager Initialized");

        gm = GetComponent<GameManager>();
        eventManager = GetComponent<EventManager>();
        gridManager = GetComponent<GridManager>();
    }

    void Update()
    {
        if (canPlay)
        {
            for (int i = 0; i < gm.nbOfPlayers; i++)
            {
                if (Input.GetKeyDown(playersKeyBinding[i].GetKeyFromAction(Actions.PlacePiece)))
                    gridManager.MainButton(i);

                if (Player.players[i].mode == PlayerMode.Normal)
                {
                    if (Input.GetKeyDown(playersKeyBinding[i].GetKeyFromAction(Actions.BonusEvent)))
                        PressEventButton(i, 0);

                    if (Input.GetKeyDown(playersKeyBinding[i].GetKeyFromAction(Actions.MalusEvent)))
                        PressEventButton(i, 1);
                }
                else if (Player.players[i].mode == PlayerMode.PlacingMode)
                {
                    if (Input.GetKeyDown(playersKeyBinding[i].GetKeyFromAction(Actions.Left)))
                        gridManager.InputDirection(i, -1);

                    if (Input.GetKeyDown(playersKeyBinding[i].GetKeyFromAction(Actions.Right)))
                        gridManager.InputDirection(i, 1);

                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneController.sc.LoadMainMenu();
            }
        }
    }

    public void PressEventButton(int playerIndex, int index)
    {
        gm.mainCanvas.playersUI[playerIndex].TriggerPressButton(index);

        if (Player.players[playerIndex].canPlayEvent[index])
        {
            eventManager.PlayEvent(index, playerIndex);
        }
        else
        {
            gm.mainCanvas.playersUI[playerIndex].TriggerDisabledButton(index);
        }
    }
}

[System.Serializable]
public struct KeyBinding
{
    public KeyCode upKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    public string upKeyString;
    public string leftKeyString;
    public string rightKeyString;

    public KeyCode GetKeyFromAction(Actions action)
    {
        switch (action)
        {
            case Actions.PlacePiece:
                return upKey;
            case Actions.BonusEvent:
                return leftKey;
            case Actions.MalusEvent:
                return rightKey;
            case Actions.Left:
                return leftKey;
            case Actions.Right:
                return rightKey;
        }
        return 0;
    }
}