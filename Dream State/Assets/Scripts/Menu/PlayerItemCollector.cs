using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{

    private InventoryController inventoryController;


    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
    }

    private void OnTriggerEnter(Collider collision)
    {

        Item item = collision.GetComponentInParent<Item>();

        if (item != null)
        {

            bool itemAdded = inventoryController.AddItem(item.itemUIPrefab, item);

            if (itemAdded)
            {
                item.PickUp();
                item.gameObject.SetActive(false);
            }
        }
    }

}
