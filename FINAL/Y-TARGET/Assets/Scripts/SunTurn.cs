using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTurn : MonoBehaviour
{
    private float RotX;
    public float RotationSpeed = 0.5f;
    
    void Start()
    {
        RotX = transform.rotation.x;
    }

    
    void Update()
    {
        RotX += RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(RotX,-30,0);
    }
}
