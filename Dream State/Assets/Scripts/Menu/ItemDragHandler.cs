using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 3f;
    public float maxDropDistance = 4f;

    public Item linkedItem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //guardar el OG Parent
        transform.SetParent(transform.root); //above other canvas
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semitransparente cuando el drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //sigue el mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        canvasGroup.blocksRaycasts = true; //podemos click otra vez
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();//slot donde lo dropeamos
        if(dropSlot == null)//si no hay slot en donde lo dropeamos
        {
            GameObject dropItem = eventData.pointerEnter;
            if(dropItem != null )
            {
                dropSlot = dropItem.GetComponent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            SoundEffectManager.Play("Inventory_switch");
            if (dropSlot.currentItem != null)
            {
                //si el slot tiene un item
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            //move item into drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //if where we are dropping is not the inventory we drop the item
            if (!IsWithinInventory(eventData.position))
            {
                //drop item
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent); //regresa al original
            }




        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //lo centra en el slot

    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);//is the mouse inside this transform
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        //find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing Player tag");
            return;
        }
        //random drop position
        Vector2 offset2D = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);

        Vector3 dropPosition = playerTransform.position + new Vector3(offset2D.x, 1f, offset2D.y);

        //make object visible drop item
        if (linkedItem != null)
        {
            SoundEffectManager.Play("DropItem");
            linkedItem.Drop(dropPosition);
        }


        //destroy the UI one
        Destroy(gameObject);
    }

}
