using UnityEngine;
using UnityEngine.AI;

public class MothAttack : MonoBehaviour
{
    public NavMeshAgent agent;

    public float attackDistance = 1.5f;
    public float moveSpeed = 5f;

    public void TickAttack(Transform target)
    {
        agent.speed = moveSpeed;

        agent.SetDestination(target.position);

        float distSqr =
            (target.position - transform.position).sqrMagnitude;

        if (distSqr <= attackDistance * attackDistance)
        {
            IDamageable dmg =
                target.GetComponent<IDamageable>();

            if (dmg != null)
            {
                dmg.TakeDamage(10f);
            }
        }
    }
}