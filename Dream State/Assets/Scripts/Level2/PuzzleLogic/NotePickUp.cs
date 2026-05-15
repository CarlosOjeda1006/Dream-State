using UnityEngine;

public class NotePickUp : MonoBehaviour, IInteractable
{
    public static bool uiActive;
    public GameObject note;
    public GameObject notesTab;
    public GameObject notesPages;

    public bool CanInteract => !alreadyRead;
    bool alreadyRead = false;

    public void Interact()
    {

        if (note != null)
        {
            note.SetActive(false);
            notesTab.SetActive(true);
            SoundEffectManager.Play("ItemPickUp");

            alreadyRead = true;
        }
    }
}