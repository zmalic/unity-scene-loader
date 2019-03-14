using UnityEngine;
using System.Collections;

public class SimpleCameraOrbit : MonoBehaviour
{
    public float horisontalSpeed = 2.5f;
    public float verticalSpeed = 2.5f;
    
    float x = 0.0f;
    float y = 0.0f;

    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * horisontalSpeed;
        y -= Input.GetAxis("Mouse Y") * verticalSpeed;
        
        transform.rotation = Quaternion.Euler(y, x, 0);
    }
}