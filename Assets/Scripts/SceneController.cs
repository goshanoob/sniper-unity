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
    /// Размер снаряда.
    /// </summary>
    [SerializeField] float shellSize = 0.2f;
    
    /// <summary>
    /// Количестов кубов в мишени.
    /// </summary>
    private const int cubeCount = 49;
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 0;

    private GameObject shell = null;
    
    private void Start()
    {
        currentLevel = GameSettings.currentLevel;
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

        if (click && shell == null)
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
    private void Shoot()
    {
        Vector3 cameraDirection = mainCamera.transform.forward;
        
        shell = GameObject.CreatePrimitive(PrimitiveType.Cube);
        shell.transform.localScale = new Vector3(shellSize, shellSize, shellSize);
        shell.transform.position = mainCamera.transform.position + cameraDirection * 5f;
        Rigidbody rigidBody = shell.AddComponent<Rigidbody>();
        rigidBody.AddForce(cameraDirection * 10f);
        StartCoroutine(DestroyShell());
    }


    private IEnumerator DestroyShell(){
        yield return new WaitForSeconds(10);
        Destroy(shell);
    }
}
