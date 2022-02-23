using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Определитель столкновений снаряда с поверхностями.
/// </summary>
public class ShellCollisionDetector : MonoBehaviour
{
    private int points = 0;

    /// <summary>
    /// Столкновение с кубом мишени.
    /// </summary>
    public event Action OnTargetCollision;
    
    /// <summary>
    /// Снярад столкнулся с поверхностью.
    /// </summary>
    /// <param name="collision">Поверхность столкновения.</param>
    private void OnCollisionEnter(Collision collision)
    {
        Color color = collision.gameObject.GetComponent<Renderer>().materials[0].color;
        if (color == Color.white)
        {
            points += 70;
        }
        else if (color == Color.blue)
        {
            points += 80;
        }
        else if (color == Color.red)
        {
            points += 90;
        }
        else if (color == Color.yellow)
        {
            points += 100;
        }
        else
        {
            return;
        }
        Debug.Log(points);
        OnTargetCollision();
    }

    /// <summary>
    /// Уничтожить снаряд.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyShell(){
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
