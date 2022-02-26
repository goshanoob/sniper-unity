using System;
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
    /// Переключатель возможности куба быть пораженным.
    /// </summary>
    private bool isHitable = true;

    /// <summary>
    /// Стоимость попадания в куб.
    /// </summary>
    /// <exception cref="Exception">Стоимость не может быть отрицательна.</exception>
    public float Cost
    {
        get
        {
            // Если в куб ранее не попадали, то вернуть его стоимость и отключить возможность повторного начисления очков за него,
            // иначе - вернуть 0.
            if (isHitable)
            {
                isHitable = false;
                return cost;
            }
            else
            {
                return 0;
            }
        }
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