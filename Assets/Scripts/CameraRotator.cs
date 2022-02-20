using System;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float maxVerticalAngle = -90f;
    [SerializeField] private float minVerticalAngle = 90f;

    private float currentXRotation = 0;
    private float currentYRotation = 0;
    
    private void Update()
    {
        float yRotation = Input.GetAxis("Mouse X") * sensitivity;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivity;
        currentXRotation += xRotation;
        currentYRotation += yRotation;
        currentXRotation = Mathf.Clamp(currentXRotation,maxVerticalAngle, minVerticalAngle);
        transform.localEulerAngles = new Vector3(currentXRotation, currentYRotation, 0);
        
        
    }
}
