using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGroup : MonoBehaviour
{
    public float clientSize;
    public int numberOfClients;
    //private readonly Vector3 SPEEDH = new(0.005f, 0, 0);
    private readonly Vector3 SPEEDH = new(0.06f, 0, 0);
    //private readonly Vector3 SPEEDV = new(0, 0.005f, 0);
    private readonly Vector3 SPEEDV = new(0, 0.06f, 0);
    private int tableAssigned;
    private int timeToStay; //segundos
    private Dictionary<string, int> order = new Dictionary<string, int>();
    private float timeElapsed;
    private float money;
    private bool canBeClicked;
    private bool orderTaken;
    private bool orderCompleted;
    private bool orderDelivered;
    private bool finishedEating;

    private void Update()
    {
        if(orderDelivered)
        {
            if(timeElapsed >= timeToStay)
            {
                orderDelivered = false;
                canBeClicked = true;
                finishedEating = true;
            }
            timeElapsed += Time.deltaTime;
        }
    }

    private void Start()
    {
        canBeClicked = false;
        orderTaken = false;
        orderCompleted = false;
        orderDelivered = false;
        finishedEating = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(MoveToEntrance());
        CreateGroupOrder();
    }

    void SitClients()
    {
        Vector3[] positions = GameManager.sharedInstance.GetSeatsPositions(numberOfClients);
        for (int i = 0; i < positions.Length; i++)
        {
            transform.GetChild(i).localPosition = positions[i];
        }
        //TODO: esperar un tiempo, en lo que toman decision de que comer
        Tables.sharedInstance.ReadyToOrder(tableAssigned);
    }

    void StandUpClients()
    {
        transform.position = GameManager.sharedInstance.StandUpClientsFromTable(tableAssigned);
        Vector3[] walk_positions = GameManager.sharedInstance.GetStandupPositions(numberOfClients);
        for(int i = 0;i < walk_positions.Length;i++)
        {
            transform.GetChild(i).localPosition = walk_positions[i];
        }
    }

    void CreateGroupOrder()
    {
        int iterations = numberOfClients;
        while(iterations > 0)
        {
            List<string> tacoTypes = new(GameManager.sharedInstance.GetListTypeOfTacos());
            CreateIndividualOrder(Random.Range(2, 9), tacoTypes);
            iterations--;
        }
        SetTimeToStay();
    }

    void CreateIndividualOrder(int amountOfTacos, List<string> typeOfTacosAvailable)
    {
        int typeSelected;
        typeSelected = Random.Range(0, typeOfTacosAvailable.Count);
        if(amountOfTacos > 3)
        {
            AddTacosToOrder(3, typeOfTacosAvailable[typeSelected]);
            money += 3 * GameManager.sharedInstance.GetPrice(typeOfTacosAvailable[typeSelected]);
            //añadir a money 3*precio
            typeOfTacosAvailable.RemoveAt(typeSelected);
            CreateIndividualOrder(amountOfTacos - 3, typeOfTacosAvailable);
        }
        else
        {
            AddTacosToOrder(amountOfTacos, typeOfTacosAvailable[typeSelected]);
            money += amountOfTacos * GameManager.sharedInstance.GetPrice(typeOfTacosAvailable[typeSelected]);
            //añadir a money amountOfTacos*precio
        }
    }

    void AddTacosToOrder(int amount, string type)
    {
        if (order.ContainsKey(type))
        {
            order[type] += amount;
        }
        else
        {
            order.Add(type, amount);
        }
    }

    void SetTimeToStay()
    {
        int timeMax = 0;
        List<string> types = GameManager.sharedInstance.GetListTypeOfTacos();
        foreach (string type in types) 
        {

            timeMax = (order.ContainsKey(type) && order[type]>timeMax) ? order[type] : timeMax;
        }
        timeToStay = timeMax;
    }

    IEnumerator MoveToEntrance()
    {
        yield return StartCoroutine(Routing(GameManager.sharedInstance.GetEntranceRouteLevel()));
        Queue.sharedInstance.AddGroupClientToQueue(this);
    }

    public IEnumerator MoveToTable(int table)
    {
        tableAssigned = table;
        yield return StartCoroutine(Routing(GameManager.sharedInstance.GetRouteClient(table)));
        SitClients();
        canBeClicked = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public IEnumerator MoveToExit()
    {
        yield return StartCoroutine(Routing(GameManager.sharedInstance.GetCorrectRouteExit(tableAssigned)));
        Destroy(gameObject);
    }

    IEnumerator Routing(string route)
    {
        int index = 0;
        string step;
        while(index < route.Length)
        {
            step = route.Substring(index,3);
            switch (step.Substring(0, 1))
            {
                case "E":
                    yield return MoveTo(GameManager.sharedInstance.GetEntranceSpot(), step.Substring(2, 1));
                    break;
                case "Q":
                    yield return MoveTo(GameManager.sharedInstance.GetExitSpot(), step.Substring(2, 1));
                    break;
                case "S":
                    yield return MoveTo(
                        GameManager.sharedInstance.GetSpotN(
                            int.Parse(
                                step.Substring(1,1))
                            ),
                        step.Substring(2, 1));
                    break;
                case "T":
                    yield return MoveTo(
                        GameManager.sharedInstance.GetTableN(
                            tableAssigned
                            ),
                        step.Substring(2, 1));
                    break;
            }
            index+=4;
        }

    }

    public IEnumerator MoveTo(Vector3 spot, string direction)
    {
        switch (direction)
        {
            case "L":
                yield return StartCoroutine(LeftMove(spot));
                break;
            case "R":
                yield return StartCoroutine(RightMove(spot));
                break;
            case "U":
                yield return StartCoroutine(UpMove(spot));
                break;
            case "D":
                yield return StartCoroutine(DownMove(spot));
                break;
        }
    }

    IEnumerator RightMove(Vector3 destination)
    {
        while (transform.position.x < destination.x)
        {
            transform.Translate(SPEEDH);
            yield return null;
        }
        transform.position = destination;
    }

    IEnumerator UpMove(Vector3 destination)
    {
        while (transform.position.y < destination.y)
        {
            transform.Translate(SPEEDV);
            yield return null;
        }
        transform.position = destination;
    }

    IEnumerator LeftMove(Vector3 destination)
    {
        while (transform.position.x > destination.x)
        {
            transform.Translate(-SPEEDH);
            yield return null;
        }
        transform.position = destination;
    }

    IEnumerator DownMove(Vector3 destination)
    {
        while (transform.position.y > destination.y)
        {
            transform.Translate(-SPEEDV);
            yield return null;
        }
        transform.position = destination;
    }

    private void OnMouseDown()
    {
        if(canBeClicked)
        {
            if (!orderTaken)
            {
                HUD.sharedInstance.AddToOrdersQueue(tableAssigned, order);
                Orders.sharedInstance.AddOrderToQueue(order, tableAssigned);
                Tables.sharedInstance.OrderTaken(tableAssigned);
                orderTaken = true;
                Debug.Log("order taken");
            }
            if (orderCompleted)
            {
                Debug.Log("order delivered");
                orderDelivered = true;
                canBeClicked = false;
                orderCompleted = false;
                Orders.sharedInstance.OrderDelivered(tableAssigned);
            }
            if (finishedEating)
            {
                Debug.Log("order paid");
                Player.sharedInstance.AddMoney(money);
                canBeClicked = false;
                Tables.sharedInstance.SetAvailable(tableAssigned);
                StandUpClients();
                StartCoroutine(Queue.sharedInstance.SendClientsToTable());
                StartCoroutine(MoveToExit());
            }
        }
    }

    public void SetOrderCompleted()
    {
        orderCompleted = true;
    }
}
