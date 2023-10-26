using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBulletBehaviour : BulletBehaviour
{
    void Start()
    {
        this.speed = 20;
    }

    void Update()
    {
        BulletTravel();
    }
}
