using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBulletBehaviour : BulletBehaviour
{
    AudioSource StandardBulletSource;

    [SerializeField] AudioClip StandarBulletSound;

    public void Start()
    {
        StandardBulletSource = this.GetComponent<AudioSource>();
        StandardBulletSource.volume = 1f;
        StandardBulletSource.clip = StandarBulletSound;
        StandardBulletSource.Play();


        speed = 20;
        _yDirection = 1;
        vector = new Vector3(0, _yDirection, 0);
    }

    void Update()
    {
        if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
        {
            BulletTravel();
        }
    }
}
