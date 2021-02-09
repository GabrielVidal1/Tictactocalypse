using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("PlayerPiece"))
        {
            other.transform.parent.GetComponent<PlayerPiece>().DestroyPiece();
        }
    }
}
