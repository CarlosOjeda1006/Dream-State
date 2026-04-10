using UnityEngine;

public class LinternaController : MonoBehaviour
{
    public Light flashlight;

    public KeyCode toggleKey = KeyCode.Q;

    [Header("Bateria")]
    public float maxBattery = 50f;
    public float drainRate = 1f;

    private float currentBattery;
    private bool isOn = false;

    void Start()
    {
        currentBattery = maxBattery;
        flashlight.enabled = false;
    }

    void Update()
    {
        HandleToggle();
        HandleBattery();
    }

    void HandleToggle()
    {
        if (Input.GetKeyDown(toggleKey) && currentBattery > 0f)
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
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
        }
    }
}