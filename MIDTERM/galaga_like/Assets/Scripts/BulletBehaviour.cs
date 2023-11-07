using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float damage = 1;
    public float speed = 10;
    public static float OnScreenDelay = 2f;
    public string bulletname = "Bullet";
    public int _yDirection;
    public Vector3 vector;


    private void Start()
    {
        speed = 10;
        damage = 1;
    }

    void Update()
    {
        if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
        {
            BulletTravel();
        }
    }


    public void setSpeed(float value)
    {
        this.speed = value;
    }

    public void setVector(Vector3 value)
    {
        this.vector = value;
    }


    public virtual void BulletTravel()
    {
        transform.position += this.vector * speed * Time.deltaTime;
        if (Mathf.Abs(this.transform.position.y) >= Screen.height/100)
        {
            //Debug.LogFormat("Bullet Speed: {0}", speed);
            Destroy(this.gameObject);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Something Hit");
        Destroy(this.transform.gameObject);
    }
}
