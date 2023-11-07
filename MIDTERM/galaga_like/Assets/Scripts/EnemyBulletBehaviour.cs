using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : BulletBehaviour
{
    //private AudioSource QuickShotSource;

    //[SerializeField] AudioClip QuickShotSound;

    public EnemyBulletBehaviour()
    {
        _yDirection = -1;
    }

    //private void Awake()
    //{
    //    QuickShotSource = this.GetComponent<AudioSource>();
    //    QuickShotSource.volume = 1f;
    //    QuickShotSource.clip = QuickShotSound;
    //    QuickShotSource.Play();
    //}

    private void Start()
    {
        damage = 10;
        _yDirection = -1;
        vector = new Vector3(0, _yDirection, 0);
    }


    void Update()
    {
        if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
        {
            BulletTravel();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(this.transform.gameObject);
        }
    }
}
