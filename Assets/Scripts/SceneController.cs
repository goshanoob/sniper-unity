using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// Счетчик прилетевших снарядов.
    /// </summary>
    private int shellsCounter = 0;

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
        player.OnNumButtonPress += ChangeWeapon;
        uiController.OnRestart += RestartGame;
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
    /// Начислить очки игроку и учесть выстрелы.
    /// </summary>
    /// <param name="points">Количество очков.</param>
    /// <param name="shots">Оставшиеся выстрелы.</param>
    public void ScorePointsAndShots(float points, int shots)
    {
        shellsCounter++;
        if (points != 0)
        {
            currentPoints += points * weapon.CurrentMultiplier;
            uiController.PrintPoints(currentPoints);
        }

        // Если все снаряды прилетели, показать мишень и проверить доступен ли следующий уровень.
        if (shellsCounter == weapon.CurrentShellsCount)
        {
            shellsCounter = 0;
            StartCoroutine(cameraController.ShowTargetForTime(3));
            uiController.PrintShots(shots);
            StartCoroutine(CheckNextLevel(shots));
        }
    }

    /// <summary>
    /// Сменить оружие.
    /// </summary>
    /// <param name="number">Номер оружия.</param>
    private void ChangeWeapon(int number)
    {
        {
            // Если не было произведено ни одного выстрела, сменить оружие.
            if (weapon.ShotsCounter == 0)
            {
                weapon.ChangeWeapon(number);
                uiController.PrintWeapon(number);
                uiController.PrintShots(weapon.MaxShots);
                cameraController.ChangeShakeSpeed(weapon.Shakes);
            }
            else
            {
                uiController.ShowMessage("Смена оружия только в начале уровня");
            }
        }
        ;
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
            currentLevel--;
            cameraController.ShowTarget();
            uiController.OpenPopupWindow("Вы победили! Ура!");
            return;
        }

        ConfigNewLevel();
    }

    /// <summary>
    /// Сбросить настройки уровня на начальные.
    /// </summary>
    private void ConfigNewLevel()
    {
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
    /// Проверить доступность следующего уровня или проигрыш.
    /// </summary>
    private IEnumerator CheckNextLevel(int shots)
    {
        Debug.Log("CheckNextLevel");
        yield return new WaitForSeconds(2);
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
    /// Начать игру заново.
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}