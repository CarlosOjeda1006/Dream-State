using UnityEngine;

public class GeneratorInteractor : MonoBehaviour, IInteractable
{
    public GeneratorSystem generatorSystem;
    public InventoryController inventory;

    public bool CanInteract => true;

    public void Interact()
    {
        // Check if player has fuel
        if (!inventory.HasItem("FuelCan"))
        {
            Debug.Log("Need fuel can");

            SoundEffectManager.Play("LockedDoor");

            return;
        }

        // Remove fuel from inventory
        inventory.RemoveItem("FuelCan");

        // Refuel generator
        generatorSystem.Refuel();

        SoundEffectManager.Play("Items");

        Debug.Log("Generator refueled");
    }

}
