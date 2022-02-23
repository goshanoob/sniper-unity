using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Контроллер сцены.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Настройки игры.
    /// </summary>
    [SerializeField] private GameSettings settings = null;

    /// <summary>
    /// Управление от игрока.
    /// </summary>
    [SerializeField] private PlayerControl player = null;

    /// <summary>
    /// Настройки оружия.
    /// </summary>
    [SerializeField] private WeaponController weapon = null;

    /// <summary>
    /// Камера.
    /// </summary>
    [SerializeField] private CameraController cameraController = null;

    /// <summary>
    /// Генератор мишени.
    /// </summary>
    [SerializeField] private TargetCreator targetCreator = null;

    private float timer = 0;
    private bool isForward = true;
    private float x1 = -2f * Mathf.Sqrt(2);
    private float dx = 0.2f;
    private float a = 2f;

    private void Awake()
    {
        // Зарегистрировать обработчики событий в ответ на действия пользователя.
        player.OnLeftClick += weapon.CreateShells;
        player.OnSpaceAction += cameraController.LookAtTarget;
        player.OnSpaceAction += cameraController.LookAtTarget;
        player.OnOneButtonPress += weapon.ChangeWeapon;
        player.OnTwoButtonPress += weapon.ChangeWeapon;
        player.OnThreeButtonPress += weapon.ChangeWeapon;
        player.OnFourButtonPress += weapon.ChangeWeapon;

        // Обработать событие выстрела из оружия.
        weapon.OnShot += cameraController.ChangeToChase;
    }

    private void Start()
    {
        targetCreator.CreateTarget();
    }
}