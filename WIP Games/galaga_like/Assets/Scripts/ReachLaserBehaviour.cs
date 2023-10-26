using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachLaserBehaviour : MonoBehaviour
{
    private Transform _shooter;
    private Transform _target;
    private Transform _distance;

    void Start()
    {
        _target = GameObject.Find("Player").transform;
        _shooter = GameObject.Find("Enemy").transform;
    }

    
    void Update()
    {
       
    }
}
