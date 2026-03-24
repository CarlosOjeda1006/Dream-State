using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab, Item itemData)
    {
        //look for empty slot
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Debug.Log("Intentando agregar item");
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                Debug.Log("Item agregado");
                GameObject newItem = Instantiate(itemPrefab, slot.transform);

                ItemDragHandler drag = newItem.GetComponent<ItemDragHandler>();
                drag.linkedItem = itemData;
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }


}
