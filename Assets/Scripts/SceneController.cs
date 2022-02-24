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
    /// Ссылка на единственный экземпляр данного класса SceneController.
    /// </summary>
    private static SceneController instance = null;

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
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 1;

    /// <summary>
    /// Количество очков, набранное на текущем уровне.
    /// </summary>
    private int currentPoints = 0;

    /// <summary>
    /// Контроллер сцены.
    /// </summary>
    public static SceneController Instance
    {
        get => instance;
    }

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
    public int Points
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
        // Если ссылка на данный экземпляр не установлена, присвоить ее.
        // Если экземпляр уже существует, уничтожить текущий игровой объект.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        // Добавить на сцену мишень.
        targetCreator.CreateTarget();
    }

    /// <summary>
    /// Начислить очки игроку.
    /// </summary>
    /// <param name="color"></param>
    public void ScorePoints(Color color)
    {
        if (color == settings.TargetsColors[0])
        {
            currentPoints += settings.PointsRange[0];
        }
        else if (color == settings.TargetsColors[0])
        {
            currentPoints += settings.PointsRange[0];
        }
        else if (color == settings.TargetsColors[0])
        {
            currentPoints += settings.PointsRange[0];
        }
        else if (color == settings.TargetsColors[0])
        {
            currentPoints += settings.PointsRange[0];
        }
        else
        {
            return;
        }

        Debug.Log(currentPoints);
    }
}