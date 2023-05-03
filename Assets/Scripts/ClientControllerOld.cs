using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    public Order[] CreateOrder()
    {
        int tacosEstimation = Random.Range(1, 16);
        int orderSize = 0;
        while(tacosEstimation > 0)
        {
            orderSize++;
            tacosEstimation -= 5;
        }
        Order[] orders = new Order[orderSize];
        int iterations = 0;
        while (iterations < orderSize)
        {
            orders[iterations] = new()
            {
                Amount = Random.Range(1, 6),
                Type = GameManager.sharedInstance.GetTypeOfTacos(Random.Range(0, 3))
            };
            iterations++;
        }
        return orders;
    }
}
