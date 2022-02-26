using UnityEngine;

/// <summary>
/// Настройки игры.
/// </summary>
public class GameSettings : MonoBehaviour
{
    /// <summary>
    /// Контроллер сцены.
    /// </summary>
    [SerializeField] private SceneController sceneController = null;

    /// <summary>
    /// Возможные расстояния до мишени в соответствии с уровнями игры.
    /// </summary>
    private readonly float[] targetDistances = new float[] { 60f, 70f, 80f, 90f, 100f };

    /// <summary>
    /// Размеры игрового поля.
    /// </summary>
    private readonly float[] gameFieldSizes = new float[] { 30f, 110f };

    /// <summary>
    /// Количество очков в зависимости от цвета куба мишени.
    /// </summary>
    private readonly float[] pointsRange = new float[] { 70, 80, 90, 100 };

    /// <summary>
    /// Цвета мишени от края к центру.
    /// </summary>
    private readonly Color[] targetsColors = new Color[] { Color.white, Color.blue, Color.red, Color.yellow };

    /// <summary>
    /// Количество уровней.
    /// </summary>
    private readonly int levelCount = 5;

    /// <summary>
    /// Расстояние до мишени в зависимости от текущего уровня игры.
    /// </summary>
    public float TargetDistance
    {
        get { return targetDistances[sceneController.CurrentLevel - 1]; }
    }

    /// <summary>
    /// Размеры игрового поля вдоль осей X и Z.
    /// </summary>
    public float[] GameFieldSizes
    {
        get { return gameFieldSizes; }
    }

    /// <summary>
    /// Количество очков в зависимости от пораженного куба.
    /// </summary>
    public float[] PointsRange
    {
        get { return pointsRange; }
    }

    /// <summary>
    /// Цвета, доступные в мишени.
    /// </summary>
    public Color[] TargetsColors
    {
        get { return targetsColors; }
    }

    /// <summary>
    /// Количество уровней в игре.
    /// </summary>
    public int LevelCount
    {
        get { return levelCount; }
    }
}