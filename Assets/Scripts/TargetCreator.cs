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
    /// Мишень.
    /// </summary>
    private GameObject target = null;

    /// <summary>
    /// Сформировать мишень.
    /// </summary>
    public void CreateTarget()
    {
        int columnCount = 7;
        int rowCount = 7;
        float offsetX = -columnCount / 2.0f;
        float offsetY = 0.5f;
        float cubeSize = 1f;
        Color[] colors = settings.TargetsColors;
        float[] points = settings.PointsRange;

        target = new GameObject("Target");
        target.transform.position = new Vector3(0, 0, settings.TargetDistance);
        for (int i = 0; i < rowCount; i++)
        {
            // int endNumber = rowCount - 1 - i;
            for (int j = 0; j < columnCount; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Rigidbody cubeTargetBody = cube.AddComponent<Rigidbody>();
                TargetsCube targetsCube = cube.AddComponent<TargetsCube>();
                Material cubeMaterial = cube.GetComponent<Renderer>().materials[0];

                cube.transform.localScale = Vector3.one;
                cube.transform.parent = target.transform;
                cube.transform.localPosition = new Vector3(offsetX, offsetY, 0);
                cubeTargetBody.mass = cubeTargetMass;

                if (j == 0 || j == rowCount - 1 || i == 0 || i == rowCount - 1)
                {
                    cubeMaterial.color = colors[0];
                    targetsCube.Cost = points[0];
                }

                if (j >= 1 && j <= rowCount - 2 && i >= 1 && i <= rowCount - 2)
                {
                    cubeMaterial.color = colors[1];
                    targetsCube.Cost = points[1];
                }

                if (j >= 2 && j <= rowCount - 3 && i >= 2 && i <= rowCount - 3)
                {
                    cubeMaterial.color = colors[2];
                    targetsCube.Cost = points[2];
                }

                if (j >= 3 && j <= rowCount - 4 && i >= 3 && i <= rowCount - 4)
                {
                    cubeMaterial.color = colors[3];
                    targetsCube.Cost = points[3];
                }

                offsetX += cubeSize;
            }

            offsetY += cubeSize;
            offsetX = -columnCount / 2.0f;
        }
    }

    /// <summary>
    /// Удалить мишень.
    /// </summary>
    public void RemoveTarget() => Destroy(target);
}