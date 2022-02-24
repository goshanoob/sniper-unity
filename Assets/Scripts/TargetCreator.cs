using UnityEngine;

/// <summary>
/// Генератор мишени.
/// </summary>
public class TargetCreator : MonoBehaviour
{
    /// <summary>
    /// Настройки игры.
    /// </summary>
    [SerializeField] private GameSettings settings = null;

    /// <summary>
    /// Масса куба мишени.
    /// </summary>
    [SerializeField] private float cubeTargetMass = 0.5f;

    /// <summary>
    /// Количество кубов в мишени.
    /// </summary>
    private const int cubeCount = 49;
    
    /// <summary>
    /// Сформировать мишень.
    /// </summary>
    public void CreateTarget()
    {
        GameObject target = new GameObject("Target");
        target.transform.position = new Vector3(0, 0, settings.TargetDistance);
        Color[] colors = settings.TargetsColors;

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
                cubeTarget.transform.parent = target.transform;
                cubeTarget.transform.localPosition = new Vector3(offsetX, offsetY, 0);

                Material cubeMaterial = cubeTarget.GetComponent<Renderer>().materials[0];

                if (j == 0 || j == 6 || i == 0 || i == 6)
                {
                    cubeMaterial.color = colors[0];
                }

                if (j >= 1 && j <= 5 && i >= 1 && i <= 5)
                {
                    cubeMaterial.color = colors[1];
                }

                if (j >= 2 && j <= 4 && i >= 2 && i <= 4)
                {
                    cubeMaterial.color = colors[2];
                }

                if (j >= 3 && j <= 3 && i >= 3 && i <= 3)
                {
                    cubeMaterial.color = colors[3];
                }

                offsetX += cubeSize;
            }

            offsetY += cubeSize;
            offsetX = -columnCount / 2.0f;
        }
    }
}