using UnityEngine;

public class GeneratorSystem : MonoBehaviour
{
    public float maxFuelTime = 60f;
    public float currentFuelTime = 60f;
    public string generatorLightTag = "GeneratorLight";
    public bool startWithFuel = true;
    public bool refreshLightsWhenChanged = true;

    Light[] controlledLights;
    bool lightsAreOn;

    public float FuelNormalized => maxFuelTime <= 0f ? 0f : Mathf.Clamp01(currentFuelTime / maxFuelTime);
    public bool HasFuel => currentFuelTime > 0f;

    [Header("Audio")]
    public GeneratorAudio generatorAudio;

    void Awake()
    {
        RefreshLights();
        currentFuelTime = startWithFuel ? maxFuelTime : 0f;
        SetLights(HasFuel);
        generatorAudio.PlayOn();
    }

    void Update()
    {
        if (currentFuelTime <= 0f)
            return;

        currentFuelTime -= Time.deltaTime;

        if (currentFuelTime <= 0f)
        {
            currentFuelTime = 0f;
            SetLights(false);
            generatorAudio.PlayOff();
        }
    }

    public void Refuel()
    {
        currentFuelTime = maxFuelTime;
        SetLights(true);
        generatorAudio.PlayOn();
    }

    public void RefreshLights()
    {
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int count = 0;

        for (int i = 0; i < allLights.Length; i++)
        {
            if (allLights[i] != null && allLights[i].CompareTag(generatorLightTag))
                count++;
        }

        controlledLights = new Light[count];
        int index = 0;

        for (int i = 0; i < allLights.Length; i++)
        {
            if (allLights[i] != null && allLights[i].CompareTag(generatorLightTag))
            {
                controlledLights[index] = allLights[i];
                index++;
            }
        }
    }

    void SetLights(bool active)
    {
        if (refreshLightsWhenChanged)
            RefreshLights();

        lightsAreOn = active;

        if (controlledLights == null)
            return;

        for (int i = 0; i < controlledLights.Length; i++)
        {
            if (controlledLights[i] != null)
                controlledLights[i].gameObject.SetActive(active);
        }
    }
}