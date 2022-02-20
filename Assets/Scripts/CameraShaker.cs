using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float a = 2f;
    private float[] x = null;
    private float[] y = null;
    
    private bool isForward = true;
    private float x1 = -2f * Mathf.Sqrt(2);
    private float dx = 0.2f;
    
    private float timer = 0;
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
            float tmpY =  Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x * x * a * a) - x * x - a * a);
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
            result +=$"{this.x[i]};{this.y[i]}\n";
        }
        Debug.Log(result);
        
    }

    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, playerTrans.position + offset, Time.deltaTime*10);
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            
            timer = 0;
            if (x1 > 2f * Mathf.Sqrt(2) || x1 < -2f * Mathf.Sqrt(2) )
            {
                dx = -dx;
            }
            float y = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(a, 4) + 4 * x1 * x1 * a * a) - x1 * x1 - a * a);
            if (!isForward)
            {
                y = -y;
            }
            transform.position = new Vector3(x1, y, 0);
            x1 += dx;
            
        }
    }
}