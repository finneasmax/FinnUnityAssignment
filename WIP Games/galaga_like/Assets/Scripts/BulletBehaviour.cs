using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float damage = 1;
    public float speed = 10;
    public static float OnScreenDelay = 2f;
    public string bulletname = "Bullet";
    public int _yDirection = 1;
    public Vector3 vector;

    void Update()
    {
        BulletTravel();
    }


    public virtual void BulletTravel()
    {
        vector = new Vector3(0, _yDirection, 0);
        transform.position += vector * speed * Time.deltaTime;
        if (Mathf.Abs(this.transform.position.y) >= 5)
        {
            //Debug.LogFormat("Bullet Speed: {0}", speed);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something Hit");
        Destroy(this.transform.gameObject);
    }
}
