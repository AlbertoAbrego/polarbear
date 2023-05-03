using System.Collections;
using UnityEngine;

public class ClientGroup : MonoBehaviour
{
    public float clientSize;
    //private readonly Vector3 SPEEDH = new(0.005f, 0, 0);
    private readonly Vector3 SPEEDH = new(0.05f, 0, 0);
    //private readonly Vector3 SPEEDV = new(0, 0.005f, 0);
    private readonly Vector3 SPEEDV = new(0, 0.05f, 0);
    private int tableAssigned = 0;
    private string entranceRoute = "";
    private string exitRoute = "";

    public ClientGroup()
    {

    }

    private void Start()
    {
        entranceRoute = GameManager.sharedInstance.GetEntranceRouteLevel();
        StartCoroutine(MoveToEntrance());
    }

    public void MoveToTableAssigned(int table)
    {
        tableAssigned = table;
        GameManager.sharedInstance.GetRouteClient(tableAssigned);
    }

    //public IEnumerator MoveToCall(float amount, string direction)
    //{
    //    Vector3 newPosition = transform.position;
    //    switch (direction)
    //    {
    //        case "L":
    //            newPosition.x -= amount;
    //            yield return StartCoroutine(LeftMove(newPosition));
    //            break;
    //        case "R":
    //            newPosition.x += amount;
    //            yield return StartCoroutine(RightMove(newPosition));
    //            break;
    //        case "U":
    //            newPosition.y += amount;
    //            yield return StartCoroutine(UpMove(newPosition));
    //            break;
    //        case "D":
    //            newPosition.y -= amount;
    //            yield return StartCoroutine(DownMove(newPosition));
    //            break;
    //    }
    //}
    
    IEnumerator MoveToEntrance()
    {
        yield return StartCoroutine(Routing(entranceRoute));
        Queue.SharedInstance.AddGroupClientToQueue(this);
    }

    IEnumerator Routing(string route)
    {
        int index = 0;
        string step;
        while(index < route.Length)
        {
            step = route.Substring(index,3);
            switch (step.Substring(0, 1))
            {
                case "E":
                    yield return StartCoroutine(MoveTo(GameManager.sharedInstance.GetEntranceSpot(), step.Substring(2, 1)));
                    break;
                case "Q":
                    yield return MoveTo(GameManager.sharedInstance.GetExitSpot(), step.Substring(2, 1));
                    break;
                case "S":
                    yield return MoveTo(
                        GameManager.sharedInstance.GetSpotN(
                            int.Parse(
                                step.Substring(1,1))
                            ),
                        step.Substring(2, 1));
                    break;
                case "T":
                    yield return MoveTo(
                        GameManager.sharedInstance.GetTableN(
                            int.Parse(step.Substring(1,1))
                            ),
                        step.Substring(2, 1));
                    break;
            }
            index+=3;
        }

    }

    public IEnumerator MoveTo(Vector3 spot, string direction)
    {
        switch (direction)
        {
            case "L":
                yield return StartCoroutine(LeftMove(spot));
                break;
            case "R":
                yield return StartCoroutine(RightMove(spot));
                break;
            case "U":
                yield return StartCoroutine(UpMove(spot));
                break;
            case "D":
                yield return StartCoroutine(DownMove(spot));
                break;
        }
    }

    IEnumerator RightMove(Vector3 destination)
    {
        while (transform.position.x < destination.x)
        {
            transform.Translate(SPEEDH);
            yield return null;
        }
        transform.position = destination;
    }

    IEnumerator UpMove(Vector3 destination)
    {
        while (transform.position.y < destination.y)
        {
            transform.Translate(SPEEDV);
            yield return null;
        }
        transform.position = destination;
    }

    IEnumerator LeftMove(Vector3 destination)
    {
        while (transform.position.x > destination.x)
        {
            transform.Translate(-SPEEDH);
            yield return null;
        }
        transform.position = destination;
    }

    IEnumerator DownMove(Vector3 destination)
    {
        while (transform.position.y > destination.y)
        {
            transform.Translate(-SPEEDV);
            yield return null;
        }
        transform.position = destination;
    }
}
