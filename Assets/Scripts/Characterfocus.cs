using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characterfocus : MonoBehaviour
{
    public Camera mainCamera;
    public float smoothing = 5f;

    public float cameraDistance = -10f;

    private Vector3 offset;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        CenterCameraOnCharacter();
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = transform.position + offset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

        void CenterCameraOnCharacter()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, cameraDistance);
    }
}
