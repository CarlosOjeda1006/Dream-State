using UnityEngine;

public class InteractHide : MonoBehaviour, IInteractable
{
    public bool CanInteract => true;
    public void Interact()
    {
        SoundEffectManager.Play("OpenDoor");
    }
}
