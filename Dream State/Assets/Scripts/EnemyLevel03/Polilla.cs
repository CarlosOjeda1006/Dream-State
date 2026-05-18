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

    public MothLightTracker lightTracker;
    public MothPatrol patrol;
    public MothIdle idle;
    public MothAttack attack;

    public Transform target;

    public State currentState;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;

        ChangeState(State.Patrol);
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

        patrol.TickPatrol();
    }

    void MoveToLightUpdate()
    {
        LuzPoililla light = lightTracker.CurrentLight;

        if (light == null)
        {
            ChangeState(State.Patrol);
            return;
        }

        agent.SetDestination(light.transform.position);

        RotateTowards(light.transform.position);

        if (!agent.pathPending &&
            agent.remainingDistance <= 1.2f)
        {
            ChangeState(State.Idle);
        }
    }

    void IdleUpdate()
    {
        if (lightTracker.CurrentLight == null)
        {
            ChangeState(State.Patrol);
            return;
        }

        idle.TickIdle();

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