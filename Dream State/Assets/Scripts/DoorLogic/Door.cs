using UnityEngine;

public class Door : MonoBehaviour
{
    void OnMouseOver()
    {
        UIController.actionText = "Abrir Puerta";
        UIController.commandText = "Abrir";
        UIController.uiActive = true;
    }

    void OnMouseExit()
    {
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.uiActive = false;
    }
}
