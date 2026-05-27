using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;

    public Image fadeImage;

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeOut(string sceneName, float duration)
    {
        float timer = 0f;

        Color color = fadeImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float alpha = timer / duration;

            fadeImage.color =
                new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}