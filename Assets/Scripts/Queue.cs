using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public static Queue sharedInstance;
    private List<ClientGroup> queue = new List<ClientGroup>();
    public List<float> sizes = new List<float>();
    private Vector3 endOfQueue;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        endOfQueue = GameManager.sharedInstance.GetEntranceSpot();
    }

    public void AddGroupClientToQueue(ClientGroup cg)
    {
        queue.Add(cg);
        if(queue.Count > 1)
        {
            if(cg.clientSize != sizes[^1])
            {
                endOfQueue.y += 1.125f;
            }
            else if(cg.clientSize == 1)
            {
                endOfQueue.y += 0.75f;
            }
            else
            {
                endOfQueue.y += 1.5f;
            }
        }
        cg.transform.position = endOfQueue;
        sizes.Add(cg.clientSize);
        int table = Tables.sharedInstance.GetAvailable();
        if(table > -1)
        {
            SendClientsToTable(table);
        }
    }

    public void SendClientsToTable(int table)
    {
        if(queue.Count > 0)
        {
            Tables.sharedInstance.SetNotAvailable(table, queue[0]);
            StartCoroutine(queue[0].MoveToTable(table));
            queue.RemoveAt(0);
            MoveQueue();
        }
    }


    public void MoveQueue()
    {
        int index = 0;
        for (int cg = 1; cg < queue.Count; cg++)
        {
            Vector3 newPosition = queue[cg].transform.position;
            newPosition.y -= sizes[index];
            StartCoroutine(queue[cg].MoveTo(newPosition, "D"));
            index++;
        }
        sizes.RemoveAt(0);
    }
}
