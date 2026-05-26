using UnityEngine;

public class MothLightTracker : MonoBehaviour
{
    public float detectionRadius = 15f;
    public float intensityMultiplier = 2f;
    public float refreshRate = 0.5f;

    public LuzPoililla ignoredLight;

    float ignoreUntilTime;

    public LuzPoililla CurrentLight { get; private set; }

    float nextRefresh;

    void Update()
    {
        if (ignoredLight != null && Time.time >= ignoreUntilTime)
        {
            ignoredLight = null;
        }

        if (Time.time >= nextRefresh)
        {
            nextRefresh = Time.time + refreshRate;

            FindBestLight();
        }
    }

    void FindBestLight()
    {
        float bestScore = -Mathf.Infinity;

        LuzPoililla best = null;

        foreach (LuzPoililla lightObj in LuzPoililla.AllLights)
        {
            if (lightObj == ignoredLight)
                continue;

            if (lightObj == null || lightObj.lightSource == null || !lightObj.lightSource.enabled)
                continue;

            float dist =
                Vector3.Distance(
                    transform.position,
                    lightObj.transform.position
                );

            if (dist > detectionRadius)
                continue;

            float score =
                (lightObj.lightSource.intensity * intensityMultiplier)
                - dist;

            if (score > bestScore)
            {
                bestScore = score;
                best = lightObj;
            }
        }

        CurrentLight = best;
    }
    public void IgnoreLightTemporarily(LuzPoililla light, float duration)
    {
        ignoredLight = light;

        ignoreUntilTime = Time.time + duration;
    }
}