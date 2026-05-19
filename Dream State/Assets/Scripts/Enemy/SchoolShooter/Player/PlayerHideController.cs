using UnityEngine;

public class PlayerHideController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public FirstPersonController movement;

    public bool IsHidden { get; private set; }

    HideSpot currentSpot;

    void Start()
    {
        if (characterController == null)
            characterController =
                GetComponent<CharacterController>();

        if (movement == null)
            movement =
                GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentSpot != null)
            {
                currentSpot.TryInteract();
            }
        }
    }

    public void EnterHideSpot(HideSpot spot)
    {
        if (IsHidden)
            return;

        IsHidden = true;

        currentSpot = spot;

        currentSpot.SetOccupied(true);

        characterController.enabled = false;

        transform.position =
            spot.hidePoint.position;

        transform.rotation =
            spot.hidePoint.rotation;

        characterController.enabled = true;

        movement.canMove = false;
    }

    public void ExitHideSpot()
    {
        if (!IsHidden)
            return;

        IsHidden = false;

        characterController.enabled = false;

        transform.position =
            currentSpot.exitPoint.position;

        characterController.enabled = true;

        movement.canMove = true;

        currentSpot.SetOccupied(false);

        currentSpot = null;
    }

    public void SetCurrentHideSpot(HideSpot spot)
    {
        currentSpot = spot;
    }
}