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
    private bool canCollised = true;

    /// <summary>
    /// Событие столкновения с кубом мишени.
    /// </summary>
    public event Action<GameObject, float> OnTargetCollision;

    /// <summary>
    /// Событие промаха.
    /// </summary>
    public event Action<GameObject> OnMissed;

    /// <summary>
    /// Снаряд столкнулся с поверхностью.
    /// </summary>
    /// <param name="collision">Поверхность столкновения.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Если снаряд еще ни с чем не сталкивался, вызвать событие столкновения.
        if (canCollised)
        {
            TargetsCube targetsCube = collision.gameObject.GetComponent<TargetsCube>();
            // Если столкновение произошло с мишенью, сгенерировать событие попадания, иначе - событие промаха.
            if (targetsCube != null)
            {
                // Передать стоимость попадания в куб мишени.
                OnTargetCollision?.Invoke(gameObject, targetsCube.Cost);
            }
            else
            {
                OnMissed?.Invoke(gameObject);
            }

            canCollised = false;
        }
    }
}