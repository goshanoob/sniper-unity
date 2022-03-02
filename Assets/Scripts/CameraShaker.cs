using System;
using UnityEngine;

/// <summary>
/// Качание камеры.
/// </summary>
public class CameraShaker : MonoBehaviour
{
    /// <summary>
    /// Максимальная координата отклонения по горизонтальной оси.
    /// </summary>
    [SerializeField] private float maxDeviationX = 2f;

    /// <summary>
    /// Максимальный угол отклонения по вертикальной оси.
    /// </summary>
    [SerializeField] private float maxDeviationY = 10f;

    /// <summary>
    /// Возможность качания камеры при прицеливании.
    /// </summary>
    private bool canShake = true;

    /// <summary>
    /// Текущий режим качания камеры.
    /// </summary>
    private int currentShakeMode = 0;

    /// <summary>
    /// Доступные скорости качания камеры в соответствии с режимами качания.
    /// </summary>
    private float[] shakesSpeeds = new float[] { 0.04f, 0.02f };

    /// <summary>
    /// Текущая величина поворота камеры вокруг горизонтальной оси.
    /// </summary>
    private float xRotation = 0;

    /// <summary>
    /// Аргументы функции лемнискаты Бернулли.
    /// </summary>
    private float[] x = null;

    /// <summary>
    /// Значения функции лемнискаты Бернулли.
    /// </summary>
    private float[] y = null;

    /// <summary>
    /// Направление вращения камеры.
    /// </summary>
    private int shakeDirection = 1;

    /// <summary>
    /// Текущий индекс точки лемнискаты.
    /// </summary>
    private int currentPointValue = 0;

    /// <summary>
    /// Счетчик времени.
    /// </summary>
    private float timer = 0;

    /// <summary>
    /// Скорость качания камеры.
    /// </summary>
    public int ShakeMode
    {
        get => currentShakeMode;
        set
        {
            if (value >= shakesSpeeds.Length)
            {
                throw new Exception("Выбрана несуществующая скорость качания камеры.");
            }

            currentShakeMode = value;
        }
    }

    /// <summary>
    /// Возможно ли качание камеры.
    /// </summary>
    public bool CanShake
    {
        get => canShake;
        set => canShake = value;
    }

    private void Start()
    {
        // Вычислить координаты перемещения камеры по линии лемнискаты.
        GetLemniscatePoints();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (canShake && timer >= shakesSpeeds[currentShakeMode])
        {
            timer = 0;
            MakeShake();
        }
    }

    /// <summary>
    /// Получить координаты кривой лемнискаты Бернулли.
    /// </summary>
    private void GetLemniscatePoints()
    {
        // Фокусное расстояние.
        float a = maxDeviationX * Mathf.Sqrt(2) / 2;
        // Приращение аргумента функции.
        float dx = 0.2f;
        int valuesCount = 2 * Mathf.CeilToInt(maxDeviationX / dx) + 1;
        this.x = new float[2 * valuesCount];
        this.y = new float[2 * valuesCount];
        float x = -maxDeviationX;
        bool isIncrementX = true;
        int direction = 1;
        string result = "";
        for (int i = 0; i < 2 * valuesCount; i++)
        {
            float y = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x * x * a * a) -
                                 x * x - a * a);
            // При переходе через начало координат функция меняет знак.
            if (isIncrementX && x > 0 || !isIncrementX && x < 0)
            {
                y = -y;
            }

            this.x[i] = x;
            this.y[i] = y;
            result += $"{this.x[i]};{this.y[i]}\n";
            // Изменить направление приращения аргумента.
            if (i == valuesCount - 1)
            {
                direction = -1;
                isIncrementX = false;
            }

            x += dx * direction;
        }

        Debug.Log(result);
    }

    /// <summary>
    /// Выполнить качание камеры по кривой лемнискаты на один шаг.
    /// </summary>
    private void MakeShake()
    {
        float distance = 50f;
        Vector3 nextPosition = new Vector3(this.x[currentPointValue], this.y[currentPointValue], distance);
        transform.LookAt(nextPosition);
        currentPointValue++;
        if (currentPointValue >= this.x.Length)
        {
            currentPointValue = 0;
        }
    }

    /// Другой вариант качания.
    /// <summary>
    /// Выполнить качание камеры вверх-вниз на один шаг.
    /// </summary>
    private void MakeEasyShake()
    {
        float newRotation = shakeDirection * shakesSpeeds[currentShakeMode] * Time.deltaTime;
        xRotation += newRotation;
        transform.localEulerAngles = new Vector3(xRotation, 0, 0);
        // Если камера отклонилась на максимальный угол, изменить направление вращения.
        if (Mathf.Abs(xRotation) >= maxDeviationY)
        {
            xRotation -= newRotation;
            shakeDirection *= -1;
        }
    }
}