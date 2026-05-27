using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    [Header("Cosas")]
    public Canvas fadeCanvas;
    public Image fadeImage;

    public void StartFade(string sceneName)
    {
        StartCoroutine(Fade(sceneName));
    }

    IEnumerator Fade(string sceneName)
    {
        fadeCanvas.enabled = true;

        Color color = fadeImage.color;

        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime;

            fadeImage.color =
                new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}