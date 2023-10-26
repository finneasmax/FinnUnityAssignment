using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="Player")
        {
            Debug.Log("Player Detected - ATTACK!!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.name=="Player")
        {
            Debug.Log("Player out of range - Resume patrol");
        }
    }
}
