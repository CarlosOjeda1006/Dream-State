using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SchoolShooterAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Aim,
        Investigate,
        Search,
        Charge
    }

    [Header("References")]
    public NavMeshAgent agent;
    public ShooterVision vision;
    public ShooterCombat combat;
    public Animator animator;

    public ShooterHearingTrigger hearing;

    public Transform target;

    [Header("Patrol")]
    public Transform[] patrolPoints;

    [Header("Movement")]
    public float patrolSpeed = 3f;
    public float chargeSpeed = 7f;
    private float updateRate = 0.2f;
    private float timer;

    [Header("Kill")]
    public float killDistance = 1.5f;

    [Header("Audio")]
    public ShooterEnemyAudio enemyAudio;

    [Header("Territory")]
    public Transform territoryCenter;

    State currentState;

    Vector3 lastKnownPosition;

    Coroutine searchRoutine;

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
        if (target == null)
        {
            target = PlayerSingle.instance.transform;
        }

        agent.updateRotation = false;

        currentState = State.Patrol;
        enemyAudio.PlayPatrol();

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

            case State.Investigate:
                InvestigateUpdate();
                break;

            case State.Search:
                SearchUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {
        if (agent.hasPath)
        {
            RotateTowards(agent.steeringTarget);
        }

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

        RotateTowards(target.position);

        float distSqr =
            (target.position - transform.position).sqrMagnitude;

        if (distSqr <= killDistance * killDistance)
        {
            KillPlayer();
        }
    }

    void SearchUpdate()
    {

    }

    void InvestigateUpdate()
    {
        if (agent.hasPath)
        {
            RotateTowards(agent.steeringTarget);
        }

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.3f)
        {
            BeginSearch();
        }
    }

    void RotateTowards(Vector3 position)
    {
        Vector3 dir = position - transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude <= 0.001f)
            return;

        Quaternion rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rot,
            8f * Time.deltaTime
        );
    }

    void HandleDetection()
    {
        if (currentState == State.Aim ||
            currentState == State.Charge)
            return;

        lastKnownPosition = target.position;

        if (currentState == State.Search)
        {
            BeginCharge();
            return;
        }

        currentState = State.Aim;

        agent.isStopped = true;

        Debug.Log(animator);

        animator.SetBool("detectedSomething", true);

        enemyAudio.PlaySearching();
        enemyAudio.PlayDetected();

        StartCoroutine(
            combat.AttackSequence(BeginInvestigate)
        );
    }

    void BeginInvestigate()
    {
        currentState = State.Investigate;

        enemyAudio.PlayCharge();

        agent.isStopped = false;
        agent.speed = chargeSpeed;

        animator.SetBool("isCharging", true);

        agent.SetDestination(lastKnownPosition);
    }

    void BeginSearch()
    {
        currentState = State.Search;

        agent.isStopped = true;
        agent.ResetPath();

        animator.SetBool("isCharging", false);
        animator.SetBool("detectedPlayer", false);

        vision.ResetDetection();
        hearing.ResetHearing();

        searchRoutine = StartCoroutine(SearchRoutine());
    }

    void BeginCharge()
    {

        if (searchRoutine != null)
        {
            StopCoroutine(searchRoutine);
        }

        currentState = State.Charge;

        agent.isStopped = false;

        agent.speed = chargeSpeed * 1.15f;

        agent.SetDestination(target.position);

        animator.SetBool("isCharging", true);
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

    void ReturnToPatrol()
    {
        currentState = State.Patrol;

        agent.isStopped = false;

        ResetAnimations();
        animator.SetBool("ReturnToWalk", true);

        vision.ResetDetection();
        hearing.ResetHearing();

        enemyAudio.PlayPatrol();

        agent.speed = patrolSpeed;

        GoToNextPatrolPoint();
    }

    IEnumerator SearchRoutine()
    {
        float duration = 4f;

        float timer = 0f;

        float angle = -70f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            Vector3 dir =
                Quaternion.Euler(0f, angle, 0f) *
                transform.forward;

            Vector3 lookPoint =
                transform.position + dir;

            RotateTowards(lookPoint);

            angle = Mathf.PingPong(timer * 140f, 140f) - 70f;

            yield return null;
        }

        ReturnToPatrol();
    }
}