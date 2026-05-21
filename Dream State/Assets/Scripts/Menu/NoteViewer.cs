using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteViewer : MonoBehaviour
{
    public GameObject viewerPanel;
    public Image bigImage;

    public GameObject firstButton;
    public GameObject previousSelected;
    public void OpenNote(Sprite noteSprite)
    {
        SoundEffectManager.Play("Inventory_Open");
        previousSelected = EventSystem.current.currentSelectedGameObject;

        viewerPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButton);

        bigImage.sprite = noteSprite;

    }

    public void CloseNote()
    {
        SoundEffectManager.Play("SwitchTab");
        viewerPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(previousSelected);
    }
}