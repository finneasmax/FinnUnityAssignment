using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeLaserBehaviour : EnemyBulletBehaviour
{

   
    void Update()
    {
        BulletTravel();
    }

    public override void BulletTravel()
    {
        base.BulletTravel();

        if (transform.localScale.y < 0.7)
        {
            transform.localScale += new Vector3(0, 1, 0) * Time.deltaTime * 0.9f;
        }
    }
}
