using UnityEngine;

public class PlayerObjectThrower : MonoBehaviour
{
    public Camera playerCamera;
    public Transform holdPoint;
    public float pickupDistance = 3f;
    public float throwForce = 12f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.R;
    public LayerMask pickupLayers = ~0;

    ThrowableDistractionObject heldObject;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (heldObject == null)
        {
            if (Input.GetKeyDown(pickupKey))
                TryPickup();
        }
        else
        {
            UpdateHeldObject();

            if (Input.GetMouseButtonDown(0))
                ThrowHeldObject();

            if (Input.GetKeyDown(dropKey))
                DropHeldObject();
        }
    }

    void TryPickup()
    {
        if (playerCamera == null || holdPoint == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, pickupDistance, pickupLayers))
            return;

        ThrowableDistractionObject throwable = hit.collider.GetComponentInParent<ThrowableDistractionObject>();

        if (throwable == null)
            return;

        heldObject = throwable;
        heldObject.transform.parent = holdPoint;
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
        heldObject.SetHeld(true);
    }

    void UpdateHeldObject()
    {
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    void ThrowHeldObject()
    {
        ThrowableDistractionObject objectToThrow = heldObject;
        heldObject = null;
        objectToThrow.Throw(playerCamera.transform.forward, throwForce);
    }

    void DropHeldObject()
    {
        ThrowableDistractionObject objectToDrop = heldObject;
        heldObject = null;
        objectToDrop.Drop();
    }
}