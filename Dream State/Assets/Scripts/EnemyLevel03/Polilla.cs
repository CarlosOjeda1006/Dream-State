using UnityEngine;
using UnityEngine.AI;

public class Polilla : MonoBehaviour
{
    public enum State
    {
        Patrol,
        MoveToLight,
        Idle,
        Attack
    }

    [Header("References")]
    public NavMeshAgent agent;

    [Header("Idle")]
    public float minIdleTime = 4f;
    public float maxIdleTime = 8f;
    private Animator animator;

    float currentIdleTime;
    float idleTimer;

    public MothLightTracker lightTracker;
    public MothPatrol patrol;
    public MothIdle idle;
    public MothAttack attack;

    public Transform target;

    LuzPoililla occupiedLight;

    public State currentState;

    [Header("Audio")]
    public PolillaAudio polillaAudio;


    void Start()
    {


        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;

        ChangeState(State.Patrol);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolUpdate();
                break;

            case State.MoveToLight:
                MoveToLightUpdate();
                break;

            case State.Idle:
                IdleUpdate();
                break;

            case State.Attack:
                AttackUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {
        if (lightTracker.CurrentLight != null)
        {
            ChangeState(State.MoveToLight);
            return;
        }
        animator.SetBool("isWalking", true);
        polillaAudio.PlayPatrol();

        patrol.TickPatrol();
    }

    void MoveToLightUpdate()
    {
        LuzPoililla light = lightTracker.CurrentLight;

        if (light == null)
        {
            occupiedLight = null;

            ChangeState(State.Patrol);
            return;
        }

        agent.SetDestination(light.transform.position);

        RotateTowards(light.transform.position);

        if (!agent.pathPending &&
            agent.remainingDistance <= 1.2f)
        {
            occupiedLight = light;

            ChangeState(State.Idle);
        }
    }

    void IdleUpdate()
    {
        if (lightTracker.CurrentLight == null)
        {
            occupiedLight = null;

            ChangeState(State.Patrol);

            return;
        }

        if (lightTracker.CurrentLight != occupiedLight)
        {
            ChangeState(State.MoveToLight);

            return;
        }

        idle.TickIdle();

        idleTimer += Time.deltaTime;

        if (idleTimer >= currentIdleTime)
        {
            lightTracker.IgnoreLightTemporarily(
                occupiedLight,
                4f
            );

            occupiedLight = null;

            ChangeState(State.Patrol);

            return;
        }

        if (target != null)
        {
            ChangeState(State.Attack);
        }
    }

    void AttackUpdate()
    {
        if (target == null)
        {
            ChangeState(State.Idle);
            return;
        }

        attack.TickAttack(target);
        animator.SetBool("isRunning", true);
        polillaAudio.PlayAttack();
    }

    public void SetPlayerTarget(Transform player)
    {
        target = player;
    }

    public void ClearPlayerTarget()
    {
        target = null;
    }

    void ChangeState(State newState)
    {
        currentState = newState;

        patrol.enabled = newState == State.Patrol;
        idle.enabled = newState == State.Idle;

        if (newState == State.Idle)
        {
            idleTimer = 0f;

            currentIdleTime =
                Random.Range(minIdleTime, maxIdleTime);
        }
    }

    void RotateTowards(Vector3 position)
    {
        Vector3 dir = position - transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rot,
            6f * Time.deltaTime
        );
    }
}