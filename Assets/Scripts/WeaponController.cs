using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер оружия.
/// </summary>
public class WeaponController : MonoBehaviour
{
    /// <summary>
    /// Настройки игры.
    /// </summary>
    [SerializeField] private GameSettings settings = null;

    /// <summary>
    /// Камера на сцене.
    /// </summary>
    [SerializeField] private CameraController cameraController = null;

    /// <summary>
    /// Контроллер сцены.
    /// </summary>
    [SerializeField] private SceneController sceneController = null;

    /// <summary>
    /// Сила выстрела снарядом.
    /// </summary>
    [SerializeField] private float shotForce = 8f;

    /// <summary>
    /// Текущее выбранное оружие.
    /// </summary>
    private int currentWeapon = 1;

    /// <summary>
    /// Снаряды на сцене.
    /// </summary>
    private GameObject[] shells = null;

    /// <summary>
    /// Количество произведенных выстрелов.
    /// </summary>
    private int shotsCounter = 0;

    /// <summary>
    /// Максимальное количество снарядов, доступное в игре.
    /// </summary>
    private readonly int maxShellsCount = 5;

    /// <summary>
    /// Положения снарядов при создании.
    /// </summary>
    private readonly Vector3[] shellsPositions = new Vector3[]
    {
        new Vector3(0, 0, 0),
        new Vector3(-0.4f, 0.4f, 0),
        new Vector3(0.4f, 0.4f, 0),
        new Vector3(-0.4f, -0.4f, 0),
        new Vector3(0.4f, -0.4f, 0)
    };

    /// <summary>
    /// Количество снарядов в соответствии с выбранным оружием.
    /// </summary>
    private readonly int[] shellsCount = new int[] {1, 1, 5, 5};

    /// <summary>
    /// Множители количества очков в соответствии с выбранным оружием.
    /// </summary>
    private readonly float[] multipliers = new float[] {0.5f, 1f, 0.1f, 0.2f};

    /// <summary>
    /// Количество доступных выстрелов в соответствии с выбранным оружием.
    /// </summary>
    private readonly int[] maxShots = new int[] {3, 2, 3, 2};

    /// <summary>
    /// Сложность стрельбы в соответствии с выбранным оружием.
    /// </summary>
    private readonly int[] shakes = new int[] { 0, 1, 0, 1 };
    
    /// <summary>
    /// Выбранное оружие.
    /// </summary>
    public int CurrentWeapon
    {
        get => currentWeapon;
    }

    /// <summary>
    /// Снаряды.
    /// </summary>
    public GameObject[] Shells
    {
        get => shells;
    }

    /// <summary>
    /// Количество снарядов за выстрел в зависимости от выбранного оружия.
    /// </summary>
    public int CurrentShellsCount
    {
        get => shellsCount[currentWeapon - 1];
    }

    /// <summary>
    /// Множитель начисления очков в зависимости от выбранного оружия.
    /// </summary>
    public float CurrentMultiplier
    {
        get => multipliers[currentWeapon - 1];
    }

    /// <summary>
    /// Количество выполненных выстрелов.
    /// </summary>
    public int ShotsCounter
    {
        get => shotsCounter;
    }

    /// <summary>
    /// Максимально доступное число выстрелов для выбранного оружия.
    /// </summary>
    public int MaxShots
    {
        get => maxShots[currentWeapon - 1];
    }

    /// <summary>
    /// Сложность стрельбы (скорость качания камеры).
    /// </summary>
    public int Shakes
    {
        get => shakes[currentWeapon - 1];
    }
    
    /// <summary>
    /// Оставшееся количество выстрелов.
    /// </summary>
    private int ExistingShots
    {
        get => MaxShots - ShotsCounter;
    }

    private void Start()
    {
        // Выделить память для массива снарядов.
        shells = new GameObject[maxShellsCount];
    }

    private void Update()
    {
        // Проверить вылет снаряда за пределы игрового поля.
        CheckForDestroy();
    }

    /// <summary>
    /// Создать снаряды.
    /// </summary>
    public void MakeShot()
    {
        // Если снаряды уже выпущены, завершить выполнение метода.
        if (shells[0] != null)
        {
            return;
        }

        // Определить положение и ориентацию камеры.
        Vector3 cameraDirection = cameraController.gameObject.transform.forward;
        for (int i = 0; i < CurrentShellsCount; i++)
        {
            // Получить экземпляр снаряда.
            shells[i] = GetShellInstance();
            shells[i].transform.position = cameraController.CameraOrigin + shellsPositions[i];
            shells[i].GetComponent<Rigidbody>().AddForce(cameraDirection * shotForce, ForceMode.Impulse);
        }

        shotsCounter++;
        // Переключить камеру в режим следования за снарядом.
        cameraController.ChangeToChase();
    }

    /// <summary>
    /// Сгенерировать снаряд.
    /// </summary>
    /// <returns>Снаряд.</returns>
    private GameObject GetShellInstance()
    {
        GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Rigidbody body = shell.AddComponent<Rigidbody>();
        ShellCollisionDetector detector = shell.AddComponent<ShellCollisionDetector>();
        shell.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        body.mass = 0.2f;
        // Выбрать постоянный режим определения коллизий.
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        // Зарегистрировать обработчики событий в результате коллизий.
        detector.OnTargetCollision += OnTargetCollisionEventHandler;
        detector.OnMissed += OnMissedEventHandler;
        return shell;
    }

    /// <summary>
    /// Обработчик события столкновения снаряда с мишенью.
    /// </summary>
    private void OnTargetCollisionEventHandler(GameObject shell, float points)
    {
        Debug.Log(points);
        // Начислить игроку очки и проверить выстрелы.
        sceneController.ScorePointsAndShots(points, ExistingShots);
        // Удалить снаряд со сцены.
        StartCoroutine(RemoveShellForSecond(shell));
    }

    /// <summary>
    /// Обработчик события промаха снаряда.
    /// </summary>
    private void OnMissedEventHandler(GameObject sender)
    {
        RemoveShellNow(sender);
        // Начислить игроку очки и выстрелы, проверить условие поражения.
        sceneController.ScorePointsAndShots(0, ExistingShots);
    }

    /// <summary>
    /// Удалить снаряды в результате вылета за границы игрового поля.
    /// </summary>
    private void CheckForDestroy()
    {
        
        // Получить размеры игрового поля и текущее положение снаряда.
        float[] fieldSizes = settings.GameFieldSizes;
        foreach (GameObject shell in shells)
        {
            if (shell != null)
            {
                Vector3 currentPosition = shell.transform.position;
                Debug.Log(currentPosition);
                // Если снаряд вылетел за границы игрового поля, уничтожить его немедленно и вернуть камеру в режим прицеливания.
                if (currentPosition.x < -fieldSizes[0] / 2 || currentPosition.x > fieldSizes[0] / 2 ||
                    currentPosition.y < 0 ||
                    currentPosition.z > fieldSizes[1] || currentPosition.z < 0)
                {
                    RemoveShellNow(shell);
                    // Начислить игроку очки и выстрелы, проверить условие поражения.
                    sceneController.ScorePointsAndShots(0, ExistingShots);
                    Debug.Log("CheckForDestroy");
                }
            }
        }
    }

    /// <summary>
    /// Уничтожить снаряды по истечении времени.
    /// </summary>
    private IEnumerator RemoveShells()
    {
        // Отложить уничтожение снарядов на одну секунду.
        yield return new WaitForSeconds(1);
        RemoveShellsNow();
    }

    /// <summary>
    /// Уничтожить снаряды немедленно.
    /// </summary>
    private void RemoveShellsNow()
    {
        // Перебрать игровые объекты снарядов и уничтожить.
        foreach (GameObject shell in shells)
        {
            // Отписаться от событий класса-компонента перед удалением объекта.
            if (shell != null)
            {
                ShellCollisionDetector detector = shell.GetComponent<ShellCollisionDetector>();
                detector.OnTargetCollision -= OnTargetCollisionEventHandler;
                detector.OnMissed -= OnMissedEventHandler;
                Destroy(shell);
            }
        }

        // Очистить массив со ссылками на игровые снаряды.
        Array.Clear(shells, 0, shells.Length);
    }

    private void RemoveShellNow(GameObject shell)
    {
        if (shell != null)
        {
            ShellCollisionDetector detector = shell.GetComponent<ShellCollisionDetector>();
            detector.OnTargetCollision -= OnTargetCollisionEventHandler;
            detector.OnMissed -= OnMissedEventHandler;
            Destroy(shell);
        }
    }

    private IEnumerator RemoveShellForSecond(GameObject shell)
    {
        yield return new WaitForSeconds(2);
        RemoveShellNow(shell);
    }

    /// <summary>
    /// Сменить оружие.
    /// </summary>
    /// <param name="weapon">Номер выбранного оружия.</param>
    public void ChangeWeapon(int weapon)
    {
        currentWeapon = weapon;
    }

    /// <summary>
    /// Перезарядить оружие.
    /// </summary>
    public void RechargeWeapon()
    {
        // Сбросить количество выстрелов.
        shotsCounter = 0;
    }
}