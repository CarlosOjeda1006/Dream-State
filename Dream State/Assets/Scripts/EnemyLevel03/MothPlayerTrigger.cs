using UnityEngine;

public class MothPlayerTrigger : MonoBehaviour
{
    public Polilla moth;

    void OnTriggerEnter(Collider other)
    {
        PlayerSingle player =
            other.GetComponent<PlayerSingle>();

        if (player == null)
            return;

        if (moth.lightTracker.CurrentLight == null)
            return;

        moth.SetPlayerTarget(player.transform);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerSingle player =
            other.GetComponent<PlayerSingle>();

        if (player == null)
            return;

        moth.ClearPlayerTarget();
    }
}