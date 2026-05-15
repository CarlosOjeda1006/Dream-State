using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderDistractionController : MonoBehaviour
{
    public SchoolShooterAI shooterMain;
    public NavMeshAgent agent;

    public float distractionStoppingDistance = 1.2f;
    public float lookAroundTime = 2f;
    public float distractionWaitTime = 3f;
    public float returnStoppingDistance = 1.2f;
    public float rotationSpeed = 8f;
    public float maxDistractionTravelTime = 6f;
    public float maxReturnTravelTime = 8f;
    public float navMeshSearchRadius = 3f;

    Coroutine distractionRoutine;

    Vector3 homePosition;
    float originalStoppingDistance;

    bool isInvestigating;

    void Awake()
    {
        if (shooterMain == null)
            shooterMain = GetComponent<SchoolShooterAI>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        homePosition = shooterMain != null && shooterMain.territoryCenter != null
            ? shooterMain.territoryCenter.position
            : transform.position;

        if (agent != null)
            originalStoppingDistance = agent.stoppingDistance;
    }

    public void HearDistraction(Vector3 position, float duration)
    {
        if (isInvestigating && Vector3.Distance(transform.position, position) > 1f)
            return; // optional priority rule

        if (distractionRoutine != null)
            StopCoroutine(distractionRoutine);

        distractionRoutine = StartCoroutine(DistractionRoutine(position, duration));
    }

    IEnumerator DistractionRoutine(Vector3 position, float duration)
    {
        isInvestigating = true;

        Vector3 targetPos = GetValidNavMeshPosition(position);

        yield return MoveToPosition(targetPos, distractionStoppingDistance, maxDistractionTravelTime);

        yield return LookAround(targetPos);

        yield return WaitAtPoint(duration);

        Vector3 returnPos = GetValidNavMeshPosition(homePosition);

        yield return MoveToPosition(returnPos, returnStoppingDistance, maxReturnTravelTime);

        if (agent != null)
        {
            agent.stoppingDistance = originalStoppingDistance;
            agent.ResetPath();
        }

        if (shooterMain != null)
            shooterMain.GoToNextPatrolPoint();

        isInvestigating = false;
        distractionRoutine = null;
    }

    IEnumerator LookAround(Vector3 center)
    {
        float t = 0f;

        while (t < lookAroundTime)
        {
            t += Time.deltaTime;
            RotateTowards(center);
            yield return null;
        }
    }

    IEnumerator WaitAtPoint(float duration)
    {
        float t = 0f;

        while (t < Mathf.Max(duration, distractionWaitTime))
        {
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MoveToPosition(Vector3 position, float stoppingDistance, float maxTravelTime)
    {
        if (agent == null) yield break;

        Animator anim = GetComponentInChildren<Animator>();

        agent.isStopped = false;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(position);

        if (anim) anim.SetBool("isWalking", true);

        float timer = 0f;

        while (timer < maxTravelTime)
        {
            timer += Time.deltaTime;

            RotateTowards(position);

            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance + 0.2f)
                break;

            yield return null;
        }

        if (anim) anim.SetBool("isWalking", false);
    }

    Vector3 GetValidNavMeshPosition(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            return hit.position;

        return position;
    }

    void RotateTowards(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rot,
            rotationSpeed * Time.deltaTime
        );
    }
}