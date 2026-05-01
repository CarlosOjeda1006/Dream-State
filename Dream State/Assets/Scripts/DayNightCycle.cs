using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float duration = 60f;

    private float timeOfDay;
    private float startTime = 12f;
    private float endTime = 22f;
    private float timeSpeed;
    private bool finished = false;

    [Header("Sun")]
    public Light sun;
    public Gradient lightColor;
    public AnimationCurve lightIntensity;

    [Header("Ambient")]
    public Gradient ambientColor;

    [Header("Time")]
    public AnimationCurve timeProgression;

    [Header("Skybox")]
    public Material skyboxMaterial;
    public AnimationCurve exposureCurve;

    [Header("Audio")]
    public AudioSource musicSource;
    public float fadeDuration = 5f;

    [Header("Audio Distortion")]
    public float minPitch = 0.6f;
    public AudioLowPassFilter lowPass;
    public float minCutoff = 500f;

    [Header("Enemy")]
    public GameObject eye;

    private bool isFading = false;

    private void Start()
    {
        timeOfDay = startTime;
        timeSpeed = (endTime - startTime) / duration;

        if (eye != null)
            eye.SetActive(false);
    }

    private void Update()
    {
        if (finished) return;

        float normalizedTime = timeOfDay / 24f;

        float speedMultiplier = (timeProgression != null && timeProgression.length > 0)
            ? timeProgression.Evaluate(normalizedTime)
            : 1f;

        timeOfDay += timeSpeed * speedMultiplier * Time.deltaTime;

        HandleAudioDistortion();


        if (timeOfDay >= endTime - 1f && !isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutMusic());
        }

        if (timeOfDay >= endTime - 0.1f)
        {
            sun.enabled = false;
        }

        if (timeOfDay >= endTime - 0.2f && !finished)
        {
            RenderSettings.ambientLight = Color.black;
        }

        if (timeOfDay >= endTime)
        {
            timeOfDay = endTime;
            finished = true;
        }

        UpdateLighting();
    }

    void UpdateLighting()
    {
        float normalizedTime = timeOfDay / 24f;

        if (sun.enabled)
        {
            float sunRotation = normalizedTime * 360f - 90f;
            sun.transform.rotation = Quaternion.Euler(sunRotation, 170f, 0);

            sun.intensity = lightIntensity.Evaluate(normalizedTime);
            sun.color = lightColor.Evaluate(normalizedTime);
        }

        if (timeOfDay < endTime - 0.2f)
        {
            RenderSettings.ambientLight = ambientColor.Evaluate(normalizedTime);
        }

        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", exposureCurve.Evaluate(normalizedTime));
        }
    }

    void HandleAudioDistortion()
    {
        float distortionStart = endTime - 3f;

        if (timeOfDay >= distortionStart && musicSource != null)
        {
            float t = Mathf.InverseLerp(distortionStart, endTime, timeOfDay);

            musicSource.pitch = Mathf.Lerp(1f, minPitch, t);

            if (lowPass != null)
            {
                lowPass.cutoffFrequency = Mathf.Lerp(22000f, minCutoff, t);
            }

            float flicker = Mathf.PerlinNoise(Time.time * 2f, 0f);
            float fluctuation = Mathf.Lerp(0.85f, 1.15f, flicker);
            musicSource.volume *= fluctuation;
        }
    }

    IEnumerator FadeOutMusic()
    {
        if (musicSource == null) yield break;

        float startVolume = musicSource.volume;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);

            yield return null;
        }


        musicSource.pitch = 0.3f;

        musicSource.Stop();

        StartCoroutine(ActivateEyeDelayed(2f));
    }
    IEnumerator ActivateEyeDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (eye != null)
            eye.SetActive(true);
    }
}