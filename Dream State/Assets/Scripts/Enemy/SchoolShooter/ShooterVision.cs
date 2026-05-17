using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class ShooterVision : MonoBehaviour
{
    public Action OnPlayerDetected;

    [Header("References")]
    public Transform eyePoint;
    

    [Header("Vision")]
    public float visionRange = 18f;
    [Range(0f, 180f)]
    public float visionAngle = 70f;

    [Header("Performance")]
    public float refreshRate = 0.15f;

    float visionRangeSqr;

    bool hasDetected;
    public Transform target;

    WaitForSeconds wait;


    void Start()
    {
        if (target == null)
        {
            target = PlayerSingle.instance.transform;
        }

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
        {
            Debug.Log("NO TARGET");
            return;
        }

        Vector3 targetPoint = target.position + Vector3.up * 1.2f;

        Vector3 dir = targetPoint - eyePoint.position;

        float distSqr = dir.sqrMagnitude;

        // RANGE CHECK
        if (distSqr > visionRangeSqr)
        {
            Debug.Log("OUT OF RANGE");
            return;
        }

        // ANGLE CHECK
        float dot = Vector3.Dot(
            eyePoint.forward,
            dir.normalized
        );

        float minDot = Mathf.Cos(
            visionAngle * 0.5f * Mathf.Deg2Rad
        );

        if (dot < minDot)
        {
            Debug.Log("OUT OF VISION ANGLE");
            return;
        }

        // LINE OF SIGHT CHECK
        float dist = Mathf.Sqrt(distSqr);

        RaycastHit hit;

        Debug.Log("LLEGO AL RAYCAST");
        Debug.DrawRay(
        eyePoint.position,
        dir.normalized * dist,
        Color.red,
        0.2f
        );

        if (Physics.Raycast(
            eyePoint.position,
            dir.normalized,
            out hit,
            dist))
        {
            Debug.Log("RAY HIT: " + hit.transform.name);


            if (!hit.transform.IsChildOf(target))
            {
                Debug.Log("VISION BLOCKED");
                return;
            }
        }
        else
        {
            Debug.Log("RAY HIT NOTHING");
            return;
        }

        Debug.Log("PLAYER DETECTED");

        hasDetected = true;

        OnPlayerDetected?.Invoke();
    }

    public void ResetDetection()
    {
        hasDetected = false;
    }
}