using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    public Transform pieceCenter;
    public bool hasPiece;

    public int playerIndex
    {
        get
        {
            if (hasPiece && playerPiece.state == PieceState.OnSlot)
                return playerPiece.playerIndex;
            return -1;
        }
    }


    [SerializeField] Transform canvas;

    [SerializeField] private GameObject[] playersPreview;

    [SerializeField] private GameObject scorePreview;

    [SerializeField] private MeshRenderer ownerMesh;
    //[SerializeField] private Material emptyMaterial;

    private PlayerPiece playerPiece;
    private Animator animator;

    public PlayerPiece GetPlayerPiece()
    { return playerPiece; }

    [SerializeField] private SlotScoreUi scoreUi;

    public bool IsVacant
    {
        get
        {
            if (hasPiece)
                return playerPiece.state == PieceState.Lost ||
                    playerPiece.state == PieceState.BeingDestroyed;
            return true;
        }
    }

    public void SetPiece(PlayerPiece piece, Material mat)
    {
        ownerMesh.material = mat;
        playerPiece = piece;
        hasPiece = true;
        animator.SetBool("HasOwner", true);
    }

    public void SetPieceLost()
    {
        animator.SetBool("HasOwner", false);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        canvas.LookAt(GameManager.gm.mainCamera.transform);
        hasPiece = false;
        for (int i = 0; i < playersPreview.Length; i++)
        {
            Preview(i, false);
        }
    }

    public void Preview(int playerIndex, bool preview)
    {
        if (hasPiece)
            preview = preview && playerPiece.state == PieceState.Lost;

        playersPreview[playerIndex].SetActive(preview);
    }

    public void ScorePoint(int playerIndexWinner)
    {
        scoreUi.ScorePoint(playerIndexWinner);
    }

}
