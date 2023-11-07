using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQuickBulletBehaviour : EnemyBulletBehaviour
{
    private void Start()
    {
        this.speed = 20;
        this._yDirection = -1;

    }

    private void Update()
    {
        BulletTravel();
    }
}