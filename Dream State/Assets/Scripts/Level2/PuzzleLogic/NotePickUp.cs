using UnityEngine;

public class NotePickUp : MonoBehaviour
{
    public static bool uiActive;
    bool canPickUp;
    public GameObject note;
    public GameObject notesTab;
    public GameObject notesPages;

    void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            note.SetActive(false);
            notesTab.SetActive(true);
            SoundEffectManager.Play("ItemPickUp");
            ResetUI();
        }
    }

    void OnMouseOver()
    {
        if (PlayerCasting.distanceFromTarget < 5)
        {
            canPickUp = true;
            UIController.actionText = "Pick up Note";
            UIController.commandText = "Pick Up";
            UIController.uiActive = true;

        }
        else
        {
            ResetUI();
        }
    }

    void OnMouseExit()
    {
        ResetUI();
    }

    void ResetUI()
    {
        canPickUp = false;
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;
    }
}
