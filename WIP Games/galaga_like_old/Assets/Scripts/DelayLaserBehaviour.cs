using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayLaserBehaviour : EnemyBulletBehaviour
{
    private Transform _target;

    void Start()
    {
        _target = GameObject.Find("Player").transform;
    }


    void Update()
    {
        vector = _target.position.normalized;
        BulletTravel();
        Debug.LogFormat("{0}",vector);
    }
}