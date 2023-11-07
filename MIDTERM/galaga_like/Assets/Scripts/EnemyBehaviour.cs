using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBehaviour : MonoBehaviour
{
    private float _xDir;
    private float _speed = 3.5f;
    private float _xBounds = 3.6f;
    private bool _shootingCascadeLasers = false;
    private bool _cascadeLasersWaiting = false;
    private bool _shootingQuickShot = false;
    private bool _shootingDelayLasers = false;
    private Vector3 MovementVector;
    public GameObject CascadeLaser;
    public GameObject EnemyBullet;
    public GameObject DelayLaser;
    [SerializeField] TextMeshProUGUI EnemyLivesGUI;

    private SpriteRenderer _sprite;
    private Rigidbody2D _body;
    private Collider2D _collider;

    private GameObject _player;
    private Transform _playerTransform;
    private Vector3 _target;


    private AudioSource BossSource;

    [SerializeField] AudioClip QuickShotSound;

    [SerializeField] AudioClip HurtSound;



    private float _startHP = 10;
    private float _hp;

    public float HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            Debug.LogFormat("Enemy HP: {0}", _hp);
        }
    }
    public float RatioHP
    {
        get
        {
            return _hp / _startHP;
        }
        set
        {
            Debug.Log("RatioHP Can't Be Set");
        }
    }


    void Start()
    {
        _hp = _startHP;
        _xBounds = (Screen.width / 136);

        _sprite = this.GetComponent<SpriteRenderer>();
        _body = this.GetComponent<Rigidbody2D>();
        _collider = this.GetComponent<Collider2D>();

        _player = GameObject.Find("Player");

        _xDir = Random.Range(0.0f, 1.0f) >= 0.5f ? 1 : -1;
        EnemyLivesGUI.text = "Enemy Lives: " + _hp;
    }


    void Update()
    {
        _xBounds = (Screen.width / 136);


        //Play State......
        if (GameBehaviour.Instance.BossAlive)
        {
            if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Start && GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
            {

                if (GameBehaviour.Instance.PlayerAlive)
                {
                    _playerTransform = _player.GetComponent<Transform>();
                    _target = _playerTransform.position;
                }


                if (_hp < 1)
                {
                    _body.simulated = false;
                    _collider.enabled = false;
                    _sprite.enabled = false;
                    GameBehaviour.Instance.BossAlive = false;
                    //Destroy(this.transform.gameObject);
                }


                if (Mathf.Abs(transform.position.x) >= _xBounds)
                {
                    transform.position = new Vector3(transform.position.x > 0 ? _xBounds - 0.1f : -_xBounds + 0.1f, transform.position.y, 0);
                    _xDir *= -1;

                }



                this.transform.position += new Vector3(_xDir, 0, 0) * _speed * Time.deltaTime;

                if (!GameBehaviour.Instance.CoutningDown)
                {
                    if (!_shootingCascadeLasers)
                    {
                        StartCoroutine(CascadeLasers());
                        _shootingCascadeLasers = true;
                    }

                    if (!_shootingDelayLasers)
                    {
                        StartCoroutine(DelayLasers());
                        _shootingDelayLasers = true;
                    }


                    if (!_shootingQuickShot && Mathf.Abs(_target.x - transform.position.x) < 0.5f)
                    {
                        StartCoroutine(QuickShot());
                        _shootingQuickShot = true;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, (Screen.height / 200) * 0.8f, 0);
    }

    //ATTACKS--------------------------------------------

    IEnumerator CascadeLasers()
    {
        for (int i = 0; i <= 9; i++)
        {
            GameObject newBullet = CascadeLaser;
            BulletBehaviour newBulletBehaviour = newBullet.GetComponent<BulletBehaviour>();
            newBulletBehaviour.setVector(_target);
            Instantiate(CascadeLaser, transform.position + new Vector3(0, -1, 0), transform.rotation);
            //Debug.LogFormat("Vector: {0}", newBulletBehaviour.vector);
            yield return new WaitForSeconds(0.1f);
        }
        _cascadeLasersWaiting = true;
        yield return new WaitForSeconds(7);
        _cascadeLasersWaiting = false;
        _shootingCascadeLasers = false;
    }


    IEnumerator QuickShot()
    {

        for(int i=0; i<=1; i++)
        {
            for (int b = 0; b <= 2; b++)
            {
                GameObject newBullet = EnemyBullet;
                BulletBehaviour newBulletBehaviour = newBullet.GetComponent<BulletBehaviour>();
                newBulletBehaviour.setSpeed(15);
                BossSource = this.GetComponent<AudioSource>();
                BossSource.volume = 0.5f;
                BossSource.clip = QuickShotSound;
                BossSource.Play();
                Instantiate(EnemyBullet, transform.position + new Vector3(0, -1, 0), transform.rotation);
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(1);
        _shootingQuickShot = false;
    }

    IEnumerator DelayLasers()
    {
        Debug.Log("Delay Laser");
        for (int i = 0; i <= 3; i++)
        {
            GameObject newBullet = DelayLaser;
            DelayLaserBehaviour newBulletBehaviour = newBullet.GetComponent<DelayLaserBehaviour>();
            newBulletBehaviour.setSpeed(15);

            Instantiate(DelayLaser, transform.position + (_xDir > 0 ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0)), this.transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(5);
        _shootingDelayLasers = false;
    }



    //----------------------------------------------------------




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            HP -= 1;
            BossSource = this.GetComponent<AudioSource>();
            BossSource.volume = 1f;
            BossSource.clip = HurtSound;
            BossSource.Play();
            EnemyLivesGUI.text = "Remaining Lives: " + _hp;
        }

    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            Debug.Log("Whoa!");

            Vector2 thisPosition = transform.position;
            Vector2 otherPosition = other.transform.position;

            if (otherPosition.x > thisPosition.x)
            {
                _xDir = -1;
                Debug.Log("Right");
            }
            else if (otherPosition.x < thisPosition.x)
            {
                _xDir = 1;
                Debug.Log("Left");
            }
            else
            {
                Debug.Log("Direct Hit!");
            }
        }
    }

    public void ResetSelf()
    {
        _body.simulated = true;
        _collider.enabled = true;
    }
}

