using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawnerOld : MonoBehaviour
{
    
    public GameObject clientGroup;
    private readonly Vector3 spawnSpot = new (-26.5f, -6.75f, 0);

    void Start()
    {
        StartCoroutine(SpawnClients(10));
    }

    IEnumerator SpawnClients(int numberClients)
    {
        while(numberClients > 0)
        {
            Instantiate(clientGroup, spawnSpot, Quaternion.identity);
            yield return new WaitForSeconds(2);
            numberClients--;
        }
    }
}
