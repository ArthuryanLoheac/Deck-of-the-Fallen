using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDestination : MonoBehaviour
{
    public Transform Destination;
    public NavMeshAgent agent;

    void Start()
    {
        agent.SetDestination(Destination.position);
    }
}
