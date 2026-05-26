using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChapterScreen : MonoBehaviour
{
    public Image chapterImage;
    public float fadeDuration = 1f;
    public float holdDuration = 2f;

    void Start()
    {
        StartCoroutine(ShowChapter());
    }

    IEnumerator ShowChapter()
    {
        Color c = chapterImage.color;

        c.a = 0; //empieza invisible
        chapterImage.color = c;

        chapterImage.gameObject.SetActive(true);

        float t = 0; //fade in

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            chapterImage.color = c;

            yield return null;
        }

        yield return new WaitForSeconds(holdDuration);

        t = 0; //fade out

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            chapterImage.color = c;

            yield return null;
        }

        chapterImage.gameObject.SetActive(false);
    }
}