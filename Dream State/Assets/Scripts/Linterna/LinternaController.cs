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

        int activeSegments = Mathf.FloorToInt(percent * segments.Length);

        if (percent > 0f && activeSegments == 0)
            activeSegments = 1; 

        Color color = Color.white;

        if (percent < 0.2f)
            color = new Color(0.6f, 0f, 0f);
        else if (percent < 0.5f)
            color = new Color(0.7f, 0.6f, 0.2f);

        foreach (var seg in segments)
            seg.color = color;

        // flicker
        if (percent < 0.1f)
            flashlight.intensity = Random.Range(originalIntensity * 0.4f, originalIntensity);
        else
            flashlight.intensity = originalIntensity;

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
}