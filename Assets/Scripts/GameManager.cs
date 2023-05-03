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
    private readonly string[] TypeOfTacos =
    {
        "bistec",
        "chorizo",
        "pastor"
    };
    private Vector3 EntranceSpot = new(-11.5f, -6.75f, 0);
    private Vector3 SpawnSpot = new(-26.5f, -6.75f, 0);
    private Vector3 ExitSpot;
    private Vector3[] SPOTS;
    private Vector3[] TABLES;
    //esto en queue controller
    private List<ClientGroup> queuedClients = new List<ClientGroup>();
    private FirstLevel firstLevel = new FirstLevel();
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
        SetGameState(GameState.inGame);
        currentLevel = firstLevel;
        EntranceSpot = currentLevel.GetEntranceSpot();
        SpawnSpot = currentLevel.GetSpawnSpot();
        ExitSpot = currentLevel.GetExitSpot();
        SPOTS = currentLevel.GetSpots();
        TABLES = currentLevel.GetTables();
        entranceRouteLevel = currentLevel.GetEntranceRoute();
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
    }

    public string GetTypeOfTacos(int index)
    {
        return TypeOfTacos[index];
    }

    public Vector3 GetSpawnSpot() { return SpawnSpot; }

    public void SetSpawnSpot(Vector3 newSpawnSpot) { SpawnSpot = newSpawnSpot; }

    public Vector3 GetEntranceSpot() { return EntranceSpot; }

    public void SetEntranceSpot(Vector3 newEntrance) { EntranceSpot = newEntrance; }

    public Vector3 GetExitSpot() { return ExitSpot; }

    public Vector3 GetSpotN(int n) { return SPOTS[n]; }

    public Vector3 GetTableN(int n) { return TABLES[n];}

    public void AddToQueue(ClientGroup groupOfClients)
    {
        queuedClients.Add(groupOfClients);
    }

    public string GetEntranceRouteLevel() { return entranceRouteLevel; }

    public string GetRouteClient(int table) { return currentLevel.GetCorrectRouteClient(table); }
}