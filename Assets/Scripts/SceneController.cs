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
    }

    private void Awake()
    {
        // Зарегистрировать обработчики событий в ответ на действия пользователя.
        player.OnLeftClick += weapon.MakeShot;
        player.OnSpaceDown += cameraController.ShowTarget;
        player.OnSpaceUp += cameraController.MoveToOrigin;
        player.OnNumButtonPress += weapon.ChangeWeapon;
        // Выполнить только в начале уровня.
        player.OnNumButtonPress += uiController.PrintWeapon;

        weapon.OnShot += () => { };
    }
    
    private void Start()
    {
        // Добавить на сцену мишень.
        targetCreator.CreateTarget();
        // Вывести статистику игры пользователю.
        uiController.PrintLevel(currentLevel);
        uiController.PrintPoints(currentPoints);
        uiController.PrintWeapon(weapon.CurrentWeapon);
        uiController.PrintShots(weapon.MaxShots);
    }

    /// <summary>
    /// Начать следующий уровень.
    /// </summary>
    private void StartNextLevel()
    {
        currentLevel++;
        // Если пройдены все уровни, завершить игру.
        if (currentLevel > settings.LevelCount)
        {
            cameraController.ShowTarget();
            uiController.OpenPopupWindow("Вы победили! Ура!");
            return;
        }
        // Сбросить количество очков и выстрелов.
        currentPoints = 0;
        weapon.RechargeWeapon();
        // Удалить старую мишень и добавить новую.
        targetCreator.RemoveTarget();
        targetCreator.CreateTarget();
        
        // Обновить графический интерфейс.
        uiController.PrintPoints(currentPoints);
        uiController.PrintLevel(currentLevel);
        uiController.PrintShots(weapon.MaxShots);
    }

    /// <summary>
    /// Окончить игру из-за проигрыша.
    /// </summary>
    private void ShowGameOver()
    {
        cameraController.ShowTarget();
        uiController.OpenPopupWindow("Вы проиграли. Увы.");
    }
    
    /// <summary>
    /// Начислить очки игроку и учесть выстрелы.
    /// </summary>
    /// <param name="points">Количество очков.</param>
    /// <param name="shots">Оставшиеся выстрелы.</param>
    public void ScorePointsAndShots(float points, int shots)
    {
        currentPoints += points * weapon.CurrentMultiplier;
        uiController.PrintPoints(currentPoints);
        uiController.PrintShots(shots);
        // Если набрано достаточное количество очков, перейти на следующий уровень,
        // иначе - проверить условие проигрыша.
        if (currentPoints >= toNextLevelPoints)
        {
            StartNextLevel();
        }
        else
        {
            if (shots <= 0)
            {
                ShowGameOver();
            }
        }
    }

    /// <summary>
    /// Показать сообщение пользователю.
    /// </summary>
    /// <param name="message">Текст сообщения.</param>
    public void ShowMessage(string message)
    {
        uiController.ShowMessage(message);
    }
}