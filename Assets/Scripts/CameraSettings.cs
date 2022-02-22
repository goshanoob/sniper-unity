using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Настройки камеры.
/// </summary>
public class CameraSettings : MonoBehaviour
{
    private static CameraSettings instance = null;

    private Vector3 cameraOrigin = Vector3.zero;
    
    public static CameraSettings Instance
    {
        get => instance;
    }

    public Vector3 CameraOrigin
    {
        get => cameraOrigin;
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else if (instance = this)
        {
            Destroy(gameObject);
        }
    }
}
