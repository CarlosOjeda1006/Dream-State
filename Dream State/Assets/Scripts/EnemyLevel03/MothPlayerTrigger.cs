using UnityEngine;

public class MothPlayerTrigger : MonoBehaviour
{
    public Polilla moth;

    Transform player;

    void Start()
    {
        player = PlayerSingle.instance.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform != player)
            return;

        if (moth.lightTracker.CurrentLight == null)
            return;

        moth.SetPlayerTarget(player);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform != player)
            return;

        moth.ClearPlayerTarget();
    }
}