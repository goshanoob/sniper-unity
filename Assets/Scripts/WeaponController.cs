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
    /// Событие выстрела.
    /// </summary>
    public event Action OnShot;

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
    private int CurrentShellsCount
    {
        get => shellsCount[currentWeapon - 1];
    }

    // Множитель начисления очков в зависимости от выбранного оружия.
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

    private void Start()
    {
        // Выделить память для массива снарядов.
        shells = new GameObject[maxShellsCount];
    }

    private void Update()
    {
        // Если на сцене существует снаряд, проверить не покинул ли он игровое поле.
        if (shells[0] != null)
        {
            CheckForDestroy();
        }
    }

    /// <summary>
    /// Создать снаряды.
    /// </summary>
    public void CreateShells()
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

        OnShot();
        // Учесть выстрел.
        CheckShotsCount();
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
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // Зарегистрировать обработчики событий в результате коллизий.
        detector.OnTargetCollision += OnTargetCollisionEventHandler;
        detector.OnMissed += OnMissedEventHandler;
        return shell;
    }

    /// <summary>
    /// Обработчик события столкновения снаряда с мишенью.
    /// </summary>
    private void OnTargetCollisionEventHandler(float points)
    {
        // Начислить игроку очки.
        sceneController.ScorePoints(points);

        // Удалить снаряды со сцены и показать мишень на несколько секунд.
        StartCoroutine(cameraController.ShowTargetForTime());
        StartCoroutine(RemoveShells());
    }

    /// <summary>
    /// Обработчик события потери снарядом скорости.
    /// </summary>
    private void OnMissedEventHandler()
    {
        RemoveShellsNow();
        cameraController.MoveToOrigin();
    }

    /// <summary>
    /// Удалить снаряды в результате промаха.
    /// </summary>
    private void CheckForDestroy()
    {
        // Получить размеры игрового поля и текущее положение снаряда.
        float[] fieldSizes = settings.GameFieldSizes;
        Vector3 currentPosition = shells[0].transform.position;

        // Если снаряд вылетел за границы игрового поля, уничтожить его немедленно и вернуть камеру в режим прицеливания.
        if (currentPosition.x < -fieldSizes[0] / 2 || currentPosition.x > fieldSizes[0] / 2 ||
            currentPosition.y < 0 || 
            currentPosition.z > fieldSizes[1] || currentPosition.z < 0)
        {
            RemoveShellsNow();
            cameraController.MoveToOrigin();
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

    /// <summary>
    /// Сменить оружие.
    /// </summary>
    /// <param name="weapon">Номер выбранного оружия.</param>
    public void ChangeWeapon(int weapon)
    {
        currentWeapon = weapon;
    }

    private void CheckShotsCount()
    {
        shotsCounter++;
        sceneController.ChangeShots();
        // Переключить камеру в режим следования за снарядом.
        cameraController.ChangeToChase();
    }
}