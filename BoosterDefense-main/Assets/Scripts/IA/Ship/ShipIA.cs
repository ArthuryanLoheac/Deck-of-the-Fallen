using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipIA : MonoBehaviour
{
    private Vector3 nextPosition;
    private GameObject nextPosObj;
    private bool canMove = false;
    private float nexTimeCanMove;
    public Vector2 timeMinMaxWait;
    private NavMeshAgent agent;
    private float coolDownNewPath, nextTimenewPath;
    public float speed;
    public float distanceMaxNewPos;

    private void SetTimeWaiting()
    {
        canMove = false;
        nexTimeCanMove = Time.time + Random.Range(timeMinMaxWait.x, timeMinMaxWait.y);
        agent.isStopped = true;
        nextTimenewPath = Time.time + coolDownNewPath;
    }

    private void SetNewPosition()
    {
        nextPosition = PlaneManager.instance.GetRandomPosOnPlane(transform.position, distanceMaxNewPos);
        nextPosObj.transform.position = nextPosition;
        agent.SetDestination(nextPosObj.transform.position);
        agent.isStopped = false;
    }

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        nextPosObj = new GameObject("DestinationShip");
        SetNewPosition();
        nextTimenewPath = Time.time + Random.Range(0, 6);
        coolDownNewPath = 6f;
    }

    private void UpdateMove()
    {
        if (!(Vector3.Distance(transform.position, nextPosition) > 0.5f))
            SetTimeWaiting();
        if (Time.time > nextTimenewPath) {
            SetTimeWaiting();
        }
    }

    void FixedUpdate()
    {
        if (canMove) {
            UpdateMove();
        } else {
            if (Time.time >= nexTimeCanMove) {
                canMove = true;
                SetNewPosition();
            }
        }
    }
}
