using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGroupController : MonoBehaviour
{
    public GameObject clientPrefab;

    private Vector3 currentQueuePosition;
    private readonly Vector3 SPEEDH = new(0.005f, 0, 0);
    private readonly Vector3 SPEEDV = new(0, 0.005f, 0);
    private readonly Vector3[] TABLES_POSITIONS = {
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
    //table seats despues
    private readonly Vector3 SPOT_A = new(-3, 3, 0);
    private readonly Vector3 SPOT_B = new(-3, -3, 0);
    private readonly Vector3 SPOT_KEY = new(-3, -6, 0);

    private readonly Vector3[] CLIENTS_POSITIONS_1 = { new Vector3(0, -0.75f) };
    private readonly Vector3[] CLIENTS_POSITIONS_2 = { new Vector3(-0.75f, -0.75f), new Vector3(0.75f, -0.75f) };
    private readonly Vector3[] CLIENTS_POSITIONS_3 = { new Vector3(-0.75f, -0.75f), new Vector3(0.75f, -0.75f), new Vector3(0, 0.75f)};
    private readonly Vector3[] CLIENTS_POSITIONS_4 = { new Vector3(-0.75f, -0.75f), new Vector3(0.75f, -0.75f), new Vector3(-0.75f, 0.75f), new Vector3(0.75f, 0.75f)};

    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_1 = { new Vector3(0, 2.25f) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_2 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_3 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f), new Vector3(2.25f, 0) };
    private readonly Vector3[] CLIENTS_SEATS_POSITIONS_4 = { new Vector3(0, 2.25f), new Vector3(0, -2.25f), new Vector3(2.25f, 0), new Vector3(-2.25f, 0f) };

    private GameObject[] clients;
    private Order[][] tableOrder;

    void Start()
    {
        int numberOfClients = Random.Range(1, 5);
        clients = new GameObject[numberOfClients];
        SpawnClients();
        StartCoroutine(MoveToQueuePosition());
    }

    IEnumerator MoveToQueuePosition()
    {
        currentQueuePosition = QueueController.sharedInstance.GetQueueSpot();
        while (transform.position.x < currentQueuePosition.x)
        {
            transform.Translate(SPEEDH);
            yield return null;
        }
        transform.position = currentQueuePosition;
        QueueController.sharedInstance.AddClientToQueue(this);
    }

    public void MoveToCall(int tableTarget)
    {
        int line = tableTarget % 3;
        switch (line)
        {
            case 0:
                StartCoroutine(MoveFromQueueToSpot(0, tableTarget));
                break;
            case 1:
                StartCoroutine(MoveFromQueueToSpot(Random.Range(0, 2), tableTarget));
                break;
            case 2:
                StartCoroutine(MoveFromQueueToSpot(1, tableTarget));
                break;
        }
    }

    IEnumerator MoveFromQueueToSpot(int spot, int tableTarget)
    {
        while (transform.position.x < SPOT_KEY.x)
        {
            transform.Translate(SPEEDH);
            yield return null;
        }
        transform.position = SPOT_KEY;
        yield return StartCoroutine(MoveFromSpotKeyToSpot(spot, tableTarget));
    }

    IEnumerator MoveFromSpotKeyToSpot(int spot, int tableTarget)
    {
        Vector3 spotSelected = (spot == 1) ? SPOT_B : SPOT_A;
        while (transform.position.y < spotSelected.y)
        {
            transform.Translate(SPEEDV);
            yield return null;
        }
        transform.position = spotSelected;
        yield return StartCoroutine(MoveFromSpotToTable(tableTarget));
    }

    IEnumerator MoveFromSpotToTable(int tableTarget)
    {

        while (transform.position.x < TABLES_POSITIONS[tableTarget].x)
        {
            transform.Translate(SPEEDH);
            yield return null;
        }
        transform.position = TABLES_POSITIONS[tableTarget];
        PutOnSeats(tableTarget);
    }

    private void SpawnClients()
    {
        int numberClients = clients.Length;
        Vector3[] positions =
            (numberClients == 4) ? CLIENTS_POSITIONS_4 :
            (numberClients == 3) ? CLIENTS_POSITIONS_3 :
            (numberClients == 2) ? CLIENTS_POSITIONS_2 : CLIENTS_POSITIONS_1;
        int iterations = 0;
        while(numberClients > 0)
        {
            clients[iterations] = Instantiate(clientPrefab, positions[iterations], Quaternion.identity, transform);
            clients[iterations].transform.localPosition = positions[iterations];
            numberClients--;
            iterations++;
        }
    }

    public int GetClientsSize()
    {
        return clients.Length;
    }

    private void PutOnSeats(int tableTarget)
    {
        int numberClients = clients.Length;
        Vector3[] positions =
            (numberClients == 4) ? CLIENTS_SEATS_POSITIONS_4 :
            (numberClients == 3) ? CLIENTS_SEATS_POSITIONS_3 :
            (numberClients == 2) ? CLIENTS_SEATS_POSITIONS_2 : CLIENTS_SEATS_POSITIONS_1;
        int iterations = 0;
        while (numberClients > 0)
        {
            clients[iterations].transform.localPosition = positions[iterations];
            numberClients--;
            iterations++;
        }
        PlaceOrders(tableTarget);
    }

    private void PlaceOrders(int tableTarget)
    {
        int iterations = 0;
        tableOrder = new Order[clients.Length][];
        while(iterations < clients.Length)
        {
            tableOrder[iterations] = clients[iterations].GetComponent<ClientController>().CreateOrder();
            //contamos los tactos y de ahi sacamos el tiempo que se quedaran
            iterations++;
        }
        QueueController.sharedInstance.tables[tableTarget].SetOrder(tableOrder);
        //for (int i = 0; i < tableOrder.Length; i++)
        //{
        //    for (int j = 0; j < tableOrder[i].Length; j++) 
        //    {
        //        Debug.Log(tableOrder[i][j].Amount + " - " + tableOrder[i][j].Type + "C-" + i);
        //    }
        //}
        //Debug.Log("finished client");
    }

}
