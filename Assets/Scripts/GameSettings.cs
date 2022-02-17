using UnityEngine;

/// <summary>
/// Настройки игры.
/// </summary>
public class GameSettings : MonoBehaviour
{
    /// <summary>
    /// Расстояния до мишени в зависимости от уровня игры.
    /// </summary>
    public static float[] targetDistances = new float[] { 60f, 70f, 80f, 90f, 100f };
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    public static int  currentLevel = 1;

    /// <summary>
    /// Теккущий уровень игры.
    /// </summary>
    public int CurrentLevel
    {
        get;
        set;
    } = 1;
}
