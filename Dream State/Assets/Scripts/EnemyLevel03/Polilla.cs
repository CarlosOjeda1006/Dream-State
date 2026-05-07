using UnityEngine;
using UnityEngine.AI;

public class Polilla : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 6f;

    [Header("Light Detection")]
    public float detectionRadius = 15f;
    public float refreshRate = 0.5f;
    public float intensityMultiplier = 2f;

    LuzPoililla currentTarget;

    float nextRefreshTime;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Time.time >= nextRefreshTime)
        {
            nextRefreshTime = Time.time + refreshRate;
            FindBestLight();
        }

        if (currentTarget != null)
        {
            Vector3 targetPos = currentTarget.transform.position;

            agent.isStopped = false;
            agent.SetDestination(targetPos);

            RotateTowards(targetPos);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void FindBestLight()
    {
        float bestScore = -Mathf.Infinity;
        LuzPoililla bestLight = null;

        foreach (LuzPoililla lightObj in LuzPoililla.AllLights)
        {
            if (lightObj == null || lightObj.lightSource == null)
                continue;

            if (!lightObj.lightSource.enabled)
                continue;

            float distance = Vector3.Distance(transform.position, lightObj.transform.position);

            if (distance > detectionRadius)
                continue;

            float score = (lightObj.lightSource.intensity * intensityMultiplier) - distance;

            if (score > bestScore)
            {
                bestScore = score;
                bestLight = lightObj;
            }
        }

        currentTarget = bestLight;
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
}
