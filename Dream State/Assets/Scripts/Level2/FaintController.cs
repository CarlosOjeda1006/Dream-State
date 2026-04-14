using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaintController : MonoBehaviour
{
    public Transform cameraTransform;
    public Image blackImage;
    public FirstPersonController playerController;
    public bool isFainting = false;

    void OnEnable()
    {
        DoorsL2.OnJumpscareTriggered += StartFaint;
    }

    void OnDisable()
    {
        DoorsL2.OnJumpscareTriggered -= StartFaint;
    }

    void StartFaint()
    {
        if (!isFainting)
        {
            StartCoroutine(FaintRoutine());
        }
    }

    IEnumerator FaintRoutine()
    {
        isFainting = true;
        playerController.enabled = false;

        float time = 0;
        float duration = 3.5f;

        Quaternion startRot = cameraTransform.rotation;
        Quaternion endRot = Quaternion.Euler(-80f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);

        Color color = blackImage.color;

        while (time < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(startRot, endRot, time / duration);

            color.a = Mathf.Lerp(0, 1, time / duration);
            blackImage.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        blackImage.color = color;
        isFainting = false;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ThankYou4Playing");
    }
}