using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject itemUIPrefab;
    public string Name;


    public virtual void PickUp()
    {
        SoundEffectManager.Play("Items");
        Sprite itemIcon = GetComponent<Image>().sprite;
        if(ItemPickUpUIController.Instance != null )
        {
            ItemPickUpUIController.Instance.ShowItemPickup(Name, itemIcon);
        }

    }

    public void Drop(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
}
