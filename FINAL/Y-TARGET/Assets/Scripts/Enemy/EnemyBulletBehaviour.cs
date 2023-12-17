using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{

    public GameObject user;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Enemy")
        {
            Destroy(this.gameObject);
        }

        //Destroy(this.gameObject);

        //Physics.IgnoreLayerCollision(bulletLayer, bulletLayer);
    }
}
