using UnityEngine;

/// <summary>
/// Слежение камеры за летящим снарядом.
/// </summary>
public class CameraChaser : MonoBehaviour
{
    /// <summary>
    /// Экземпляр снаряда, за которым должна двигаться камера.
    /// </summary>
    [SerializeField] private GameObject shell = null;

    /// <summary>
    /// Переключатель возможности слежения за летящим снарядом.
    /// </summary>
    private bool canChase = false;

    private void Update()
    {
        // Если камера может преследовать снаряд в результате выстрела, переместить ее.
        if (canChase)
        {
            // Положение летящего снаряда.
            Vector3 shellPosition = shell.transform.position;
            // Величина смещения камеры относитльно летящего сняряда (сзади, правее и выше).
            Vector3 cameraOffset = new Vector3(5f, 5f, -10f);
            // transform.position = shell.transform.position + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, shellPosition + cameraOffset, Time.deltaTime);
            transform.LookAt(shellPosition);
        }
    }

    private void OnShot()
    {
        canChase = true;
    }
}