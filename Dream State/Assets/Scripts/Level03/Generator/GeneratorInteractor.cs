using UnityEngine;

public class GeneratorInteractor : MonoBehaviour
{
    public GeneratorSystem generatorSystem;
    public FuelInventoryConsumer fuelInventoryConsumer;
    public Transform player;
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public bool requireFuelItem = true;

    void Start()
    {
        if (generatorSystem == null)
            generatorSystem = GetComponent<GeneratorSystem>();

        if (fuelInventoryConsumer == null)
            fuelInventoryConsumer = FindFirstObjectByType<FuelInventoryConsumer>();

        if (player == null && PlayerSingle.instance != null)
            player = PlayerSingle.instance.transform;
    }

    void Update()
    {
        if (MenuController.isMenuOpen)
            return;

        if (player == null || generatorSystem == null)
            return;

        if ((player.position - transform.position).sqrMagnitude > interactionDistance * interactionDistance)
            return;

        if (!Input.GetKeyDown(interactKey))
            return;

        TryRefuelGenerator();
    }

    void TryRefuelGenerator()
    {
        if (requireFuelItem)
        {
            if (fuelInventoryConsumer == null)
                return;

            if (!fuelInventoryConsumer.ConsumeFuelCan())
                return;
        }

        generatorSystem.Refuel();
        SoundEffectManager.Play("Items");
    }
}