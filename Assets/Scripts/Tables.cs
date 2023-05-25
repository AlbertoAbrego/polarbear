using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tables : MonoBehaviour
{
    public static Tables sharedInstance;
    public GameObject Table_Prefab;
    public GameObject Cart_Prefab;
    private bool[] availables;
    private ClientGroup[] clientsOnTables;
    private GameObject[] tables;

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

    public void SpawnTables()
    {
        Vector3[] tablesP = GameManager.sharedInstance.GetTables();
        tables = new GameObject[tablesP.Length];
        int index = 0;
        foreach(Vector3 position in tablesP)
        {
            tables[index] = Instantiate(Table_Prefab,position, Quaternion.identity);
            index++;
        }
        Instantiate(Cart_Prefab, GameManager.sharedInstance.GetCartPosition(), Quaternion.identity);
    }

    public void ReadyToOrder(int table)
    {
        GameObject clientsStatus = new("client status");
        clientsStatus.AddComponent<SpriteRenderer>();
        clientsStatus.transform.localPosition = Vector3.zero;
        clientsStatus.GetComponent<SpriteRenderer>().enabled = true;
        clientsStatus.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("orderReady");
        clientsStatus.transform.SetParent(tables[table].transform, false);
    }

    public void OrderTaken(int table)
    {
        tables[table].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }
}
