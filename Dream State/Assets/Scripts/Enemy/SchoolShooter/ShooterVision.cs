using System;
using System.Collections;
using UnityEngine;

public class ShooterVision : MonoBehaviour
{
    public Action OnPlayerDetected;

    [Header("References")]
    public Transform eyePoint;
    public Transform target;

    [Header("Vision")]
    public float visionRange = 18f;
    [Range(0f, 180f)]
    public float visionAngle = 70f;

    [Header("Performance")]
    public float refreshRate = 0.15f;

    [Header("Layers")]
    public LayerMask obstacleMask;

    float visionRangeSqr;

    bool hasDetected;

    WaitForSeconds wait;

    void Start()
    {
        visionRangeSqr = visionRange * visionRange;

        wait = new WaitForSeconds(refreshRate);

        StartCoroutine(VisionRoutine());
    }

    IEnumerator VisionRoutine()
    {
        while (true)
        {
            if (!hasDetected)
            {
                DetectPlayer();
            }

            yield return wait;
        }
    }

    void DetectPlayer()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - eyePoint.position;

        float distSqr = dir.sqrMagnitude;

        // RANGE CHECK
        if (distSqr > visionRangeSqr)
            return;

        // ANGLE CHECK
        float dot = Vector3.Dot(
            eyePoint.forward,
            dir.normalized
        );

        float minDot = Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);

        if (dot < minDot)
            return;

        // WALL CHECK
        float dist = Mathf.Sqrt(distSqr);

        if (Physics.Raycast(
            eyePoint.position,
            dir.normalized,
            dist,
            obstacleMask))
        {
            return;
        }

        hasDetected = true;

        OnPlayerDetected?.Invoke();
    }

    public void ResetDetection()
    {
        hasDetected = false;
    }
}