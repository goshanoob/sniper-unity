using System;
using UnityEngine;

/// <summary>
/// Настройки камеры.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Настройки игры.
    /// </summary>
    [SerializeField] private GameSettings settings = null;

    /// <summary>
    /// Качание камеры.
    /// </summary>
    [SerializeField] private CameraShaker shaker = null;

    /// <summary>
    /// Преследование камеры летящего снаряда.
    /// </summary>
    [SerializeField] private CameraChaser chaser = null;

    /// <summary>
    /// Экземпляр текущего класса CameraController.
    /// </summary>
    private static CameraController instance = null;

    /// <summary>
    /// Положение камеры в режиме прицеливания.
    /// </summary>
    private Vector3 cameraOrigin = new Vector3(0, 2 ,10);

    public static CameraController Instance
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

    /// <summary>
    /// Посмотреть на мишень.
    /// </summary>
    /// <param name="isNear">Передвинуть камеру к мишени.</param>
    public void LookAtTarget(bool isNear)
    {
        if (isNear)
        {
            shaker.CanShake = false;
            transform.position = cameraOrigin + Vector3.forward * (settings.TargetDistance - 10);
        }
        else
        {
            transform.position = cameraOrigin;
            shaker.CanShake = true;
        }
    }

    /// <summary>
    /// Разрешить следование за снарядом после выстрела.
    /// </summary>
    public void ChangeToChase() => chaser.CanChase = true;
}