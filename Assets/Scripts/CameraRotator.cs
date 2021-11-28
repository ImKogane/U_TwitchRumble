using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{

    public Transform targetPoint;
    public float rotationSpeed;
    
    void Update()
    {
        transform.RotateAround(targetPoint.position, Vector3.up,  rotationSpeed * Time.deltaTime);
    }
}
