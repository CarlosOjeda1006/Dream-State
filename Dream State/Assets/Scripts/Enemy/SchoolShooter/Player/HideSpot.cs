using UnityEngine;

public class HideSpot : MonoBehaviour
{
    [Header("Positions")]
    public Transform hidePoint;
    public Transform exitPoint;

    bool playerInsideTrigger;
    bool isOccupied;

    PlayerHideController currentPlayer;


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

    }

    void OnTriggerExit(Collider other)
    {
        PlayerHideController player = other.GetComponentInParent<PlayerHideController>();

        if (player == null)
            return;

        playerInsideTrigger = false;

        currentPlayer = null;

    }

    public void SetOccupied(bool value)
    {
        isOccupied = value;

    }

}