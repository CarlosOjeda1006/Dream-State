using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BoltCuttersController : MonoBehaviour
{
    public static bool uiActive;
    bool canPickUp;
    public GameObject boltcutters;
    public static bool hasBoltCutters = false;

    void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            hasBoltCutters = true;
            boltcutters.SetActive(false);
            SoundEffectManager.Play("ItemPickUp");
            ResetUI();
        }
    }

    void OnMouseOver()
    {
        if (PlayerCasting.distanceFromTarget < 5)
        {
            canPickUp = true;
            UIController.actionText = "Pick up Bolt Cutters";
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
