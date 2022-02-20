using UnityEngine;

public class CameraChaser : MonoBehaviour
{
    [SerializeField] private GameObject shell = null;

    private bool canChase = false;

    private void Update()
    {
        if (canChase)
        {
            var cameraOffset = new Vector3(-5f, 5f, -10f);
            transform.position = shell.transform.position + cameraOffset;
            

            if (transform.position.z >= 50f)
            {
               canChase = false;
            }
        }
    }

    private void OnShot()
    {
        canChase = true;
    }
}