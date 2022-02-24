using System;
using UnityEngine;

/// <summary>
/// Определитель столкновений снаряда с поверхностями.
/// </summary>
public class ShellCollisionDetector : MonoBehaviour
{
    private bool wasCollised = false;
    
    /// <summary>
    /// Столкновение с кубом мишени.
    /// </summary>
    public event Action<Color> OnTargetCollision;
    
    /// <summary>
    /// Долгое соприкосновение.
    /// </summary>
    public event Action<float> OnLongCollision;

    private void Update()
    {
        // Если вектор скорости близок к нулевому, вызвать событие остановки снаряда.
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < Mathf.Epsilon)
        {
            OnLongCollision(1f);
        }
    }

    /// <summary>
    /// Снаряд столкнулся с поверхностью.
    /// </summary>
    /// <param name="collision">Поверхность столкновения.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!wasCollised)
        {
            Color color = collision.gameObject.GetComponent<Renderer>().materials[0].color;
            wasCollised = true;
            OnTargetCollision(color);
        }
    }
}
