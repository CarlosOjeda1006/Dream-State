using UnityEngine;

public class FuelCanItem : Item
{
    public bool refillsGenerator = true;
    public Sprite pickupIcon;

    public override void PickUp()
    {
        SoundEffectManager.Play("Items");

        if (ItemPickUpUIController.Instance != null)
            ItemPickUpUIController.Instance.ShowItemPickup(Name, pickupIcon);
    }
}