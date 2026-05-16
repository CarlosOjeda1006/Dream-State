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

    public ShooterHearingTrigger hearing;

    public Transform target;

    [Header("Patrol")]
    public Transform[] patrolPoints;

    [Header("Movement")]
    public float patrolSpeed = 3f;
    public float chargeSpeed = 7f;
    private float updateRate = 0.2f;
    private float timer;

    [Header("Territory")]
    public Transform territoryCenter;

    [Header("Kill")]
    public float killDistance = 1.5f;

    [Header("Audio")]
    private Animator animator;
    public ShooterEnemyAudio enemyAudio;

    State currentState;

    int patrolIndex;

    void OnEnable()
    {
        vision.OnPlayerDetected += HandleDetection;
        hearing.OnPlayerHeard += HandleDetection;
    }

    void OnDisable()
    {
        vision.OnPlayerDetected -= HandleDetection;
        hearing.OnPlayerHeard -= HandleDetection;
    }

    void Start()
    {
        currentState = State.Patrol;
        enemyAudio.PlayPatrol();

        agent.speed = patrolSpeed;

        GoToNextPatrolPoint();

        animator = GetComponentInChildren<Animator>();
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
            GoToNextPatrolPoint();
        }
    }

    void ChargeUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= updateRate)
        {
            timer = 0f;
            agent.SetDestination(target.position);
        }
    }

    void HandleDetection()
    {
        if (currentState != State.Patrol)
            return;

        currentState = State.Aim;

        agent.isStopped = true;
        animator.SetBool("detectedSomething", true);
        enemyAudio.PlaySearching();
        enemyAudio.PlayDetected();

        StartCoroutine(
            combat.AttackSequence(BeginCharge)
        );
    }

    void BeginCharge()
    {
        currentState = State.Charge;
        enemyAudio.PlayCharge();

        agent.isStopped = false;
        animator.SetBool("isCharging", true);

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

    public void ResetAnimations()
    {
        animator.SetBool("isCharging", false);
        animator.SetBool("detectedSomething", false);
        animator.SetBool("detectedPlayer", false);
    }

    public void GoToNextPatrolPoint()
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