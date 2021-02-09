using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEvent : MonoBehaviour
{
    public Transform alienShip;
    public Transform alienShipRay;
    public Transform target;



    private bool focusOnSlot = false;

    private Vector3 desiredPosition;
    private PlayerPiece absorbedPiece;


    private void Update()
    {
        if (!focusOnSlot)
        {
            desiredPosition = target.position;
            desiredPosition.y = alienShip.position.y;
        }

        alienShip.position = Vector3.Lerp(alienShip.position,
            desiredPosition, 0.1f);
    }

    public void Focus()
    {
        focusOnSlot = true;

        List<Slot> possibleSlots = new List<Slot>();
        for (int i = 0; i < GameManager.gm.gridSize; i++)
        {
            for (int j = 0; j < GameManager.gm.gridSize; j++)
            {
                Slot slot = GameManager.gm.grid.GetSlot(i, j, 0);
                if (!slot.IsVacant)
                {
                    possibleSlots.Add(slot);
                }
            }
        }

        if (possibleSlots.Count > 0)
        {
            Slot choosenSlot = possibleSlots[Random.Range(0, possibleSlots.Count)];

            desiredPosition = choosenSlot.pieceCenter.position;
            desiredPosition.y = alienShip.position.y;
            absorbedPiece = choosenSlot.GetPlayerPiece();
        }
        else
        {
            ResumeAnimation();
        }
    }

    public void AbsorbePiece()
    {
        if (absorbedPiece != null)
        {
            absorbedPiece.transform.SetParent(alienShipRay);
            absorbedPiece.transform.localPosition = Vector3.zero;

            absorbedPiece.state = PieceState.BeingDestroyed;

            absorbedPiece.disableRigidBody = true;
            Destroy(absorbedPiece.GetComponent<Rigidbody>());
        }
    }

    public void DestroyPiece()
    {
        if (absorbedPiece != null)
            absorbedPiece.DestroyPiece();
    }

    public void ResumeAnimation()
    {
        focusOnSlot = false;
    }

}
