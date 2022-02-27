using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Куб мишени.
/// </summary>
public class TargetsCube : MonoBehaviour
{
    /// <summary>
    /// Стоимость попадания в куб.
    /// </summary>
    private float cost = 70;

    /// <summary>
    /// Стоимость попадания в куб.
    /// </summary>
    /// <exception cref="Exception">Стоимость не может быть отрицательна.</exception>
    public float Cost
    {
        get => cost;
        set
        {
            if (value < 0)
            {
                throw new Exception("Стоимость попадания в мишень не может быть отрицательна");
            }

            cost = value;
        }
    }
}