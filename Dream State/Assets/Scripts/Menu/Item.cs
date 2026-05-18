using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject itemUIPrefab;
    public GameObject photosTab;
    public GameObject itemPhoto;
    public bool isPhotoItem;
    public GameObject instructionsBox;
    public string Name;


    public virtual void PickUp()
    {
        SoundEffectManager.Play("Items");
        Sprite itemIcon = GetComponent<Image>().sprite;
        if(ItemPickUpUIController.Instance != null )
        {
            ItemPickUpUIController.Instance.ShowItemPickup(Name, itemIcon);
        }

        if(isPhotoItem)
        {
            photosTab.SetActive(true);
            itemPhoto.SetActive(true);
            StartCoroutine(PickupSequence());
        }

    }

    public void Drop(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
    IEnumerator PickupSequence()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMP_Text>().text = "New Photo Added [Press TAB to view].";

        SoundEffectManager.Play("Clue");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);
    }
}
