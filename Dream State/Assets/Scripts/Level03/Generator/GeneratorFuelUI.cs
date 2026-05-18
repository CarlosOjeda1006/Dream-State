using UnityEngine;
using UnityEngine.UI;

public class GeneratorFuelUI : MonoBehaviour
{
    public GeneratorSystem generatorSystem;
    public CanvasGroup generatorCanvasGroup;
    public Slider fuelSlider;
    public Text fuelText;
    public string textPrefix = "Generator: ";
    public bool hideWhenInventoryIsOpen = true;
    public bool hideWhenPauseIsOpen = true;

    void Start()
    {
        if (generatorSystem == null)
            generatorSystem = FindFirstObjectByType<GeneratorSystem>();

        if (generatorCanvasGroup == null)
            generatorCanvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (generatorSystem == null)
            return;

        bool inventoryOpen = hideWhenInventoryIsOpen && MenuController.isMenuOpen;
        bool pauseOpen = hideWhenPauseIsOpen && PauseMenu_Script.isPaused;
        bool shouldShow = !inventoryOpen && !pauseOpen;

        SetVisible(shouldShow);

        if (!shouldShow)
            return;

        float value = generatorSystem.FuelNormalized;

        if (fuelSlider != null)
            fuelSlider.value = value;

        if (fuelText != null)
            fuelText.text = textPrefix + Mathf.CeilToInt(value * 100f) + "%";
    }

    void SetVisible(bool visible)
    {
        if (generatorCanvasGroup == null)
            return;

        generatorCanvasGroup.alpha = visible ? 1f : 0f;
        generatorCanvasGroup.interactable = visible;
        generatorCanvasGroup.blocksRaycasts = visible;
    }
}