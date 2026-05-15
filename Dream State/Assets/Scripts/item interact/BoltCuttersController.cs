using UnityEngine;

public class BoltCuttersController : MonoBehaviour, IInteractable
{
    public static bool hasBoltCutters = false;
    bool alreadyPickedUp = false;
    public bool CanInteract => !alreadyPickedUp;

    public GameObject Visual;

    public void Interact()
    {
        if (hasBoltCutters) return;

        hasBoltCutters = true;
        alreadyPickedUp=true;

        if (Visual != null)
            Visual.SetActive(false);

        SoundEffectManager.Play("ItemPickUp");
    }
}