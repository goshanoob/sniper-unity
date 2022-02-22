using UnityEngine;

/// <summary>
/// Вращение камеры вокруг двух осей для осматривания.
/// </summary>
public class CameraRotator : MonoBehaviour
{
    /// <summary>
    /// Чувствительность камеры к движениям мыши.
    /// </summary>
    [SerializeField] private float sensitivity = 5f;

    /// <summary>
    /// Максимальный угол вращения камеры при наклоне вверх.
    /// </summary>
    [SerializeField] private float maxVerticalAngle = -90f;

    /// <summary>
    /// Минимальный угол вращения камеры при наклоне вниз.
    /// </summary>
    [SerializeField] private float minVerticalAngle = 90f;

    /// <summary>
    /// Текущий угол поворота камеры вокруг горизонтальной оси.
    /// </summary>
    private float currentXRotation = 0;

    /// <summary>
    /// Текущий угол поворота камеры вокруг вертикальной оси.
    /// </summary>
    private float currentYRotation = 0;

    private void Update()
    {
        // Получить изменения вращения вокруг осей из-за движения мыши.
        float yRotation = Input.GetAxis("Mouse X") * sensitivity;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivity;
        // Вычислить новые текущие значения поворота камеры.
        currentXRotation += xRotation;
        currentYRotation += yRotation;
        // Ограничить величину вращения вокруг горизонтальной оси.
        currentXRotation = Mathf.Clamp(currentXRotation, maxVerticalAngle, minVerticalAngle);
        // Изменить положение камеры.
        transform.localEulerAngles = new Vector3(currentXRotation, currentYRotation, 0);
    }
}