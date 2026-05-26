using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayMenus : MonoBehaviour
{
    [SerializeField] private GameObject buttonOptions;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject buttonExit;
    [SerializeField] private GameObject menuSettings;

    public GameObject firstButtonMenu;
    public GameObject firstButtonSettings;
    public void Options()
    {
        //Time.timeScale = 0f;
        mainMenu.SetActive(false);
        menuSettings.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);

    }
    public void ExitOptions()
    {
        //Time.timeScale = 0f;
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonMenu);
        menuSettings.SetActive(false);
    }
}

