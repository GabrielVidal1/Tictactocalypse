public class Player
{
    public static Player[] players;

    public PlayerMode mode;


    public int score;

    public int indexInGameManager;

    public GameEvent[] events;
    //0 -> bonus
    //1 -> malus

    public bool[] canPlayEvent;
    public bool canPlayMain;

    public Player(int index)
    {
        indexInGameManager = index;
    }
}

public enum PlayerMode
{
    Normal,
    PlacingMode,
}