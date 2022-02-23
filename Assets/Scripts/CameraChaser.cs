using UnityEngine;

/// <summary>
/// Слежение камеры за летящим снарядом.
/// </summary>
public class CameraChaser : MonoBehaviour
{
    /// <summary>
    /// Оружие.
    /// </summary>
    [SerializeField] private WeaponController weapon = null;

    /// <summary>
    /// Возможность слежения за летящим снарядом.
    /// </summary>
    private bool canChase = false;

    /// <summary>
    /// Режим слежения за летящим снарядом.
    /// </summary>
    public bool CanChase
    {
        get => canChase;
        set => canChase = value;
    }

    private void Update()
    {
        GameObject shell = weapon.Shells[0];
        // Если камера может преследовать снаряд в результате выстрела, переместить ее.
        if (shell != null && canChase)
        {
            // Положение летящего снаряда.
            Vector3 shellPosition = shell.transform.position;
            // Величина смещения камеры относитльно летящего сняряда (сзади, правее и выше).
            Vector3 cameraOffset = new Vector3(3f, 5f, -9f);
            // transform.position = shell.transform.position + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, shellPosition + cameraOffset, 0.9f);
            transform.LookAt(shellPosition);
        }
    }
}