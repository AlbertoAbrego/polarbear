using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver,
    pause,
}

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    private readonly string[] StarterTypeOfTacos =
    {
        "bistec",
        "chorizo",
        "pastor"
    };
    private float[] TacosPrices =
    {
        11,
        12,
        13
    };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_1 = { new Vector3(0, 2.25f) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_2 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_3 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f), new Vector3(2.25f, 0) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_4 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f), new Vector3(2.25f, 0), new Vector3(-2.25f, 0f) };
    private readonly Vector3[] CLIENTS_WALK_POSITIONS_1 = { Vector3.zero };
    private readonly Vector3[] CLIENTS_WALK_POSITIONS_2 = { new Vector3(-0.75f, 0, 0), new Vector3(0.75f, 0, 0) };
    private readonly Vector3[] CLIENTS_WALK_POSITIONS_3 = { new Vector3(-0.75f, -0.375f, 0), new Vector3(0.75f, -0.375f, 0), new Vector3(0, 0.375f, 0) };
    private readonly Vector3[] CLIENTS_WALK_POSITIONS_4 = { new Vector3(-0.75f, -0.375f, 0), new Vector3(0.75f, -0.375f, 0), new Vector3(-0.75f, 0.375f, 0), new Vector3(0.75f, 0.375f, 0) };
    private List<string> TypeOfTacosUnlocked = new List<string>();
    private Vector3 EntranceSpot = new(-11.5f, -6.75f, 0);
    private Vector3 SpawnSpot = new(-26.5f, -6.75f, 0);
    private Vector3 ExitSpot;
    private Vector3[] SPOTS;
    private Vector3[] TABLES;
    private FirstLevel firstLevel;
    private string entranceRouteLevel = "";
    private ILevel currentLevel;

    public GameState currentGameState;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        firstLevel = new FirstLevel();
        SetGameState(GameState.inGame);
        currentLevel = firstLevel;
        EntranceSpot = currentLevel.GetEntranceSpot();
        SpawnSpot = currentLevel.GetSpawnSpot();
        ExitSpot = currentLevel.GetExitSpot();
        SPOTS = currentLevel.GetSpots();
        TABLES = currentLevel.GetTables();
        entranceRouteLevel = currentLevel.GetEntranceRoute();
        Tables.sharedInstance.StartTables(TABLES.Length);
        foreach(string type in StarterTypeOfTacos)
        {
            TypeOfTacosUnlocked.Add(type);
        }
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
    }

    public string GetTypeOfTacos(int index)
    {
        return StarterTypeOfTacos[index];
    }

    public Vector3 GetSpawnSpot() { return SpawnSpot; }

    public void SetSpawnSpot(Vector3 newSpawnSpot) { SpawnSpot = newSpawnSpot; }

    public Vector3 GetEntranceSpot() { return EntranceSpot; }

    public void SetEntranceSpot(Vector3 newEntrance) { EntranceSpot = newEntrance; }

    public Vector3 GetExitSpot() { return ExitSpot; }

    public Vector3 GetSpotN(int n) { return SPOTS[n]; }

    public Vector3 GetTableN(int n) { return TABLES[n]; }

    public Vector3 StandUpClientsFromTable(int table) { return currentLevel.GetCorrectStandupSpot(table); }

    public string GetEntranceRouteLevel() { return entranceRouteLevel; }

    public string GetRouteClient(int table) { return currentLevel.GetCorrectRouteClient(table); }

    public string GetCorrectRouteExit(int table) { return currentLevel.GetCorrectExitClientRoute(table); }

    public Vector3[] GetSeatsPositions(int clients)
    {
        return (clients) switch
        {
            1 => CLIENTS_SEATS_POSITIONS_1,
            2 => CLIENTS_SEATS_POSITIONS_2,
            3 => CLIENTS_SEATS_POSITIONS_3,
            4 => CLIENTS_SEATS_POSITIONS_4,
            _ => throw new System.NotImplementedException()
        };
    }

    public Vector3[] GetStandupPositions(int clients)
    {
        return (clients) switch
        {
            1 => CLIENTS_WALK_POSITIONS_1,
            2 => CLIENTS_WALK_POSITIONS_2,
            3 => CLIENTS_WALK_POSITIONS_3,
            4 => CLIENTS_WALK_POSITIONS_4,
            _ => throw new System.NotImplementedException()
        };
    }

    public List<string> GetListTypeOfTacos()
    {
        return TypeOfTacosUnlocked;
    }

    public float GetPrice(string type)
    {
        return TacosPrices[TypeOfTacosUnlocked.IndexOf(type)];
    }
}