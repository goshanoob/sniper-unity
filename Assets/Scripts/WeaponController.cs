using System;
using System.Collections;
using System.Timers;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    /// <summary>
    /// Настройки игры.
    /// </summary>
    [SerializeField] private GameSettings settings = null;

    /// <summary>
    /// Префаб снаряда.
    /// </summary>
    [SerializeField] private GameObject shellPrefab = null;

    /// <summary>
    /// Камера на сцене.
    /// </summary>
    [SerializeField] private CameraController cameraController = null;

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
    /// Максимальное количество снарядов, доступное в игре.
    /// </summary>
    private readonly int maxShellsCount = 5;

    /// <summary>
    /// Снаряды.
    /// </summary>
    public GameObject[] Shells
    {
        get => shells;
    }

    /// <summary>
    /// Количество снарядов в зависимости от выбранного оружия.
    /// </summary>
    public int CurrentShellsCount
    {
        get
        {
            int count = 1;
            switch (currentWeapon)
            {
                case 1:
                case 2:
                    count = 1;
                    break;

                case 3:
                case 4:
                    count = 5;
                    break;

                default:
                    count = 1;
                    break;
            }

            return count;
        }
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

        Vector3 cameraDirection = cameraController.gameObject.transform.forward;
        for (int i = 0; i < CurrentShellsCount; i++)
        {
            shells[i] = Instantiate<GameObject>(shellPrefab);
            shells[i].transform.position = cameraController.CameraOrigin;
            shells[i].GetComponent<Rigidbody>().AddForce(cameraDirection * shotForce, ForceMode.Impulse);
            shells[i].GetComponent<ShellCollisionDetector>().OnTargetCollision += OnTargetCollisionEventHandler;
            shells[i].GetComponent<ShellCollisionDetector>().OnTargetCollision += SceneController.Instance.ScorePoints;
            shells[i].GetComponent<ShellCollisionDetector>().OnLongCollision += OnLongCollisionEventHandler;
        }

        // Переключить камеру в режим следования за снарядом.
        cameraController.ChangeToChase();
    }

    /// <summary>
    /// Обработчик события столкновения снаряда с мишенью.
    /// </summary>
    private void OnTargetCollisionEventHandler(Color color)
    {
        // Определить, столкнулся ли снаряд с мишенью или чем-то другим.
        bool isTarget = false;
        // Перебрать цвета мишени.
        foreach (Color targetColor in settings.TargetsColors)
        {
            // Если цвет поверхности столкновения совпадает с любым цветом мишени, значит столкнулись с мишенью.
            if (color == targetColor)
            {
                isTarget = true;
            }
        }

        // Если столкновение произошло не с мишенью, завершить функцию.
        if (!isTarget)
        {
            return;
        }

        // Удалить снаряды со сцены и показать мишень на несколько секунд.
        StartCoroutine(cameraController.ShowTargetForTime());
        StartCoroutine(RemoveShells());
    }

    /// <summary>
    /// Обработчкик события слишком долгого соприкосновения снаряда с поверхностью.
    /// </summary>
    /// <param name="time"></param>
    private void OnLongCollisionEventHandler(float time)
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
            currentPosition.y < 0 || currentPosition.z > fieldSizes[1] ||
            currentPosition.z < 0)
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
                shell.GetComponent<ShellCollisionDetector>().OnTargetCollision -= OnTargetCollisionEventHandler;
                shell.GetComponent<ShellCollisionDetector>().OnLongCollision -= OnLongCollisionEventHandler;
                shell.GetComponent<ShellCollisionDetector>().OnTargetCollision -= SceneController.Instance.ScorePoints;
                ;
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
        Debug.Log("Выбрано оружие " + weapon);
    }
}