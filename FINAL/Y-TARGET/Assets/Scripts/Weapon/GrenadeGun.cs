using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : Weapon
{
    private bool _ready = true;
    private bool _recoiling=false;
    public float recoil = 5;

    public override void Use(Vector3 target, Transform origin, GameObject user)
    {
        if(!_recoiling)
        {
            Vector3 frontOfOrigin = (origin.transform.forward * 3);
            GameObject newGrenade = Instantiate(bullet, origin.position + frontOfOrigin, Quaternion.Euler(0, 0, 0));

            GrenadeBehaviour grenadeBehaviour = newGrenade.GetComponent<GrenadeBehaviour>();
            grenadeBehaviour.user = user;

            Rigidbody BulletRB = newGrenade.GetComponent<Rigidbody>();

            BulletRB.velocity = (target - transform.position).normalized * speed;

            //StartCoroutine(Recoil());
            //_recoiling = true;
        }
    }

    IEnumerator Recoil()
    {
        yield return new WaitForSeconds(recoil);
        _recoiling = false;
    }
}
