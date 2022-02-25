using System;
using System.Collections;
using UnityEditor.SearchService;
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

    /// <summary>
    /// Контроллер графического интерфейса.
    /// </summary>
    [SerializeField] private UIController uiController = null;

    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 1;

    /// <summary>
    /// Количество очков, набранное на текущем уровне.
    /// </summary>
    private float currentPoints = 0;

    /// <summary>
    /// Количество очков, необходимое для перехода на следующий уровень.
    /// </summary>
    private readonly float toNextLevelPoints = 100;

    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    public int CurrentLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    }

    /// <summary>
    /// Количество очков, набранное на текущем уровне.
    /// </summary>
    public float Points
    {
        get => currentPoints;
        set => currentPoints = value;
    }

    private void Awake()
    {
        // Зарегистрировать обработчики событий в ответ на действия пользователя.
        player.OnLeftClick += weapon.CreateShells;
        player.OnSpaceDown += cameraController.ShowTarget;
        player.OnSpaceUp += cameraController.MoveToOrigin;
        player.OnOneButtonPress += weapon.ChangeWeapon;
        player.OnTwoButtonPress += weapon.ChangeWeapon;
        player.OnThreeButtonPress += weapon.ChangeWeapon;
        player.OnFourButtonPress += weapon.ChangeWeapon;
    }

    private void Start()
    {
        // Добавить на сцену мишень.
        targetCreator.CreateTarget();
        // Вывести статистику игры пользователю.
        uiController.PrintLevel(currentLevel);
        uiController.PrintPoints(currentPoints);
        uiController.PrintWeapon(weapon.CurrentWeapon);
        uiController.PrintShots(weapon.ShotsCount);
    }

    /// <summary>
    /// Начислить очки игроку.
    /// </summary>
    /// <param name="color"></param>
    public void ScorePoints(Color color)
    {
        if (color == settings.TargetsColors[0])
        {
            currentPoints += settings.PointsRange[0] * weapon.CurrentMultiplicator;
        }
        else if (color == settings.TargetsColors[1])
        {
            currentPoints += settings.PointsRange[1] * weapon.CurrentMultiplicator;
        }
        else if (color == settings.TargetsColors[2])
        {
            currentPoints += settings.PointsRange[2] * weapon.CurrentMultiplicator;
        }
        else if (color == settings.TargetsColors[3])
        {
            currentPoints += settings.PointsRange[3] * weapon.CurrentMultiplicator;
        }
        else
        {
            return;
        }

        Debug.Log(currentPoints);
        // Если набрано достаточное количество очков, перейти на следующий уровень.
        if (currentPoints >= toNextLevelPoints)
        {
            StartNextLevel();
        }
    }

    // Начать следующий уровень.
    private void StartNextLevel()
    {
        currentLevel++;
        // Если пройдены все уровни завершить игру.
        if (currentLevel > settings.LevelCount)
        {
            return;
        }

        currentPoints = 0;
        // Удалить старую мишень.
        targetCreator.RemoveTarget();
        // Добавить на сцену новую мишень.
        targetCreator.CreateTarget();
    }
}