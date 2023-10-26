using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : BulletBehaviour
{

    private void Start()
    {
        this.damage = 10;
        this._yDirection = -1;
    }


    void Update()
    {
        BulletTravel();
    }
}
