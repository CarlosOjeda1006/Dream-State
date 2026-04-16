using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneToOther : MonoBehaviour
{
    [SerializeField] private Slider loadBar;
    [SerializeField] private GameObject loadPanel;

    public void SceneLoad(string sceneName)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync("Level_01"));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            loadBar.value = asyncOperation.progress;
            yield return null;
        }

    }
}