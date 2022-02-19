using System;
using System.Collections;
using UnityEngine;

public class ShellCollisionDetector : MonoBehaviour
{
    /// <summary>
    /// Снярад столкнулся с поверхностью.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DestroyShell());
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
