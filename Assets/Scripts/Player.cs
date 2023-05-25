using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player sharedInstance;
    [SerializeField]
    private float money;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void AddMoney(float payment)
    {
        money += payment;
        HUD.sharedInstance.UpdateCash(money);
        Debug.Log(money);
    }
}
