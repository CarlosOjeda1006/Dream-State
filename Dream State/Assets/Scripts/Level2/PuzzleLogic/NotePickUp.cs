using System.Collections;
using TMPro;
using UnityEngine;

public class NotePickUp : MonoBehaviour, IInteractable
{
    public static bool uiActive;
    public GameObject note;
    public GameObject notesTab;
    public GameObject notesPages;
    public GameObject notesSolution;
    public GameObject instructionsBox;

    public bool CanInteract => !alreadyRead;
    bool alreadyRead = false;

    public void Interact()
    {

        if (note != null)
        {
            notesTab.SetActive(true);
            notesSolution.SetActive(true);
            SoundEffectManager.Play("ItemPickUp");

            alreadyRead = true;

            StartCoroutine(PickupSequence());
        }
    }
    IEnumerator PickupSequence()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMP_Text>().text = "New Clue Added [Press TAB to view].";

        SoundEffectManager.Play("Clue");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);

        if (note != null)
            note.SetActive(false);
    }
}