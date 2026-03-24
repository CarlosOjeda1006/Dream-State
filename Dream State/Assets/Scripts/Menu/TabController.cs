using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    bool isFirstTime = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //siempre abre a la primer page
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        if (!isFirstTime)
        {
            SoundEffectManager.Play("SwitchTab");
        }
        isFirstTime = false;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }
        pages[tabNo].SetActive(true); //al clickearle
        tabImages[tabNo].color = Color.white;
    }
}
