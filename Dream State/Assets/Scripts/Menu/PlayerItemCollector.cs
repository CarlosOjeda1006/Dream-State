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
        Debug.Log("Something entered: " + collision.name);

        Item item = collision.GetComponentInParent<Item>();

        if (item != null)
        {
            Debug.Log("Item detected correctamente");

            bool itemAdded = inventoryController.AddItem(item.itemUIPrefab, item);

            if (itemAdded)
            {
                item.PickUp();
                item.gameObject.SetActive(false);
            }
        }
    }

}
