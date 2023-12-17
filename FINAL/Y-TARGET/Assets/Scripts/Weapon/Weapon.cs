using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Damage;
    public GameObject bullet;
    public float speed;
    private GameObject _user;

    public virtual void Use(Vector3 target, Transform origin, GameObject user)
    {

    }
}
