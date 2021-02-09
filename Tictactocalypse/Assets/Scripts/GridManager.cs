using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    private int[][] selectedPosition;

    private PlayGrid grid;

    private GameManager gm;
    private EventManager em;
    private MainCanvas mainCanvas;

    private bool gridIsFull = false;

    public void Initialize()
    {
        print("Grid Manager Initialized");

        gm = GetComponent<GameManager>();
        grid = gm.grid;
        em = GetComponent<EventManager>();

        mainCanvas = gm.mainCanvas;

        selectedPosition = new int[gm.nbOfPlayers][];
        for (int i = 0; i < gm.nbOfPlayers; i++)
        {
            selectedPosition[i] = new int[3];
            selectedPosition[i][0] = -1;
        }
    }

    public void InputDirection(int playerIndex, int direction)
    {
        bool IsGridFull = false;
        int offsetDir = (direction == 0) ? 1: direction;

        int k = -1;

        if (selectedPosition[playerIndex][0] == 0)
        {
            grid.PreviewRow(selectedPosition[playerIndex][1], playerIndex, false);

            k = 0;
            selectedPosition[playerIndex][1] = (selectedPosition[playerIndex][1] + 4 + direction) % 4;
            while (!IsGridFull && grid.NbVacantSlots(selectedPosition[playerIndex][1], playerIndex) == 0) //ROW IS FULL
            {
                selectedPosition[playerIndex][1] = (selectedPosition[playerIndex][1] + 4 + offsetDir) % 4;
                k++;
                if (k > 4)
                {
                    IsGridFull = true;
                }
            }

            grid.PreviewRow(selectedPosition[playerIndex][1], playerIndex, true);

        } else if (selectedPosition[playerIndex][0] == 1)
        {
            grid.PreviewSlot(selectedPosition[playerIndex][1], selectedPosition[playerIndex][2], playerIndex, false);

            k = 0;
            selectedPosition[playerIndex][2] = (selectedPosition[playerIndex][2] + 4 + direction) % 4;
            while (!IsGridFull && !grid.IsSlotVacant(selectedPosition[playerIndex][1], selectedPosition[playerIndex][2], playerIndex)) //SLOT IS FULL
            {
                selectedPosition[playerIndex][2] = (selectedPosition[playerIndex][2] + 4 + offsetDir) % 4;
                k++;
                if (k > 4)
                {
                    IsGridFull = true;
                }
            }

            grid.PreviewSlot(selectedPosition[playerIndex][1], selectedPosition[playerIndex][2], playerIndex, true);
        }
    }

    public void MainButton(int playerIndex)
    {
        mainCanvas.playersUI[playerIndex].TriggerPressButton(-1);

        if (gridIsFull)
            return;

        if (selectedPosition[playerIndex][0] == -1)
        {
            if (Player.players[playerIndex].canPlayMain)
            {
                selectedPosition[playerIndex][0] = 0;
                selectedPosition[playerIndex][1] = 0;
                selectedPosition[playerIndex][2] = 0;

                mainCanvas.TriggerPlaceMode(playerIndex, true, 0);

                mainCanvas.playersUI[playerIndex].PressMainButton();
                Player.players[playerIndex].mode = PlayerMode.PlacingMode;

                InputDirection(playerIndex, 0);
            }
            else
            {
                mainCanvas.playersUI[playerIndex].TriggerDisabledButton(-1);
                return;
            }
        }
        else if (Player.players[playerIndex].mode == PlayerMode.PlacingMode)
        {
            if (selectedPosition[playerIndex][0] < 1)
            {
                selectedPosition[playerIndex][0]++;

                if (selectedPosition[playerIndex][0] == 1)
                {
                    mainCanvas.TriggerPlaceMode(playerIndex, true, 1);
                    grid.PreviewRow(selectedPosition[playerIndex][1], playerIndex, false);
                    InputDirection(playerIndex, 0);

                    if (grid.NbVacantSlots(selectedPosition[playerIndex][1], playerIndex) == 1)
                    {
                        selectedPosition[playerIndex][2] = 0;
                        while (!grid.IsSlotVacant(selectedPosition[playerIndex][1], selectedPosition[playerIndex][2], playerIndex)) //ROW IS FULL
                        {
                            selectedPosition[playerIndex][2]++;
                        }
                        selectedPosition[playerIndex][0]++;
                        MainButton(playerIndex);
                        return;
                    }


                }
            }
            else //selectedPosition[playerIndex][0] >= 2
            {
                Player.players[playerIndex].mode = PlayerMode.Normal;
                mainCanvas.TriggerPlaceMode(playerIndex, false, 1);
                grid.PreviewSlot(selectedPosition[playerIndex][1], selectedPosition[playerIndex][2], playerIndex, false);
                if (true)
                {
                    Slot slot = grid.GetSlot(selectedPosition[playerIndex][1], selectedPosition[playerIndex][2], playerIndex);

                    if (slot.IsVacant)
                    {
                        if (slot.hasPiece)
                        {
                            slot.GetPlayerPiece().ReplacePieceOnSlot();
                        }

                        PlayerPiece piece = Instantiate(gm.playersData[playerIndex].playerPiecePrefab, slot.pieceCenter);
                        piece.playerIndex = playerIndex;
                        piece.Initialize(slot);

                        slot.hasPiece = true;
                    }
                }
                selectedPosition[playerIndex][0] = -1;
            }
        }

    }
}
