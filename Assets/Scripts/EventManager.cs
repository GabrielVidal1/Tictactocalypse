using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameEvent[] bonuses;
    [SerializeField] private GameEvent[] maluses;


    private bool disableEventRestriction;
    public bool AreEventRestricted()
    { return !disableEventRestriction; }

    [HideInInspector]
    public bool[] vacantEventSlots;
    // 0 -> North
    // 1 -> East
    // 2 -> South
    // 3 -> West

    private GameManager gm;

    public void Initialize()
    {
        disableEventRestriction = !SceneController.sc.areEventRestricted;

        print("Event Manager Initialized");
        
        gm = GetComponent<GameManager>();

        vacantEventSlots = new bool[4];
        for (int j = 0; j < 4; j++)
            vacantEventSlots[j] = true;


        for (int i = 0; i < gm.nbOfPlayers; i++)
        {
            Player.players[i].canPlayEvent = new bool[2];
            Player.players[i].events = new GameEvent[2];

            Player.players[i].canPlayMain = true;

            gm.mainCanvas.playersUI[i].UpdatePlayerEventsUI();


            for (int j = 0; j < 2; j++)
            {
                //SetRandomEvent(i, j);
                Player.players[i].canPlayEvent[j] = false;
            }
        }
    }

    public void SetRandomEvent(int playerIndex, int gameEventIndexToReplace)
    {
        if (gameEventIndexToReplace == 0)
        {
            Player.players[playerIndex].events[gameEventIndexToReplace] =
                bonuses[Random.Range(0, bonuses.Length)];
        }
        else if (gameEventIndexToReplace == 1)
        {
            Player.players[playerIndex].events[gameEventIndexToReplace] =
                maluses[Random.Range(0, maluses.Length)];
        }

        gm.mainCanvas.UpdateEventIcons(playerIndex, gameEventIndexToReplace);
    }

    public void PlayEvent(int gameEventIndex, int playerIndex)
    {
        GameEvent gameEvent = Instantiate(Player.players[playerIndex].events[gameEventIndex]);

        gameEvent.Initialize(this);

        if (!gameEvent.TriggerEvent(playerIndex))
        {
            Destroy(gameEvent.gameObject);

            //CANT PLAY EVENT
        }
        else
        {
            
            //SetRandomEvent(playerIndex, gameEventIndex);
            //START COOLDOWN OF NEW EVENT

            gm.mainCanvas.playersUI[playerIndex].StartCooldownOnEvent(gameEventIndex);
            Player.players[playerIndex].events[gameEventIndex] = null;
            Player.players[playerIndex].canPlayEvent[gameEventIndex] = false;

            gm.mainCanvas.playersUI[playerIndex].UpdatePlayerEventsUI(gameEventIndex);
        }

    }

    public void AttributeEvents()
    {
        for (int i = 0; i < gm.nbOfPlayers; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                SetRandomEvent(i, j);
                Player.players[i].canPlayEvent[j] = true;
            }
        }
    }


}
