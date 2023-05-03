using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    public static QueueController sharedInstance;
    public Vector3 queueSpot;
    public List<ClientGroupController> clients = new();
    private float timeElapsed = 0;
    public Table[] tables = new Table[9];

    private void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        StartTables();
        SetSpotPosition(-11.5f, -6.75f);
    }

    void Update()
    {
        if (timeElapsed < 5)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0;
            if (clients.Count > 0)
            {
                int table = TableAvailable(0);
                if (table > -1)
                {
                    tables[table].SetOccupied();
                    RemoveClientFromQueue(table);
                }
            }
        }
    }

    int TableAvailable(int index)
    {
        if (index < 9 && tables[index].IsAvailable())
        {
            return index;
        }
        else if (index < 9)
        {
            return TableAvailable(index + 1);
        }
        else
        {
            return -1;
        }
    }

    void StartTables()
    {
        for (int i = 0; i < 9; i++)
        {
            tables[i] = new Table();
        }
    }

    public Vector3 GetQueueSpot() { return queueSpot; }
    public void SetSpotPosition(float x, float y) { queueSpot = new Vector3(x, y, 0); }
    public void AddClientToQueue(ClientGroupController newClient) 
    {
        clients.Add(newClient);
        newClient.transform.position = queueSpot;
        queueSpot.y += (newClient.GetClientsSize()>2) ? 2.5f : 1f;
    }
    public void RemoveClientFromQueue(int tableTarget)
    {
        clients[0].MoveToCall(tableTarget);
        ClientGroupController clientRemoving = clients[0];
        clients.RemoveAt(0);
        queueSpot.y -= (clientRemoving.GetClientsSize() > 2) ? 2.5f : 1f;
        clients.ForEach(client =>
        {
            client.transform.position -= (clientRemoving.GetClientsSize() > 2) ? new Vector3(0, 2.5f, 0) : new Vector3(0, 1f, 0);
        });
        //clientes avanzan en la fila
    }
}
