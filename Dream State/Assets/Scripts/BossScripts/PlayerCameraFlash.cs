using System;
using System.Collections;
using UnityEngine;

public class PlayerCameraFlash : MonoBehaviour
{
    public Action<RaycastHit> OnPhotoTaken;

    [Header("References")]
    public Camera playerCamera;

    [Header("Flash")]
    public Light flashLight;

    [Header("Photo")]
    public float photoRange = 30f;

    void Update()
    {
        if (Input.GetButtonDown("Linterna"))
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        SoundEffectManager.Play("Flash");

        StartCoroutine(FlashEffect());

        Debug.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * photoRange,
            Color.red,
            1f
        );

        RaycastHit hit;

        Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            photoRange
        );

        Debug.Log("foto tomada");

        OnPhotoTaken?.Invoke(hit);
    }

    IEnumerator FlashEffect()
    {
        flashLight.intensity = 10f;

        yield return new WaitForSeconds(0.05f);

        flashLight.intensity = 0f;
    }
}