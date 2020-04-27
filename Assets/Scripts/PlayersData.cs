using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public PlayerPiece playerPiecePrefab;
    public Color playerColor;

    public Texture playerIcon;

    public Material normalMat;
    public Material lostMat;
    public Material invicibleMat;

    public Material slotOwnerMat;


}
