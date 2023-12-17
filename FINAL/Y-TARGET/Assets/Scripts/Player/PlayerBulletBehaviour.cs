using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public LayerMask bulletLayer;

    public GameObject user;

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.name != "Player")
        //{
        //    Destroy(this.gameObject);
        //}

        Destroy(this.gameObject);
        
    }
}
