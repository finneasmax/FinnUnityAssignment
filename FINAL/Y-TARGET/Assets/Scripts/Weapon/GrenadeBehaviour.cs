using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : BulletBehaviour
{
    private Material _mat;
    private bool _collidedWith;

    //private void Update()
    //{
    //    Rigidbody RB = GetComponent<Rigidbody>();

    //    RB.velocity -= new Vector3(0,10,0)*Time.deltaTime;
    //}

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != user && collision.gameObject.layer != 8)
        {
            _collidedWith=true;
            GameObject newDet=Instantiate(detonation,transform.position,Quaternion.Euler(0,0,0));

            DetonationBehaviour detonationBehaviour = newDet.GetComponent<DetonationBehaviour>();
            detonationBehaviour.user = user;
            detonationBehaviour._addImpulse = true;

            Destroy(this.gameObject);
            Debug.Log($"{user}");
            Debug.Log($"{collision.gameObject.name}");
        }
    }
}
