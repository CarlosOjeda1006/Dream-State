using System.Collections;
using TMPro;
using UnityEngine;

public class BoltCuttersController : MonoBehaviour, IInteractable
{
    public static bool hasBoltCutters = false;
    bool alreadyPickedUp = false;
    public GameObject instructionsBox;
    public bool CanInteract => !alreadyPickedUp;

    public GameObject Visual;

    public void Interact()
    {
        if (hasBoltCutters) return;

        hasBoltCutters = true;
        alreadyPickedUp = true;

        SoundEffectManager.Play("ItemPickUp");

        StartCoroutine(PickupSequence());
    }

    IEnumerator PickupSequence()
    {
        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMP_Text>().text =
            "Maybe I can open the gate with this.";

        SoundEffectManager.Play("OpenGate");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);

        if (Visual != null)
            Visual.SetActive(false);
    }
}