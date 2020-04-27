using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionEvent : MonoBehaviour
{
    public Transform[] spots;

    public Transform spotTarget;

    private bool focusOnSlot;

    private List<Slot> possibleSlots;


    private void Update()
    {
        if (!focusOnSlot)
        {
            foreach (Transform spot in spots)
            {
                spot.LookAt(spotTarget);
            }
        }
    }

    private void Start()
    {
        GameEvent ge = GetComponent<GameEvent>();
        int playerIndex = ge.GetPlayerIndex();

        possibleSlots = new List<Slot>();
        for (int i = 0; i < GameManager.gm.gridSize; i++)
        {
            for (int j = 0; j < GameManager.gm.gridSize; j++)
            {
                Slot slot = GameManager.gm.grid.GetSlot(i, j, 0);
                if (slot.playerIndex == playerIndex)
                {
                    possibleSlots.Add(slot);
                }
            }
        }
    }

    public void Focus()
    {
        if (possibleSlots.Count == 0)
            return;

        Slot choosenSlot = possibleSlots[Random.Range(0, possibleSlots.Count)];

        focusOnSlot = true;
        foreach (Transform spot in spots)
        {
            spot.LookAt(choosenSlot.pieceCenter);
        }

        PlayerPiece pp = choosenSlot.GetPlayerPiece();

        if (pp != null)
            pp.Invicible(10);
    }
}
