using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : MonoBehaviour, ILevel
{
    public Vector3 EntranceSpot { get; } = new(-11.5f, -6.75f, 0); //ES
    public Vector3 SpawnSpot { get; } = new(-26.5f, -6.75f, 0);//SS
    public Vector3 ExitSpot { get; } = new(-11.5f, 15f, 0);//QS

    public Vector3[] SPOTS { get; } = 
    {
        new Vector3(-3, -6, 0),
        new Vector3(-3, 3, 0),
        new Vector3(-3, -3, 0)
    };

    public Vector3[] TABLES_POSITIONS { get; } = {
        new Vector3(1, 6, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, -6, 0),
        new Vector3(7, 6, 0),
        new Vector3(7, 0, 0),
        new Vector3(7, -6, 0),
        new Vector3(13, 6, 0),
        new Vector3(13, 0, 0),
        new Vector3(13, -6, 0)
    };
    public Vector3[] STANDUP_SPOTS { get; } =
    {
        new Vector3(1, 3, 0),
        new Vector3(1, -3, 0),
        new Vector3(7, 3, 0),
        new Vector3(7, -3, 0),
        new Vector3(13, 3, 0),
        new Vector3(13, -3, 0)
    };
    public Vector3 GetEntranceSpot() { return EntranceSpot; }
    public Vector3 GetSpawnSpot() { return SpawnSpot; }
    public Vector3 GetExitSpot() { return ExitSpot; }
    public Vector3[] GetTables() { return TABLES_POSITIONS; }
    public Vector3[] GetSpots() { return SPOTS; }
    public string GetEntranceRoute() { return "ESR"; }
    public string GetCorrectRouteClient(int table)
    {
        return (table % 3) switch
        {
            0 => GetRouteNewClient1(),
            1 => (Random.Range(0, 2) == 0) ? GetRouteNewClient1() : GetRouteNewClient2(),
            _ => GetRouteNewClient2()
        };
    }
    public string GetRouteNewClient1() { return "S0R,S1U,TnR"; }
    public string GetRouteNewClient2() { return "S0R,S2U,TnR"; }
    public string GetCorrectExitClientRoute(int table)
    {
        return (table % 3) switch
        {
            0 => GetRouteExitClient1(),
            _ => GetRouteExitClient2()
        };
    }
    public string GetRouteExitClient1() { return "S1L,S0D,ESL,QSU"; }
    public string GetRouteExitClient2() { return "S2L,S0D,ESL,QSU"; }
    public Vector3 GetCorrectStandupSpot(int table)
    {
        int table_column = table / 3;
        return (table % 3) switch
        {
            0 => STANDUP_SPOTS[table_column * 2],
            _ => STANDUP_SPOTS[table_column * 2 + 1]
        };
    }
}
