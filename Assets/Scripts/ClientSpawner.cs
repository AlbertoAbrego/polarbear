using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public static ClientSpawner sharedInstance;

    public GameObject[] clientGroupPrefab = new GameObject[4];
    private Vector3 spawnSpot = new(-26.5f, -6.75f, 0);
    private float time = 0f;
    private int waitForNewClientInS = 0;
    private bool stopSpawning = false;

    public void Awake()
    {
        sharedInstance = this;
    }

    private void Update()
    {
        if (!stopSpawning)
        {
            if (time < waitForNewClientInS)
            {
                time += Time.deltaTime;
            }
            else
            {
                SpawnClient();
                time = 0;
                waitForNewClientInS = Random.Range(1, 11);
            }
        }
    }

    public void SpawnClient()
    {
        spawnSpot = GameManager.sharedInstance.GetSpawnSpot();
        Instantiate(clientGroupPrefab[Random.Range(0, 4)], spawnSpot, Quaternion.identity);
    }

    public void StopSpawning() { stopSpawning = true; }
}
