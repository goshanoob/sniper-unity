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
        // Если камера может преследовать снаряд в результате выстрела, переместить ее.
        if (canChase)
        {
            // Положение летящего снаряда.
            Vector3 shellPosition = weapon.Shells[0].transform.position;
            // Величина смещения камеры относитльно летящего сняряда (сзади, правее и выше).
            Vector3 cameraOffset = new Vector3(5f, 5f, -10f);
            // transform.position = shell.transform.position + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, shellPosition + cameraOffset, Time.deltaTime);
            transform.LookAt(shellPosition);
        }
    }
}