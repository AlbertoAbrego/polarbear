using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tables : MonoBehaviour
{
    public static Tables sharedInstance;
    private bool[] availables;
    private ClientGroup[] clientsOnTables;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void StartTables(int totalTables)
    {
        availables = new bool[totalTables];
        clientsOnTables = new ClientGroup[totalTables];
        for (int i = 0; i < availables.Length; i++)
        {
            availables[i] = true;
        }
    }

    public int GetAvailable(int table = 0)
    {
        return (table < availables.Length) ? (availables[table]) ? table : GetAvailable(table + 1) : -1;
    }

    public void SetNotAvailable(int table, ClientGroup client)
    {
        availables[table] = false;
        clientsOnTables[table] = client;
    }

    public void SetAvailable(int table)
    {
        availables[table] = true;
        clientsOnTables[table] = null;
    }

    public void SetOrderCompleted(int table)
    {
        clientsOnTables[table].SetOrderCompleted();
    }
}
