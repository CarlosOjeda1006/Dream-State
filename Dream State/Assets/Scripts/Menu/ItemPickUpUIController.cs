using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ItemPickUpUIController : MonoBehaviour
{

    public static ItemPickUpUIController Instance { get; private set; }
    //para que cualquier script pueda accederlo sin tener que poner referencia

    public GameObject popupPrefab;
    public int maxPopups = 3;
    public float popupDuration = 3f;

    private readonly Queue<GameObject> activePopups = new();
    private void Awake()
    {
        if(Instance == null) //si nunca hemos corrido este script
        {
            Instance = this;//referencia a este script
        }

        else
        {
            Debug.LogError("Multiple ItemPickUpUIManager instances detected");
            Destroy(gameObject);
        }
    }

    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = Instantiate(popupPrefab, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();

        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }

        activePopups.Enqueue(newPopup);
        if (activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());//se borra el más antiguo de la lista
        }

        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

        //fade out
        private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);
        if(popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed; //alpha es lo que lo hace trasnparente
            yield return null;
        }

        Destroy(popup);
    }
    
}
