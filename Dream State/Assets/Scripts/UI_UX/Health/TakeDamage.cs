using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TakeDamage : MonoBehaviour
{
    public float maxIntensity = 0.8f;
    public float fadeSpeed = 2f;

    Volume _volume;
    Vignette _vignette;
    ChromaticAberration _chromatic;
    ColorAdjustments _color;

    Coroutine currentRoutine;

    void Awake()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _color);

        if (_color != null)
            _color.active = false;

        if (_volume == null)
        {
            Debug.LogError("No Volume component found");
            return;
        }

        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _chromatic);

        if (_vignette == null)
            Debug.LogError("No Vignette in Volume Profile");

        if (_chromatic == null)
            Debug.LogWarning("No Chromatic Aberration");

        if (_vignette != null)
            _vignette.active = false;

        if (_chromatic != null)
            _chromatic.active = false;
    }

    public void TriggerDamageEffect(float damageAmount = 1f)
    {
        if (_vignette == null) return;

        float intensity = Mathf.Clamp01(damageAmount) * maxIntensity;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DamageEffect(intensity));
    }

    private IEnumerator DamageEffect(float startIntensity)
    {
        _vignette.active = true;
        _vignette.color.value = new Color(1f, 0.2f, 0.2f);
        _vignette.intensity.value = startIntensity;

        if (_chromatic != null)
        {
            _chromatic.active = true;
            _chromatic.intensity.value = 1f;
        }
        if (_color != null)
        {
            _color.active = true;
            _color.postExposure.value = 0.5f;
        }
        yield return new WaitForSeconds(0.08f);

        float t = startIntensity;

        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;

            _vignette.intensity.value = t;

            if (_chromatic != null)
                _chromatic.intensity.value = t;
            if (_color != null)
                _color.postExposure.value = t;

            yield return null;
        }

        _vignette.intensity.value = 0;
        _vignette.active = false;

        if (_chromatic != null)
        {
            _chromatic.intensity.value = 0;
            _chromatic.active = false;
        }
        if (_color != null)
        {
            _color.postExposure.value = 0;
            _color.active = false;
        }
    }
}