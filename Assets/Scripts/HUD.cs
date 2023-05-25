using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD sharedInstance;
    public GameObject queue;
    public GameObject cash;
    public GameObject Slider;
    public GameObject QueueElement;
    public GameObject TableHUDElement;
    public GameObject BistecHUDElement;
    public GameObject ChorizoHUDElement;
    public GameObject PastorHUDElement;
    private List<GameObject> ordersQueue = new();
    private List<GameObject> ordersCompleted = new();
    private float nextOrderHudPosition = 0;
    private List<int> ordersTableFrom;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void UpdateCash(float amount)
    {
        cash.GetComponent<TextMeshProUGUI>().text = "$"+ amount;
    }

    public void AddToOrdersQueue(int table, Dictionary<string, int> order)
    {
        GameObject orderHud = Instantiate(QueueElement, queue.transform, false);
        orderHud.transform.localPosition = new(0, nextOrderHudPosition, 0);
        GameObject tableHud = Instantiate(TableHUDElement, orderHud.transform, false);
        tableHud.GetComponentInChildren<TextMeshProUGUI>().text = "" + (table + 1);
        tableHud.transform.localPosition = new(-60f, 0, 0);
        GameObject[] tacosHud = new GameObject[order.Count];
        int indexForHud = 0;
        foreach(KeyValuePair<string, int> tacoType in order)
        {
            tacosHud[indexForHud] = Instantiate(TacoType(tacoType.Key), orderHud.transform, false);
            tacosHud[indexForHud].GetComponentInChildren<TextMeshProUGUI>().text = "" + tacoType.Value;
            tacosHud[indexForHud].transform.localPosition = new(indexForHud * 60f, 0, 0);
            indexForHud++;
        }
        GameObject slider = Instantiate(Slider, orderHud.transform, false);
        slider.GetComponent<RectTransform>().sizeDelta = new Vector2(60 * (indexForHud + 1), 60);
        ordersQueue.Add(orderHud);
        nextOrderHudPosition -= 60f;
        ordersTableFrom.Add(table);
    }

    GameObject TacoType(string type)
    {
        return type switch
        {
            "bistec" => BistecHUDElement,
            "chorizo" => ChorizoHUDElement,
            "pastor" => PastorHUDElement,
            _ => null,
        };
    }

    public void UpdateOrder(float time)
    {
        ordersQueue[0].transform.GetComponentInChildren<OrderHUD>().UpdateHUD(time);
    }

    public void PrepareNextOrder()
    {
        ordersCompleted.Add(ordersQueue[0]);
        ordersQueue.RemoveAt(0);
    }

    public void RemoveOrderDelivered(int table)
    {
        int orderToRemove = ordersTableFrom.IndexOf(table);
        
    }
}
