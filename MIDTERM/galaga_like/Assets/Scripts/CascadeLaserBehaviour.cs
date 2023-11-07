using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeLaserBehaviour : EnemyBulletBehaviour
{
    private AudioSource CascadeLaserSource;

    [SerializeField] AudioClip CascadeLaserSound;

    private void Awake()
    {
        CascadeLaserSource = this.GetComponent<AudioSource>();
        CascadeLaserSource.volume = 1f;
        CascadeLaserSource.clip = CascadeLaserSound;
        CascadeLaserSource.Play();
    }


    void Update()
    {
        if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
        {
            BulletTravel();
        }
    }

    public override void BulletTravel()
    {
        base.BulletTravel();

        if (transform.localScale.y < 0.7)
        {
            transform.localScale += new Vector3(0, 1, 0) * Time.deltaTime * 0.9f;
        }
    }
}
