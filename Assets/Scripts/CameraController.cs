using System.Collections;
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
    /// Вращение камеры.
    /// </summary>
    [SerializeField] private CameraRotator rotator = null;

    /// <summary>
    /// Качание камеры.
    /// </summary>
    [SerializeField] private CameraShaker shaker = null;

    /// <summary>
    /// Преследование камерой летящего снаряда.
    /// </summary>
    [SerializeField] private CameraChaser chaser = null;

    /// <summary>
    /// Положение камеры в режиме прицеливания.
    /// </summary>
    private Vector3 cameraOrigin = new Vector3(0, 2, 0);

    /// <summary>
    /// Положение камеры в позиции прицеливания.
    /// </summary>
    public Vector3 CameraOrigin
    {
        get => cameraOrigin;
    }

    private void Start()
    {
        // Захватить и скрыть курсор.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Посмотреть на мишень.
    /// </summary>
    public void ShowTarget()
    {
        // Местоположение мишени.
        Vector3 lookPosition = cameraOrigin + Vector3.forward * (settings.TargetDistance - 10);

        chaser.CanChase = false;
        shaker.CanShake = false;
        rotator.CanRotate = false;

        transform.position = lookPosition;
        transform.LookAt(lookPosition);
    }

    /// <summary>
    /// Посмотреть на мишень в течение 2 секунд.
    /// </summary>
    public IEnumerator ShowTargetForTime()
    {
        ShowTarget();
        yield return new WaitForSeconds(2);
        MoveToOrigin();
    }

    /// <summary>
    /// Передвинуть камеру в положение прицеливания.
    /// </summary>
    public void MoveToOrigin()
    {
        transform.position = cameraOrigin;
        shaker.CanShake = true;
        rotator.CanRotate = true;
    }

    /// <summary>
    /// Разрешить следование за снарядом после выстрела.
    /// </summary>
    public void ChangeToChase()
    {
        rotator.CanRotate = false;
        shaker.CanShake = false;
        chaser.CanChase = true;
    }
}