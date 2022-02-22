using System;
using System.Collections;
using UnityEngine;

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
    /// Настройки оружия.
    /// </summary>
    [SerializeField] private WeaponController weapon = null;
    /// <summary>
    /// Масса куба мишени.
    /// </summary>
    [SerializeField] private float cubeTargetMass = 0.2f;
    /// <summary>
    /// Камера.
    /// </summary>
    [SerializeField] private Camera mainCamera = null;
    
    /// <summary>
    /// Количество кубов в мишени.
    /// </summary>
    private const int cubeCount = 49;
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 0;
    

    private int shellsCountInMode = 1;

    private bool canChase = false;

    private float timer = 0;
    private bool isForward = true;
    private float x1 = -2f * Mathf.Sqrt(2);
    private float dx = 0.2f;
    private float a = 2f;
    
    
    
    private void Start()
    {
        currentLevel = settings.CurrentLevel;
        CrateTarget();
    }
    
    private void Update()
    {
        bool space = Input.GetKey(KeyCode.Space);
        bool click = Input.GetMouseButtonDown(0);
        if (space)
        {
            mainCamera.transform.position = new Vector3(0, 2, settings.TargetDistance - 10);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, 2, 0);
        }

        // Если нажата кнопка мыши и на сцене отсутсвуют снаряды, выстрелить.
        if (click && weapon.Shells[0] == null)
        {
            weapon.CrateShells();
            canChase = true;
        }
        
        
        if (canChase)
        {
            Vector3 cameraOffset = new Vector3(-5f, 5f, -10f);
            mainCamera.transform.position = weapon.Shells[0].transform.position + cameraOffset;
            mainCamera.transform.transform.LookAt(weapon.Shells[0].transform);
            if (Mathf.Abs((transform.position - mainCamera.transform.position).magnitude) >= 50f)
            {
                canChase = false;
            }
        }
        
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            
            timer = 0;
            if (x1 > 2f * Mathf.Sqrt(2) || x1 < -2f * Mathf.Sqrt(2) )
            {
                dx = -dx;
            }
            float y = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x1 * x1 * a * a) - x1 * x1 - a * a);
            if (!isForward)
            {
                y = -y;
            }

            mainCamera.transform.position =
                Vector3.Lerp(mainCamera.transform.position, new Vector3(x1, y, 0), Time.deltaTime*10);
            x1 += dx;
            
        }
    }

    /// <summary>
    /// Сформировать мишень.
    /// </summary>
    private void CrateTarget()
    {
        GameObject target = new GameObject("Target");
        target.transform.position = new Vector3(0, 0, settings.TargetDistance);
        Color[] colors = new Color[] {Color.white, Color.blue, Color.red, Color.yellow};
        
        int columnCount = 7;
        int rowCount = 7;
        float offsetX = -columnCount / 2.0f;
        float offsetY = 0.5f;
        float cubeSize = 1f;

        for (int i = 0; i < rowCount; i++)
        {
            int endNumber = rowCount - 1 - i;
            for (int j = 0; j < columnCount; j++)
            {
                GameObject cubeTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeTarget.transform.localScale = Vector3.one;
                Rigidbody cubeTargetBody = cubeTarget.AddComponent<Rigidbody>();
                cubeTargetBody.mass = cubeTargetMass;
                cubeTarget.transform.localPosition = new Vector3(offsetX, offsetY, 0);
                cubeTarget.transform.parent = target.transform;
                    
                Material cubeMaterial = cubeTarget.GetComponent<Renderer>().materials[0];
                
                if (j == 0 || j == 6 || i == 0 || i == 6)
                {
                    cubeMaterial.color = Color.white;
                }
                if (j >= 1 && j <= 5 && i >= 1 && i <= 5)
                {
                    cubeMaterial.color = Color.blue;
                }
                if (j >= 2 && j <= 4 && i >= 2 && i <= 4)
                {
                    cubeMaterial.color = Color.red;
                }
                if (j >= 3 && j <= 3 && i >= 3 && i <= 3)
                {
                    cubeMaterial.color = Color.yellow;
                }
                offsetX += cubeSize;
            }
            offsetY += cubeSize;
            offsetX = -columnCount / 2.0f;
        }
    }
}
