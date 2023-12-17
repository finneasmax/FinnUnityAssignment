using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Weapon
{
    public override void Use(Vector3 target,Transform origin, GameObject user)
    {
        Vector3 frontOfOrigin = (origin.transform.forward * 3);
        GameObject newBullet = Instantiate(bullet, origin.position + frontOfOrigin, Quaternion.Euler(0, 0, 0));

        BulletBehaviour bulletBehaviour = newBullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.user = user;

        Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();

        BulletRB.velocity = (target-newBullet.transform.position).normalized * speed;
        //Debug.Log($"{(target-(origin.position + frontOfOrigin)).normalized}");
    }
}
