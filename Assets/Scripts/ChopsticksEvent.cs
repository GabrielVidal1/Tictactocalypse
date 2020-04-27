using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopsticksEvent : MonoBehaviour
{

    [SerializeField] private Transform offsetParent;
    [SerializeField] private Transform pieceParent;


    private PlayerPiece piece;

    public void Focus()
    {
        GameEvent ge = GetComponent<GameEvent>();

        int offset = ((int)ge.choosenPosition - (int)Direction.West + 4 ) % 4;

        int rowIndex = offset % 3 == 0 ? 0 : 3;
        int pIndex = offset % 2 == 0 ? 1 : 0;

        //print(rowIndex + "   " + pIndex);


        List<Slot> possibleSlots = new List<Slot>();
        List<int> indexes = new List<int>();

        for (int j = 0; j < GameManager.gm.gridSize; j++)
        {
            Slot slot = GameManager.gm.grid.GetSlot(rowIndex, j, pIndex);
            if (!slot.IsVacant)
            {
                if (slot.GetPlayerPiece().playerIndex != ge.GetPlayerIndex())
                {
                    possibleSlots.Add(slot);
                    if (offset == 2 || offset == 3)
                        indexes.Add(j);
                    else
                        indexes.Add(GameManager.gm.gridSize - 1 - j);
                }
            }
        }

        if (possibleSlots.Count > 0)
        {
            int index = Random.Range(0, possibleSlots.Count);

            offsetParent.Translate(Vector3.back * indexes[index]);

            Slot choosenSlot = possibleSlots[index];
            piece = choosenSlot.GetPlayerPiece();
        }
        else
        {
            offsetParent.Translate(Vector3.back * 1.5f);

        }

    }

    public void AttachPieceToChopsticks()
    {
        if (piece == null)
            return;

        piece.transform.SetParent(pieceParent);

        piece.transform.localPosition = Vector3.zero;
        piece.transform.rotation = Quaternion.identity;

        piece.disableRigidBody = true;
        piece.state = PieceState.BeingDestroyed;
        Destroy(piece.GetComponent<Rigidbody>());
    }

    public void DestroyPiece()
    {
        if (piece != null)
            piece.DestroyPiece();
    }

}
