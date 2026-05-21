using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteViewer : MonoBehaviour
{
    public GameObject viewerPanel;
    public Image bigImage;

    public GameObject firstButton;

    public void OpenNote(Sprite noteSprite)
    {
        SoundEffectManager.Play("Inventory_Open");
        viewerPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButton);

        bigImage.sprite = noteSprite;
    }

    public void CloseNote()
    {
        SoundEffectManager.Play("SwitchTab");
        viewerPanel.SetActive(false);
    }
}