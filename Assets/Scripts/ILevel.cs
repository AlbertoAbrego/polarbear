using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    Vector3 EntranceSpot { get; }
    Vector3 SpawnSpot { get; }
    Vector3 ExitSpot { get; }
    Vector3[] SPOTS { get; }
    Vector3[] TABLES_POSITIONS { get; }
    public Vector3 GetEntranceSpot();
    public Vector3 GetSpawnSpot();
    public Vector3 GetExitSpot();
    public Vector3[] GetSpots();
    public Vector3[] GetTables();
    public string GetEntranceRoute();
    public string GetCorrectRouteClient(int table);
}
