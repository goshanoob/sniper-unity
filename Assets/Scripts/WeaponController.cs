using System;
using System.Collections;
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
    [SerializeField] private Camera mainCamera = null;
    /// <summary>
    /// Сила выстрела снарядом.
    /// </summary>
    [SerializeField] private float shotForce = 10f;
    
    /// <summary>
    /// Экземпляр текущего класса WeaponController.
    /// </summary>
    private static WeaponController instance = null;
    /// <summary>
    /// Текущее выбранное оружие.
    /// </summary>
    private int currentWeapon = 1;
    /// <summary>
    /// Сняряды на сцене.
    /// </summary>
    private GameObject[] shells = null;
    /// <summary>
    /// Максимальное количество снарядов, доступное в игре.
    /// </summary>
    private readonly int maxShellsCount = 5;
    
    /// <summary>
    /// Контроллер оружия.
    /// </summary>
    public static WeaponController Instance
    {
        get => instance;
    }

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
        // Если ссылка на данный экземпляр не установлена, присвоить ее.
        // Если экзепляр уже существует, уничтожить текущий игровой объект.
        if (instance == null)
        {
            instance = this;
        }
        else if(instance == this)
        {
            Destroy(gameObject);
        }
        
        // Выделить память для массива снарядов.
        shells = new GameObject[maxShellsCount];
    }
    
    /// <summary>
    /// Создать снаряды.
    /// </summary>
    public void CrateShells()
    {
        Vector3 cameraDirection = mainCamera.transform.forward;
        for (int i = 0; i < CurrentShellsCount; i++)
        {
            shells[i] = Instantiate<GameObject>(shellPrefab);
            shells[i].transform.position = mainCamera.transform.position;
            shells[i].GetComponent<Rigidbody>().AddForce(cameraDirection * shotForce, ForceMode.Impulse);
        }
        // Уничтожить снаряды после создания.
        StartCoroutine(RemoveShells());
    }
    
    /// <summary>
    /// Уничтожить снаряды по истечении времени.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveShells()
    {
        yield return new WaitForSeconds(10); 
        
        foreach (GameObject shell in shells)
        {
            Destroy(shell);
        }
        Array.Clear(shells, 0, shells.Length);
    }
}
