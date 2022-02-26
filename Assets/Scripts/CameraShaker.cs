using UnityEngine;

/// <summary>
/// Качание камеры. Класс нужно капительно переписать.
/// </summary>
public class CameraShaker : MonoBehaviour
{
    /// <summary>
    /// Фокальный радиус лемнискаты.
    /// </summary>
    [SerializeField] private float a = 2f;

    /// <summary>
    /// Возможность качания камеры при прицеливинии.
    /// </summary>
    private bool canShake = true;

    private float[] x = null;
    private float[] y = null;

    private bool isForward = true;
    private float x1 = -2f * Mathf.Sqrt(2);
    private float dx = 0.2f;

    private float timer = 0;


    private Vector3 origin = Vector3.zero;
    private Vector3 nextPoint = Vector3.zero;

    public bool CanShake
    {
        get => canShake;
        set => canShake = value;
    }

    private void Start()
    {
        float squere = a * Mathf.Sqrt(2);
        float x = -squere;
        float dx = 0.2f;
        int valuesCount = Mathf.CeilToInt(2.0f * squere / dx);
        this.x = new float[2 * valuesCount];
        this.y = new float[2 * valuesCount];
        string result = "";

        for (int i = 0; i < valuesCount; i++)
        {
            this.x[i] = x;
            float tmpY = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x * x * a * a) - x * x - a * a);
            if (x > 0)
            {
                tmpY *= -1;
            }

            this.y[i] = tmpY;
            x += dx;

            result += $"{this.x[i]};{this.y[i]}\n";
        }

        for (int i = 0; i < valuesCount; i++)
        {
            this.x[valuesCount + i] = this.x[valuesCount - i];
            this.y[valuesCount + i] = -this.y[valuesCount - i];
            result += $"{this.x[i]};{this.y[i]}\n";
        }

        // Debug.Log(result);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.03f && canShake)
        {
            timer = 0;
            if (x1 > 2f * Mathf.Sqrt(2) || x1 < -2f * Mathf.Sqrt(2))
            {
                dx = -dx;
            }


            float y = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x1 * x1 * a * a) - x1 * x1 - a * a);
            if (!isForward)
            {
                y = -y;
            }


            nextPoint = new Vector3(x1, y, 50f);
            float different = (nextPoint - origin).magnitude;
            transform.LookAt(nextPoint);
            x1 += dx;
        }
    }

    private void MakeShake()
    {
        // Попытка качания по лемнискате Бернулли.
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            timer = 0;
            if (x1 > 2f * Mathf.Sqrt(2) || x1 < -2f * Mathf.Sqrt(2))
            {
                dx = -dx;
            }

            float y = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x1 * x1 * a * a) - x1 * x1 - a * a);
            if (!isForward)
            {
                y = -y;
            }

            transform.position =
                Vector3.Lerp(transform.position, new Vector3(x1, y, 0), Time.deltaTime * 10);
            x1 += dx;
        }
    }
}