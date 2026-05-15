using UnityEngine;

public class PlayerObjectThrower : MonoBehaviour
{
    public Camera playerCamera;
    public Transform holdPoint;
    public float pickupDistance = 3f;
    public float throwForce = 12f;

    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.R;

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
            if (Input.GetMouseButtonDown(0))
                ThrowHeldObject();

            if (Input.GetKeyDown(dropKey))
                DropHeldObject();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            ThrowableDistractionObject throwable =
                hit.collider.GetComponentInParent<ThrowableDistractionObject>();

            if (throwable == null) return;

            heldObject = throwable;

            heldObject.SetHeld(true);

            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
        }
    }

    void ThrowHeldObject()
    {
        if (heldObject == null) return;

        ThrowableDistractionObject obj = heldObject;
        heldObject = null;

        obj.transform.SetParent(null);
        obj.Throw(playerCamera.transform.forward, throwForce);
    }

    void DropHeldObject()
    {
        if (heldObject == null) return;

        ThrowableDistractionObject obj = heldObject;
        heldObject = null;

        obj.transform.SetParent(null);
        obj.Drop();
    }
}