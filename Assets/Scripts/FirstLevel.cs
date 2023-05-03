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

    //esto yo creo que en otro lado, tal vez clientspawner
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_1 = { new Vector3(0, 2.25f) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_2 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_3 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f), new Vector3(2.25f, 0) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_4 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f), new Vector3(2.25f, 0), new Vector3(-2.25f, 0f) };

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
    public Vector3 GetEntranceSpot() { return EntranceSpot; }
    public Vector3 GetSpawnSpot() { return SpawnSpot; }
    public Vector3 GetExitSpot() { return ExitSpot; }
    public Vector3[] GetTables() { return TABLES_POSITIONS; }
    public Vector3 GetTablePosition(int n) { return TABLES_POSITIONS[n]; }
    public Vector3[] GetSpots() { return SPOTS; }
    public Vector3 GetSpotsPosition(int n) { return SPOTS[n]; }
    public string GetEntranceRoute() { return "ESR"; }
    public string GetCorrectRouteClient(int table)
    {
        return (table % 3) switch
        {
            0 => GetRouteNewClient1(),
            1 => GetRouteNewClient2(),
            _ => (Random.Range(0, 2) == 0) ? GetRouteNewClient1() : GetRouteNewClient2(),
        };
    }
    public string GetRouteNewClient1() { return "S0R,S1U,TnR"; }
    public string GetRouteNewClient2() { return "S0R,S2U,TnR"; }
    public string GetRouteExitClient1() { return "S1L,S0D,ESL,QSU"; }
    public string GetRouteExitClient2() { return "S2L,S0D,ESL,QSU"; }
    //NOTE: Guardar la mesa del cliente en los datos del cliente

}
