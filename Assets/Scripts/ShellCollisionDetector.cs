using System;
using UnityEngine;

/// <summary>
/// Определитель столкновений снаряда с поверхностями.
/// </summary>
public class ShellCollisionDetector : MonoBehaviour
{
    /// <summary>
    /// Снаряд доступен для столкновения.
    /// </summary>
    private bool canCollised = false;
    
    /// <summary>
    /// Событие столкновения с кубом мишени.
    /// </summary>
    public event Action<float> OnTargetCollision;
    
    /// <summary>
    /// Событие остановки снаряда.
    /// </summary>
    public event Action OnStopped;

    private void Update()
    {
        // Если вектор скорости близок к нулевому, вызвать событие остановки снаряда.
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < Mathf.Epsilon)
        {
            OnStopped();
        }
    }

    /// <summary>
    /// Снаряд столкнулся с поверхностью.
    /// </summary>
    /// <param name="collision">Поверхность столкновения.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Если снаряд еще ни с чем не сталкивался, вызвать событие столкновения.
        if (!canCollised)
        {
            // Вернуть стоимость попадания в куб мишени.
            float cost = 0;
            TargetsCube targetsCube = collision.gameObject.GetComponent<TargetsCube>();
            // Если столкновение произошло не с мишенью, стоимость попадания равна 0.
            if (targetsCube != null)
            {
                cost = targetsCube.Cost;
            }
            canCollised = true;
            OnTargetCollision(cost);
        }
    }
}
