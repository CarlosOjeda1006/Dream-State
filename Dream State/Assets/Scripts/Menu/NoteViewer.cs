using UnityEngine;
using UnityEngine.UI;

public class NoteViewer : MonoBehaviour
{
    public GameObject viewerPanel;
    public Image bigImage;

    public void OpenNote(Sprite noteSprite)
    {
        SoundEffectManager.Play("Inventory_Open");
        viewerPanel.SetActive(true);
        bigImage.sprite = noteSprite;
    }

    public void CloseNote()
    {
        SoundEffectManager.Play("SwitchTab");
        viewerPanel.SetActive(false);
    }
}