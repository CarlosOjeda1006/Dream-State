using UnityEngine;
using UnityEngine.AI;

public class MothPatrol : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform[] patrolPoints;

    int patrolIndex;

    public void TickPatrol()
    {
        if (patrolPoints.Length == 0)
            return;

        if (!agent.hasPath ||
            agent.remainingDistance <= 1f)
        {
            agent.SetDestination(
                patrolPoints[patrolIndex].position
            );

            patrolIndex++;

            if (patrolIndex >= patrolPoints.Length)
                patrolIndex = 0;
        }
    }
}