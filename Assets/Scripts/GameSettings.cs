using UnityEngine;

/// <summary>
/// Настройки игры.
/// </summary>
public class GameSettings : MonoBehaviour
{
    /// <summary>
    /// Ссылка на единственный экземпляр данного класса.
    /// </summary>
    private static GameSettings instance = null;
    /// <summary>
    /// Возможные расстояния до мишени в соответствии с уровнянми игры..
    /// </summary>
    private float[] targetDistances = new float[] { 60f, 70f, 80f, 90f, 100f };
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    private int currentLevel = 1;

    /// <summary>
    /// Настройки игры.
    /// </summary>
    public static GameSettings Instance
    {
        get => instance;
    }
    
    /// <summary>
    /// Расстояние до мишени в зависимости от текущего уровня игры.
    /// </summary>
    public float TargetDistance
    {
        get
        {
            return targetDistances[currentLevel - 1];
        }
    }
    
    /// <summary>
    /// Текущий уровень игры.
    /// </summary>
    public int CurrentLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    } 

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance == this)
        {
            Destroy(gameObject);
        }
    }
}