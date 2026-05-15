using System;
using UnityEngine;

public class PlayerCameraFlash : MonoBehaviour
{
    public Action<RaycastHit> OnPhotoTaken;

    [Header("References")]
    public Camera playerCamera;

    [Header("Photo")]
    public float photoRange = 30f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        // FLASH
        SoundEffectManager.Play("Flash");
        // UI

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
}