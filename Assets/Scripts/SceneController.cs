using System;
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
    /// Количестов кубов в мишени.
    /// </summary>
    private const int cubeCount = 49;
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 0;
    
    private void Start()
    {
        currentLevel = GameSettings.currentLevel;
        CrateTarget();
    }
    
    private void Update()
    {
        bool space = Input.GetKey(KeyCode.Space);
        bool click = Input.GetMouseButton(0);
        if (space)
        {
            mainCamera.transform.position = new Vector3(0, 2, GameSettings.targetDistances[currentLevel - 1] - 10);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, 2, 0);
        }

        if (click)
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
            
        }
    }
    
    /// <summary>
    /// Выстрелить по мишени.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Shoot()
    {
        throw new NotImplementedException();
    }
}
