using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public Camera cam;
    public Transform holdPoint;
    public float interactDistance = 3f;
    public float throwForce = 10f;

    ThrowableDistractionObject currentObject;
    ThrowableDistractionObject heldObject;

    IInteractable interactable;

    void Update()
    {
        DetectObject();

        // PICKUP / INTERACT
        if (Input.GetKeyDown(KeyCode.E)) // Aqui agregar el KeyCode.GetJoystickButton2
        {
            // interactables
            if (interactable != null && heldObject == null)
            {
                interactable.Interact();
            }

            // pickups
            else if (currentObject != null && heldObject == null)
            {
                PickUp();
            }
        }

        // THROW
        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            DiffManager.Instance.bottlesThrown++;
            Throw();
        }

        // DROP
        if (Input.GetKeyDown(KeyCode.R) && heldObject != null)
        {
            Drop();
        }
    }

    void DetectObject()
    {
        if (heldObject != null)
        {
            UIController.uiActive = false;
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // INTERACTABLE
            interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable.CanInteract)
            {
                currentObject = null;

                UIController.actionText = "Interact";
                UIController.uiActive = true;
                return;
            }

            // PICKUP
            currentObject =
                hit.collider.GetComponentInParent<ThrowableDistractionObject>();

            if (currentObject != null)
            {
                interactable = null;

                UIController.actionText = "Pick up";
                UIController.uiActive = true;
                return;
            }
        }

        currentObject = null;
        interactable = null;

        UIController.uiActive = false;
        UIController.actionText = "";
    }

    void PickUp()
    {
        heldObject = currentObject;

        heldObject.SetHeld(true);

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        currentObject = null;
    }

    void Throw()
    {
        ThrowableDistractionObject obj = heldObject;

        heldObject = null;
        SoundEffectManager.Play("ThrowObject");

        obj.transform.SetParent(null);
        obj.Throw(cam.transform.forward, throwForce);
    }

    void Drop()
    {
        ThrowableDistractionObject obj = heldObject;

        heldObject = null;

        obj.transform.SetParent(null);
        obj.Drop();
    }
}