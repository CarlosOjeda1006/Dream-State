using UnityEngine;

public class PlayerHideController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public FirstPersonController movement;

    bool isHidden;

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
        if (!isHidden)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitHideSpot();
        }
    }

    public void EnterHideSpot(HideSpot spot)
    {
        if (isHidden)
            return;

        isHidden = true;

        currentSpot = spot;

        currentSpot.SetOccupied(true);

        characterController.enabled = false;

        transform.position =
            spot.hidePoint.position;

        transform.rotation =
            spot.hidePoint.rotation;

        characterController.enabled = true;

        movement.enabled = false;
    }

    public void ExitHideSpot()
    {
        if (!isHidden)
            return;

        isHidden = false;

        characterController.enabled = false;

        transform.position =
            currentSpot.exitPoint.position;

        characterController.enabled = true;

        movement.enabled = true;

        currentSpot.SetOccupied(false);

        currentSpot = null;
    }

    public bool IsHidden()
    {
        return isHidden;
    }
}