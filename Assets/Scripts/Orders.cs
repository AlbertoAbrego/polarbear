using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public static Orders sharedInstance;
    private bool makingOrder;
    private float timeElapsed, timeUntilOrderCompleted;
    private List<Dictionary<string, int>> ordersQueue = new();
    private List<int> ordersQueueTables = new();
    private List<Dictionary<string, int>> ordersCompleted = new();
    private List<int> ordersCompletedTables = new();

    private void Awake()
    {
        sharedInstance = this;
        makingOrder = false;
    }

    private void Update()
    {
        if(makingOrder)
        {
            if(timeElapsed >= timeUntilOrderCompleted)
            {
                timeElapsed = 0;
                ordersCompleted.Add(ordersQueue[0]);
                ordersQueue.RemoveAt(0);
                Tables.sharedInstance.SetOrderCompleted(ordersQueueTables[0]);
                ordersCompletedTables.Add(ordersQueueTables[0]);
                ordersQueueTables.RemoveAt(0);
                HUD.sharedInstance.PrepareNextOrder();
                if(ordersQueue.Count > 0 )
                {
                    timeUntilOrderCompleted = SetOrderTime();
                }
                else
                {
                    makingOrder = false;
                }
            }
            else
            {
                timeElapsed += Time.deltaTime;
                HUD.sharedInstance.UpdateOrder(Time.deltaTime/timeUntilOrderCompleted);
            }
        }
    }

    public void AddOrderToQueue(Dictionary<string, int> order, int table)
    {
        ordersQueue.Add(order);
        ordersQueueTables.Add(table);
        if(!makingOrder)
        {
            timeUntilOrderCompleted = SetOrderTime();
            makingOrder=true;
        }
    }

    float SetOrderTime()
    {
        float totalTime = 0;
        foreach(string type in ordersQueue[0].Keys)
        {
            totalTime += ordersQueue[0][type];
        }
        return totalTime;
    }

    public void OrderDelivered(int table)
    {
        ordersCompleted.RemoveAt(ordersCompletedTables.IndexOf(table));
        ordersCompletedTables.RemoveAt(ordersCompletedTables.IndexOf(table));
        HUD.sharedInstance.RemoveOrderDelivered(table);
    }
}
