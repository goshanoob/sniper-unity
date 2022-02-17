using UnityEngine;

/// <summary>
/// Куб мишени.
/// </summary>
public class TargetsCube : MonoBehaviour
{
    private int cost = 70;
    private Color color = Color.white;
    

    public int Cost
    {
        set
        {
            cost = value;
        }
    }
    
    public Color Color
    {
        set
        {
            color = value;
        }
    }
}
