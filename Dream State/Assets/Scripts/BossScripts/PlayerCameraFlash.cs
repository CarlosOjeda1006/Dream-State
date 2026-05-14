using System;
using UnityEngine;

public class PlayerCameraFlash : MonoBehaviour
{
    public Action OnPhotoTaken;

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
        // SONIDO
        // UI

        OnPhotoTaken?.Invoke();
    }
}