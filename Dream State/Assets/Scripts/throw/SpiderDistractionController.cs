using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderDistractionController : MonoBehaviour
{
    public SpiderMain spiderMain;
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

    void Awake()
    {
        if (spiderMain == null)
            spiderMain = GetComponent<SpiderMain>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (spiderMain != null && spiderMain.territoryCenter != null)
            homePosition = spiderMain.territoryCenter.position;
        else
            homePosition = transform.position;

        if (agent != null)
            originalStoppingDistance = agent.stoppingDistance;
    }

    public void HearDistraction(Vector3 position, float duration)
    {
        if (distractionRoutine != null)
            StopCoroutine(distractionRoutine);

        distractionRoutine = StartCoroutine(DistractionRoutine(position, duration));
    }

    IEnumerator DistractionRoutine(Vector3 position, float duration)
    {
        if (spiderMain != null)
            spiderMain.enabled = false;

        Vector3 distractionPosition = GetValidNavMeshPosition(position);

        yield return MoveToPosition(distractionPosition, distractionStoppingDistance, maxDistractionTravelTime);

        float lookTimer = 0f;

        while (lookTimer < lookAroundTime)
        {
            lookTimer += Time.deltaTime;
            RotateTowards(distractionPosition);
            yield return null;
        }

        float waitTimer = 0f;
        float finalWaitTime = Mathf.Max(duration, distractionWaitTime);

        while (waitTimer < finalWaitTime)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        Vector3 returnPosition = GetValidNavMeshPosition(homePosition);

        yield return MoveToPosition(returnPosition, returnStoppingDistance, maxReturnTravelTime);

        if (agent != null)
        {
            agent.isStopped = true;
            agent.stoppingDistance = originalStoppingDistance;
            agent.ResetPath();
        }

        if (spiderMain != null)
            spiderMain.enabled = true;

        distractionRoutine = null;
    }

    IEnumerator MoveToPosition(Vector3 position, float stoppingDistance, float maxTravelTime)
    {
        if (agent == null)
            yield break;

        agent.isStopped = false;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(position);

        float timer = 0f;

        while (timer < maxTravelTime)
        {
            timer += Time.deltaTime;
            RotateTowards(position);

            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance + 0.2f)
                break;

            yield return null;
        }
    }

    Vector3 GetValidNavMeshPosition(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            return hit.position;

        return position;
    }

    void RotateTowards(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}