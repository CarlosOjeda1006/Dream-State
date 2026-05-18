using UnityEngine;

public class KeyItem : Item
{
    public bool opensElevator = true;
    public Sprite pickupIcon;

    public override void PickUp()
    {
        SoundEffectManager.Play("Items");

        if (ItemPickUpUIController.Instance != null)
            ItemPickUpUIController.Instance.ShowItemPickup(Name, pickupIcon);
    }
}
