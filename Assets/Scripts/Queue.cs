using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public static Queue SharedInstance;
    private List<ClientGroup> queue = new List<ClientGroup>();
    public List<float> sizes = new List<float>();
    private Vector3 endOfQueue;

    private void Awake()
    {
        SharedInstance = this;
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
        //endOfQueue.y += (queue.Count == 1) ? 0 : Mathf.Max(cg.clientSize , sizes[^1]);
        cg.transform.position = endOfQueue;
        sizes.Add(cg.clientSize);
    }

    //TODO: para cuando se eliminen de la fila pasos a futuro
    //private void DeleteFromQueue()
    //{
    //    int table = Tables.GetAvailableTable();
    //    queue[0].MoveToTableAssigned(table);
    //    queue.RemoveAt(0);
    //    MoveQueue();
    //}

    //public void MoveQueue()
    //{
    //    int index = 0;
    //    for(int cg = 1; cg < queue.Count; cg++)
    //    {
    //        Vector3 newPosition = queue[cg].transform.position;
    //        newPosition.y -= sizes[index];
    //        StartCoroutine(queue[cg].MoveTo(newPosition, "D"));
    //        index++;
    //    }
    //    sizes.RemoveAt(0);
    //}
}
