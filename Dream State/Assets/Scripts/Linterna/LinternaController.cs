using UnityEngine;
using UnityEngine.UI;

public class LinternaController : MonoBehaviour
{
    public Light flashlight;
    public GameObject flashlightModel;

    public KeyCode toggleKey = KeyCode.Q;

    [Header("Bateria")]
    public float maxBattery = 50f;
    public float drainRate = 1f;

    public float currentBattery;
    public bool isOn = false;

    [Header("UI")]
    public Image[] segments;
    public GameObject batteryUI;
    float flickerSoundTimer = 0f;

    float originalIntensity;

    void Start()
    {
        currentBattery = maxBattery;
        flashlight.enabled = false;
        originalIntensity = flashlight.intensity;
    }

    void Update()
    {
        HandleToggle();
        HandleBattery();
        UpdateUI();
    }

    void UpdateUI()
    {
        float percent = currentBattery / maxBattery;

        int activeSegments;

        if (percent > 0.66f)
            activeSegments = 3;
        else if (percent > 0.33f)
            activeSegments = 2;
        else if (percent > 0f)
        {
            activeSegments = 1;
        }
        else
            activeSegments = 0;

        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].enabled = i < activeSegments;
        }


        // flicker
        if (percent < 0.1f && isOn)
        {
            flickerSoundTimer -= Time.deltaTime;

            if (flickerSoundTimer <= 0f)
            {
                SoundEffectManager.Play("Flicker");
                flickerSoundTimer = Random.Range(0.1f, 0.25f);
            }

            flashlight.intensity = Random.Range(originalIntensity * 0.4f, originalIntensity);
        }
        else
        {
            flashlight.intensity = originalIntensity;
        }

        batteryUI.SetActive(isOn);
    }

    void HandleToggle()
    {
        if (Input.GetKeyDown(toggleKey) && currentBattery > 0f)
        {
            isOn = !isOn;

            flashlight.enabled = isOn;
            flashlightModel.SetActive(isOn);

            if (isOn)
                SoundEffectManager.Play("FlashlightOn");
        }
    }

    void HandleBattery()
    {
        if (!isOn) return;

        currentBattery -= drainRate * Time.deltaTime;

        if (currentBattery <= 0f)
        {
            currentBattery = 0f;
            isOn = false;
            flashlight.enabled = false;
            flashlightModel.SetActive(false);
        }
    }

    public void AddBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }
}