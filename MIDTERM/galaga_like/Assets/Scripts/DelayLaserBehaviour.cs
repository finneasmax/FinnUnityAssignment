using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayLaserBehaviour : EnemyBulletBehaviour
{
    private float _delayTime = 4;
    private bool _isWaiting = false;
    private bool _doneWaiting = false;
    private bool _isReady = false;
    private bool _isShooting = false;
    private bool _hasShot = false;

    private float _growFactor = 25;
    //private float _growSpeed = 5;

    private GameObject _player;
    private Transform _playerTransform;
    private Vector3 _target;

    private AudioSource DelayLazerSource;

    [SerializeField] AudioClip DelayLaserSpawn;
    [SerializeField] AudioClip DelayLaserShoot;


    private void Awake()
    {
        DelayLazerSource = this.GetComponent<AudioSource>();
        DelayLazerSource.volume = 1f;
        DelayLazerSource.clip = DelayLaserSpawn;
        DelayLazerSource.Play();
    }

    void Start()
    {
        _player = GameObject.Find("Player");
        speed = 20;
        _yDirection = -1;
        vector = new Vector3(0, _yDirection, 0);
        transform.localScale = new Vector3(0.04f, 0.04f, 0);
    }


    void Update()
    {
        if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
        {
            if (GameBehaviour.Instance.PlayerAlive)
            {
                _playerTransform = _player.GetComponent<Transform>();
                _target = _playerTransform.position;
            }

            if (!_isWaiting && !_isShooting)
            {
                StartCoroutine(WaitToGo());
                _isWaiting = true;
            }

            if (_isWaiting)
            {
                transform.localScale += new Vector3(0.001f, 0.001f, 0);
            }

            if (_doneWaiting && !_isShooting)
            {
                targetRotation();
                vector = (this.transform.position - _target).normalized * -1;
                _doneWaiting = false;
            }


            if (_isReady)
            {
                if (!_isShooting)
                {
                    if(!_hasShot)
                    {
                        DelayLazerSource = this.GetComponent<AudioSource>();
                        DelayLazerSource.volume = 1f;
                        DelayLazerSource.clip = DelayLaserShoot;
                        DelayLazerSource.Play();
                        _hasShot = true;
                    }
                    //Debug.Log("Shooting Out");

                    transform.localScale = new Vector3(0.02f, transform.localScale.y, 0);
                    transform.localScale += new Vector3(0, 1, 0) * Time.deltaTime * _growFactor;
                    //Debug.LogFormat("Collider size: {0}", collider.size);
                    //Debug.LogFormat("Laser size: {0}", transform.localScale);
                }

                if (transform.localScale.y > 10)
                {
                    _isShooting = true;
                }
            }
            if (_isShooting)
            {
                BulletTravel();
            }
        }  
    }


    IEnumerator WaitToGo()
    {
        //Debug.Log("Waiting");
        yield return new WaitForSeconds(_delayTime);
        _doneWaiting = true;
        _isWaiting = false;
        _isReady = true;
        yield break;
    }


    public override void BulletTravel()
    {
        transform.position += this.vector * speed * Time.deltaTime;
        if (Mathf.Abs(this.transform.position.y) >= 20 || Mathf.Abs(this.transform.position.y) >= 20)
        {
            Destroy(this.gameObject);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something Hit");
        }

    }

    private void targetRotation()
    {
        //Special thanks to ChatGPT...
        Vector3 direction = _target - this.transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, -angle);
        this.transform.rotation = rotation;
        //Debug.Log("Targeting...");
    }
}