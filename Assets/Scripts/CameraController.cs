using System;
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
    /// Контроллер оружия.
    /// </summary>
    [SerializeField] private WeaponController weapon = null;

    /// <summary>
    /// Экземпляр текущего класса CameraController.
    /// </summary>
    private static CameraController instance = null;

    /// <summary>
    /// Положение камеры в режиме прицеливания.
    /// </summary>
    private Vector3 cameraOrigin = new Vector3(0, 2, 0);

    public static CameraController Instance
    {
        get => instance;
    }

    /// <summary>
    /// Положение камеры в позиции прицеливания.
    /// </summary>
    public Vector3 CameraOrigin
    {
        get => cameraOrigin;
    }

    private void Awake()
    {
        weapon.OnShot += () =>
        {
            foreach (GameObject shell in weapon.Shells)
            {
                if (shell != null)
                {
                    shell.GetComponent<ShellCollisionDetector>().OnTargetCollision += LookAtTarget;
                }
            }
        };
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

        // Захватить и скрыть курсор.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Посмотреть на мишень.
    /// </summary>
    /// <param name="isNear">Передвинуть камеру к мишени, если true, иначе - вернуть в позицию прицеливания.</param>
    public void LookAtTarget(bool isNear)
    {
        if (isNear)
        {
            shaker.CanShake = false;
            rotator.CanRotate = false;
            transform.position = cameraOrigin + Vector3.forward * (settings.TargetDistance - 10);
        }
        else
        {
            transform.position = cameraOrigin;
            shaker.CanShake = true;
            rotator.CanRotate = true;
        }
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

    /// <summary>
    /// Посмотреть на мишень в результате попадания снаряда.
    /// </summary>
    /// <returns></returns>
    private void LookAtTarget()
    {
        transform.position = cameraOrigin;
        StartCoroutine(MoveToOrigin());
    }

    /// <summary>
    /// Передвинуть камеру в положение прицеливания через две сукунды после вызова.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToOrigin()
    {
        // Вернуть камеру в положение прицеливания после двух секунд изучения мишени.
        yield return new WaitForSeconds(2);
        LookAtTarget(false);
    }
}