using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public Camera cam;
    public Transform holdPoint;
    public float interactDistance = 3f;
    public float throwForce = 10f;

    ThrowableDistractionObject currentObject;
    IInteractable interactable;

    void Update()
    {
        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            // interactables
            if (interactable != null)
            {
                interactable.Interact();
            }
            // pickups
            else if (currentObject != null)
            {
                PickUp();
            }
        }

        if (Input.GetMouseButtonDown(0) && currentObject != null)
        {
            Throw();
        }
    }

    void DetectObject()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // check interactable
            interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable.CanInteract)
            {
                currentObject = null;

                UIController.actionText = "Interact";
                UIController.uiActive = true;
                return;
            }

            // check pickable
            currentObject = hit.collider.GetComponentInParent<ThrowableDistractionObject>();

            if (currentObject != null)
            {
                interactable = null;

                UIController.actionText = "Pick up";
                UIController.uiActive = true;
                return;
            }
        }

        // nothing hit
        currentObject = null;
        interactable = null;

        UIController.uiActive = false;
        UIController.actionText = "";
    }

    void PickUp()
    {
        currentObject.SetHeld(true);

        currentObject.transform.SetParent(holdPoint);
        currentObject.transform.localPosition = Vector3.zero;
        currentObject.transform.localRotation = Quaternion.identity;
    }

    void Throw()
    {
        currentObject.transform.SetParent(null);
        currentObject.Throw(cam.transform.forward, throwForce);
        currentObject = null;
    }
}