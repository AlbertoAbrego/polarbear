using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table
{
    private bool available;
    private Order[][] order;

    public Table()
    {
        available = true;
    }

    public bool IsAvailable() { return available; }

    public void SetOccupied() { available = false; }

    public void SetAvailable() { available = true; }

    public Order[][] GetOrder() { return order; }

    public void SetOrder(Order[][] newOrder) {  order = newOrder; }
}
