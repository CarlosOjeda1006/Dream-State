/*using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
*/
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject itemUIPrefab;
    public GameObject photosTab;
    public GameObject itemPhoto;
    public GameObject floorCamera;
    public GameObject handCamera;
    public bool isPhotoItem;
    public string Name;


    public virtual void PickUp()
    {
        SoundEffectManager.Play("Items");

        Sprite itemIcon = GetComponent<Image>().sprite;

        if (ItemPickUpUIController.Instance != null)
        {
            ItemPickUpUIController.Instance.ShowItemPickup(Name, itemIcon);
        }

        if (isPhotoItem)
        {
            photosTab.SetActive(true);
            itemPhoto.SetActive(true);

            InstructionsUI.Instance.ShowInstruction(
                "New Photo Added [Press LB to view]."
            );

            SoundEffectManager.Play("Clue");
        }
        if(tag == "camera")
        {
            floorCamera.SetActive(false);
            handCamera.SetActive(true);
        }

    }

    public void Drop(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
}
