using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер сцены.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Куб мишени.
    /// </summary>
    [SerializeField] private GameObject targetsCube = null;
    /// <summary>
    /// Камера.
    /// </summary>
    [SerializeField] private Camera mainCamera = null;
    /// <summary>
    /// Элемень снаряда.
    /// </summary>
    [SerializeField] private GameObject shellPrefab = null;
    /// <summary>
    /// Сила выстрела снарядом.
    /// </summary>
    [SerializeField] private float shotForce = 10f;
    /// <summary>
    /// Количестов кубов в мишени.
    /// </summary>
    private const int cubeCount = 49;
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 0;
    /// <summary>
    /// Количество снарядов в игре.
    /// </summary>
    private const int shellsCount = 5;

    private int shellsCountInMode = 1;

    
    
    /// <summary>
    /// Сняряды на сцене.
    /// </summary>
    private GameObject[] shellsInScene = null;
    
    private void Start()
    {
        
        currentLevel = GameSettings.currentLevel;
        shellsInScene = new GameObject[shellsCount];
        CrateTarget();
    }
    
    private void Update()
    {
        bool space = Input.GetKey(KeyCode.Space);
        bool click = Input.GetMouseButtonDown(0);
        if (space)
        {
            mainCamera.transform.position = new Vector3(0, 2, GameSettings.targetDistances[currentLevel - 1] - 10);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, 2, 0);
        }

        if (click && shellsInScene[0] == null)
        {
            Shoot();
        }
    }
    
    /// <summary>
    /// Сформировать мишень.
    /// </summary>
    private void CrateTarget()
    {
        GameObject target = new GameObject("Target");
        target.transform.position = new Vector3(0, 0, GameSettings.targetDistances[0]);
        Color[] colors = new Color[4] {Color.white, Color.blue, Color.red, Color.yellow};
        
        int columnCount = 7;
        float offsetX = 0;
        float offsetY = 0.5f;
        for (int i = 0; i < cubeCount; i++)
        {
            GameObject cube = Instantiate<GameObject>(targetsCube, target.transform);
            cube.transform.position = new Vector3(offsetX, offsetY, GameSettings.targetDistances[0]);
            offsetX++;
            if (offsetX > columnCount)
            {
                offsetX = 0;
                offsetY++;
            }

            int rowNumber = i % 7;
            int colNumber = i / 7;
            Material cubeMaterial = cube.GetComponent<Renderer>().materials[0];
            cubeMaterial.color = colors[colNumber % colors.Length];
        }
    }
    
    /// <summary>
    /// Выстрелить по мишени.
    /// </summary>
    private void Shoot()
    {
        CrateShells();
        StartCoroutine(RemoveShells());
    }

    /// <summary>
    /// Добавить сняряды на сцену.
    /// </summary>
    private void CrateShells()
    {
        Vector3 cameraDirection = mainCamera.transform.forward;
        for (int i = 0; i < shellsCountInMode; i++)
        {
            shellsInScene[i] = Instantiate<GameObject>(shellPrefab);
            shellsInScene[i].transform.position = mainCamera.transform.position;
            shellsInScene[i].GetComponent<Rigidbody>().AddForce(cameraDirection * shotForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Уничтожить снаряды после выстрела.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveShells()
    {
        yield return new WaitForSeconds(10); 
        
        foreach (GameObject shell in shellsInScene)
        {
            Destroy(shell);
        }
        Array.Clear(shellsInScene, 0, shellsInScene.Length);
    }
}
