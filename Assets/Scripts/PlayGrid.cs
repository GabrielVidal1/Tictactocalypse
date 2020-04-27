using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGrid : MonoBehaviour {

    [SerializeField] private Row[] rows;

    public Slot GetSlot(int rowIndex, int slotIndex, int playerIndex)
    {
        if (playerIndex % 2 == 0)
            return rows[rowIndex].slots[slotIndex];
        else
            return rows[slotIndex].slots[rowIndex];
    }

    public bool IsSlotVacant(int rowIndex, int slotIndex, int playerIndex)
    {
        return GetSlot(rowIndex, slotIndex, playerIndex).IsVacant;
    }

    public bool isFull()
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            count += NbVacantSlots(i, 0);
        }
        return count == 0;
    }

    public int NbVacantSlots(int rowIndex, int playerIndex)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (IsSlotVacant(rowIndex, i, playerIndex))
                count++;
        }
        return count;
    }

    public void PreviewRow(int rowIndex, int playerIndex, bool preview)
    {
        for (int i = 0; i < 4; i++)
        {
            PreviewSlot(rowIndex, i, playerIndex, preview);
        }
    }

    public void PreviewSlot(int rowIndex, int slotIndex, int playerIndex, bool preview)
    {
        GetSlot(rowIndex, slotIndex, playerIndex).Preview(playerIndex, preview);
    }
}
