using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    private Rigidbody rb;
    private Slot slot;

    public PieceState state;

    [SerializeField] public Material slotOwnerMat;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image loadingImage;

    [SerializeField] private float warpDelayMultiplier = 1f;

    [HideInInspector] public bool disableRigidBody = false;

    [HideInInspector]
    public int playerIndex;

    [SerializeField] private MeshRenderer meshRenderer;

    public void Initialize(Slot slot)
    {
        this.slot = slot;
        slot.SetPiece(this, slotOwnerMat);
        rb = GetComponent<Rigidbody>();

        meshRenderer.material = GameManager.gm.playersData[playerIndex].normalMat;

        canvas.gameObject.SetActive(false);
        state = PieceState.OnSlot;
    }

    public void DestroyPiece()
    {
        slot.hasPiece = false;
        slot.SetPieceLost();

        Destroy(gameObject);
    }

    public void ReplacePieceOnSlot()
    {
        state = PieceState.BeingDestroyed;

        slot.hasPiece = false;
        meshRenderer.material = GameManager.gm.playersData[playerIndex].lostMat;
        canvas.gameObject.SetActive(false);

        StartCoroutine(ReplacePieceOnSlotCoroutine());
    }

    private IEnumerator ReplacePieceOnSlotCoroutine()
    {
        while(transform.localScale.sqrMagnitude > 0.01f)
        {
            yield return 0;
            transform.localScale *= 0.9f;
        }
        Destroy(gameObject);
    }

    private float timer;

    private void Update()
    {
        if (!disableRigidBody)
        {
            if ((slot.pieceCenter.position - transform.position).sqrMagnitude > 0.03f)
            {
                if (state == PieceState.OnSlot)
                {
                    state = PieceState.Lost;
                    meshRenderer.material = GameManager.gm.playersData[playerIndex].lostMat;
                    slot.SetPieceLost();

                    canvas.gameObject.SetActive(true);
                }
            }


            if (rb.velocity.sqrMagnitude < 0.1f)
            {
                if (state == PieceState.Lost)
                {
                    timer += Time.deltaTime;

                    loadingImage.fillAmount = timer / (warpDelayMultiplier * ConstantManager.piecesWarpDelay);

                    if (timer > warpDelayMultiplier * ConstantManager.piecesWarpDelay)
                    {
                        StartCoroutine(GoToSlot());
                        canvas.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                timer = 0f;
            }
        }
    }

    IEnumerator GoToSlot()
    {
        state = PieceState.MovingToSLot;

        rb.isKinematic = true;
        rb.detectCollisions = false;

        while ((slot.pieceCenter.position - transform.position).sqrMagnitude > 0.0001)
        {
            transform.position = Vector3.Lerp(transform.position, slot.pieceCenter.position, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.3f);
            yield return 0;
        }
        transform.position = slot.pieceCenter.position;
        transform.rotation = Quaternion.identity;

        rb.detectCollisions = true;
        rb.isKinematic = false;

        meshRenderer.material = GameManager.gm.playersData[playerIndex].normalMat;

        state = PieceState.OnSlot;

        slot.SetPiece(this, slotOwnerMat);
    }

    public void Invicible(float duration)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;

        meshRenderer.material = GameManager.gm.playersData[playerIndex].invicibleMat;
    }

}

public enum PieceState
{
    OnSlot,
    MovingToSLot,
    Lost,
    BeingDestroyed
}
