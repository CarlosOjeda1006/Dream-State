using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenus : MonoBehaviour
{
    [SerializeField] private GameObject buttonOptions;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject buttonExit;
    [SerializeField] private GameObject menuSettings;

    public void Options()
    {
        //Time.timeScale = 0f;
        mainMenu.SetActive(false);
        menuSettings.SetActive(true);
    }
    public void ExitOptions()
    {
        //Time.timeScale = 0f;
        mainMenu.SetActive(true);
        menuSettings.SetActive(false);
    }
}

