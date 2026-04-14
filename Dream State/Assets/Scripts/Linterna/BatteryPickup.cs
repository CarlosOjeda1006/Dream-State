using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float batteryAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        LinternaController flashlight = other.GetComponentInChildren<LinternaController>();

        if (flashlight != null && flashlight.currentBattery < flashlight.maxBattery)
        {
            SoundEffectManager.Play("BatteryLoad");
            flashlight.AddBattery(batteryAmount);
            gameObject.SetActive(false);
        }
    }
}