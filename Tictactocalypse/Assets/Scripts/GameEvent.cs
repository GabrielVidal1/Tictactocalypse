using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{

    public List<Direction> usedSides; //5 = 4 sides + top of board
    public Direction[] possiblePositions; //4 position (4 sides)

    public Texture icon;
    public float cooldown;

    [SerializeField] private MeshRenderer[] coloredMeshes;

    private Animator animator;
    private List<int> usedEventSlots;

    [HideInInspector] public Direction choosenPosition;

    private EventManager eventManager;

    private int playerIndex;
    public int GetPlayerIndex()
    { return playerIndex; }

    public void Initialize(EventManager em)
    {
        eventManager = em;
    }

    public bool TriggerEvent(int playerIndex)
    {
        foreach (MeshRenderer mesh in coloredMeshes)
        {
            mesh.material = GameManager.gm.playersData[playerIndex].slotOwnerMat;
        }

        bool canBeSummoned = OrientToRandomSide();

        if (canBeSummoned)
        {
            animator = GetComponent<Animator>();
            animator.SetTrigger("PlayEvent");
        }

        return canBeSummoned;
    }

    public bool OrientToRandomSide()
    {
        Direction normalPos = possiblePositions[0];
        usedEventSlots = new List<int>();

        //Find real used sides
        for (int i = 0; i < usedSides.Count; i++)
            usedSides[i] -= normalPos;


        List<Direction> positions = new List<Direction>();
        foreach (Direction pos in possiblePositions)
        {
            bool canBeHere = true;
            if (eventManager.AreEventRestricted())
            {
                foreach (Direction usedDir in usedSides)
                {
                    int index = ((int)pos + (int)usedDir + 4) % 4;

                    //print(pos + " used :" + (int)usedDir + " is vacant ? " + GameManager.gm.vacantEventSlots[index]);
                    canBeHere = canBeHere && eventManager.vacantEventSlots[index];
                }
            }
            if (canBeHere)
            {
                positions.Add(pos);
            }
        }
        if (positions.Count > 0)
        {
            choosenPosition = positions[Random.Range(0, positions.Count)];

            foreach (Direction dir in usedSides)
            {
                int index = ((int)choosenPosition + (int)dir + 4) % 4;
                eventManager.vacantEventSlots[index] = false;
                usedEventSlots.Add(index);
            }

            //print("choosen position : " + choosenPosition);
            float angle = 90f * (choosenPosition - normalPos);
            transform.Rotate(Vector3.up, angle);

            return true;
        }
        else
        {
            return false;
        }

    }

    public void Autodestroy()
    {
        FreeEventSlots();
        Destroy(gameObject);
    }

    private void FreeEventSlots()
    {
        foreach (int index in usedEventSlots)
            eventManager.vacantEventSlots[index] = true;
    }

    public void SetGeneralLightLevel(float intensity)
    {
        GameManager.gm.SetGeneralLightLevel(intensity);
    }
}

public enum Direction
{
    North,
    East,
    South,
    West,
    TopOfBoard
}