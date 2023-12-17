using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject user;
    public GameObject detonation;

    public void Awake()
    {
        Destroy(this.gameObject, 5);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != user)
        {
            GameObject newDet = Instantiate(detonation, transform.position, Quaternion.Euler(0, 0, 0));

            DetonationBehaviour detonationBehaviour = newDet.GetComponent<DetonationBehaviour>();
            detonationBehaviour.user = user;
            detonationBehaviour.size = 10f;
            Destroy(this.gameObject);
        }
    }
}
