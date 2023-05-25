using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public static Queue sharedInstance;
    private List<ClientGroup> queue = new List<ClientGroup>();
    public List<float> sizes = new List<float>();
    public Vector3 endOfQueue;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        endOfQueue = GameManager.sharedInstance.GetEntranceSpot();
    }

    private void Update()
    {
        
    }

    public void AddGroupClientToQueue(ClientGroup cg)
    {
        queue.Add(cg);
        if(queue.Count > 1)
        {
            if(cg.clientSize != sizes[^1])
            {
                endOfQueue.y += 1.125f;//.75
            }
            else if(cg.clientSize == 1)
            {
                endOfQueue.y += 0.75f;//.5
            }
            else
            {
                endOfQueue.y += 1.5f;//1
            }
        }
        cg.transform.position = endOfQueue;
        sizes.Add(cg.clientSize);
        StartCoroutine(SendClientsToTable());
    }

    public IEnumerator SendClientsToTable()
    {
        if(Tables.sharedInstance.GetAvailable() > -1)
        {
            while(Tables.sharedInstance.GetAvailable() > -1 && queue.Count > 0)
            {
                int table = Tables.sharedInstance.GetAvailable();
                Tables.sharedInstance.SetNotAvailable(table, queue[0]);
                StartCoroutine(queue[0].MoveToTable(table));
                float clientMovedY = queue[0].transform.position.y;
                queue.RemoveAt(0);
                sizes.RemoveAt(0);
                yield return StartCoroutine(MoveQueue(clientMovedY));
            }
        }
    }


    IEnumerator MoveQueue(float clientMovedPosition)
    {
        if(queue.Count > 0)
        {
            float distance = queue[0].transform.position.y - clientMovedPosition;
            endOfQueue.y -= distance;
            int cg = 0;
            while (cg < queue.Count)
            {
                Vector3 newPosition = queue[cg].transform.position;
                newPosition.y -= distance;
                queue[cg].transform.position = newPosition;
                cg++;
            }

        }
        yield return null;
    }
}
