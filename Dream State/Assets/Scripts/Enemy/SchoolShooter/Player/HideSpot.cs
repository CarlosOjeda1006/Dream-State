using UnityEngine;

public class HideSpot : MonoBehaviour
{
    [Header("Positions")]
    public Transform hidePoint;
    public Transform exitPoint;

    [Header("UI")]
    public GameObject interactUI;

    bool playerInsideTrigger;
    bool isOccupied;

    PlayerHideController currentPlayer;

    void Start()
    {
        if (interactUI != null)
            interactUI.SetActive(false);


    }

    public void TryInteract()
    {
        if (currentPlayer == null)
            return;

        if (currentPlayer.IsHidden)
        {
            currentPlayer.ExitHideSpot();
        }
        else
        {
            currentPlayer.EnterHideSpot(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerHideController player =
            other.GetComponentInParent<PlayerHideController>();

        if (player == null)
            return;

        if (isOccupied)
            return;

        playerInsideTrigger = true;

        currentPlayer = player;

        player.SetCurrentHideSpot(this);

        if (interactUI != null)
            interactUI.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerHideController player = other.GetComponentInParent<PlayerHideController>();

        if (player == null)
            return;

        playerInsideTrigger = false;

        currentPlayer = null;

        if (interactUI != null)
            interactUI.SetActive(false);
    }

    public void SetOccupied(bool value)
    {
        isOccupied = value;

        if (interactUI != null)
            interactUI.SetActive(false);
    }

}