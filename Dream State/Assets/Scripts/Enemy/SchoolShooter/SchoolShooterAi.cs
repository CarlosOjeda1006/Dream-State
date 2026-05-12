using UnityEngine;
using UnityEngine.AI;

public class SchoolShooterAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Aim,
        Charge
    }

    [Header("References")]
    public NavMeshAgent agent;
    public ShooterVision vision;
    public ShooterCombat combat;

    public Transform target;

    [Header("Patrol")]
    public Transform[] patrolPoints;

    [Header("Movement")]
    public float patrolSpeed = 3f;
    public float chargeSpeed = 7f;

    [Header("Kill")]
    public float killDistance = 1.5f;

    State currentState;

    int patrolIndex;

    void OnEnable()
    {
        vision.OnPlayerDetected += HandleDetection;
    }

    void OnDisable()
    {
        vision.OnPlayerDetected -= HandleDetection;
    }

    void Start()
    {
        currentState = State.Patrol;

        agent.speed = patrolSpeed;

        GoToNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolUpdate();
                break;

            case State.Charge:
                ChargeUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("Patrol update");
            GoToNextPatrolPoint();
        }
    }

    void ChargeUpdate()
    {
        if (target == null)
            return;

        agent.SetDestination(target.position);

        Debug.Log("Charging");

        float distSqr = (target.position - transform.position).sqrMagnitude;

        if (distSqr <= killDistance * killDistance)
        {
            KillPlayer();
        }
    }

    void HandleDetection()
    {
        if (currentState != State.Patrol)
            return;

        currentState = State.Aim;

        agent.isStopped = true;
        Debug.Log("Detecting");

        StartCoroutine(
            combat.AttackSequence(BeginCharge)
        );
    }

    void BeginCharge()
    {
        currentState = State.Charge;

        agent.isStopped = false;

        agent.speed = chargeSpeed;
    }

    void KillPlayer()
    {
        IDamageable damageable =
        target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(9999f);
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(
            patrolPoints[patrolIndex].position
        );

        patrolIndex++;

        if (patrolIndex >= patrolPoints.Length)
        {
            patrolIndex = 0;
        }
    }
}