using UnityEngine;

public class FuelInventoryConsumer : MonoBehaviour
{
    public bool ConsumeFuelCan()
    {
        Slot[] slots = Object.FindObjectsByType<Slot>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null || slots[i].currentItem == null)
                continue;

            ItemDragHandler dragHandler = slots[i].currentItem.GetComponent<ItemDragHandler>();

            if (dragHandler == null || dragHandler.linkedItem == null)
                continue;

            FuelCanItem fuelCan = dragHandler.linkedItem.GetComponent<FuelCanItem>();

            if (fuelCan == null || !fuelCan.refillsGenerator)
                continue;

            GameObject uiItem = slots[i].currentItem;
            slots[i].currentItem = null;

            if (fuelCan.gameObject != null)
                Destroy(fuelCan.gameObject);

            Destroy(uiItem);
            return true;
        }

        return false;
    }
}