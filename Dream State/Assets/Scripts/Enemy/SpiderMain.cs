using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMain : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public NavMeshAgent agent;

    [Header("Territory")]
    public Transform territoryCenter;
    public float territoryRadius = 10f;

    [Header("Chase")]
    public float maxChaseOutsideDistance = 6f;
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 8f;

    [Header("Combat")]
    public float attackRange = 1.5f;
    public float damage = 8f;
    public float damageCooldown = 1.2f;

    [Header("Detection")]
    public float detectionRange = 12f;

    float attackRangeSqr;
    float detectionRangeSqr;

    float nextDamageTime;

    Vector3 startPosition;

    IDamageable damageable;

    bool isChasing = false;
    bool isReturning = false;

    Vector3 lastKnownPlayerPosition;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.updateRotation = false;

        if (target == null)
            target = PlayerSingle.instance.transform;

        damageable = target.GetComponent<IDamageable>();

        startPosition = territoryCenter != null ? territoryCenter.position : transform.position;

        attackRangeSqr = attackRange * attackRange;
        detectionRangeSqr = detectionRange * detectionRange;
    }

    void Update()
    {
        if (target == null) return;

        float distToPlayerSqr = (target.position - transform.position).sqrMagnitude;
        float distFromTerritory = Vector3.Distance(transform.position, startPosition);

        bool insideTerritory = distFromTerritory <= territoryRadius;

        // DETECT PLAYER
        if (distToPlayerSqr <= detectionRangeSqr)
        {
            isChasing = true;
            isReturning = false;
            lastKnownPlayerPosition = target.position;
        }

        // CHASE LOGIC
        if (isChasing)
        {
            float distFromCenterToPlayer = Vector3.Distance(startPosition, target.position);

            // Si el player está dentro del territorio persecución normal
            if (distFromCenterToPlayer <= territoryRadius)
            {
                Chase(target.position);
            }
            else
            {
                // Player salió perseguir un poco más
                if (distFromTerritory <= territoryRadius + maxChaseOutsideDistance)
                {
                    Chase(target.position);
                }
                else
                {
                    // Ya se alejó demasiado dejar de perseguir
                    isChasing = false;
                    isReturning = true;
                }
            }
        }
        // RETURN TO TERRITORY
        else if (isReturning)
        {
            agent.isStopped = false;
            agent.SetDestination(startPosition);

            RotateTowards(startPosition);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
            {
                isReturning = false;
                agent.isStopped = true;
            }
        }
        else
        {
            agent.isStopped = true;
        }

        TryAttack(distToPlayerSqr);
    }

    void Chase(Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);

        RotateTowards(destination);
    }

    void RotateTowards(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
    }

    void TryAttack(float distanceSqr)
    {
        if (!isChasing || damageable == null)
            return;

        if (distanceSqr <= attackRangeSqr && Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageCooldown;
            damageable.TakeDamage(damage);
        }
    }

}