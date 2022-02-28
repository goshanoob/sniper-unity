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
    /// Контроллер графического интерфейса.
    /// </summary>
    [SerializeField] private UIController uiController = null;

    /// <summary>
    /// Положение камеры в режиме прицеливания.
    /// </summary>
    private readonly Vector3 cameraOrigin = new Vector3(0, 2, 0);

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
    /// Изучить мишень.
    /// </summary>
    public void ShowTarget()
    {
        // Местоположение камеры вблизи мишени.
        float distanceToTarget = 10f;
        
        Vector3 lookPosition = cameraOrigin + Vector3.forward * (settings.TargetDistance - distanceToTarget);
        transform.position = lookPosition;
        transform.LookAt( lookPosition + Vector3.forward);

        chaser.CanChase = false;
        shaker.CanShake = false;
        rotator.CanRotate = false;
    }

    /// <summary>
    /// Посмотреть на мишень в течение 2 секунд.
    /// </summary>
    public IEnumerator ShowTargetForTime(float time)
    {
        ShowTarget();
        yield return new WaitForSeconds(time);
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
        uiController.ToggleAim(true);
    }

    /// <summary>
    /// Разрешить следование за снарядом после выстрела.
    /// </summary>
    public void ChangeToChase()
    {
        rotator.CanRotate = false;
        shaker.CanShake = false;
        chaser.CanChase = true;
        uiController.ToggleAim(false);
    }

    /// <summary>
    /// Изменить скорость качания камеры.
    /// </summary>
    /// <param name="speed">Скорость качания.</param>
    public void ChangeShakeSpeed(int speed)
    {
        shaker.ShakeMode = speed;
    }
}