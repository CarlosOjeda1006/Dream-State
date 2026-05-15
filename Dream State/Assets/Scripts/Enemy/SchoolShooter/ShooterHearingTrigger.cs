using System;
using UnityEngine;

public class ShooterHearingTrigger : MonoBehaviour
{
    public Action OnPlayerHeard;

    bool hasHeardPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (hasHeardPlayer)
            return;

        if (other.GetComponent<PlayerSingle>())
        {
            hasHeardPlayer = true;

            OnPlayerHeard?.Invoke();
        }
    }

    public void ResetHearing()
    {
        hasHeardPlayer = false;
    }
}