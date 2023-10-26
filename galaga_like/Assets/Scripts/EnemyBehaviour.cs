using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBehaviour : MonoBehaviour
{
    private float _hp = 10;
    private float _xDir;
    private float _speed = 4;
    private float _xBounds = 3.6f;
    private bool _shootingCascadeLasers = false;
    private bool _shootingQuickShot = false;
    private bool _shootingDelayLasers = false;
    private Transform _target;
    public GameObject CascadeLaser;
    public GameObject EnemyQuickBullet;
    public GameObject DelayLaser;
    [SerializeField] TextMeshProUGUI EnemyLivesGUI;





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


    void Start()
    {

        _target = GameObject.Find("Player").transform;

        _xDir = Random.Range(0.0f, 1.0f) >= 0.5f ? 1 : -1;
        EnemyLivesGUI.text = "Enemy Lives: " + _hp;
    }


    void Update()
    {



        if (_hp < 1)
        {
            Destroy(this.transform.gameObject);
        }


        if (Mathf.Abs(transform.position.x) >= _xBounds)
        {
            transform.position = new Vector3(transform.position.x > 0 ? _xBounds - 0.1f : -_xBounds + 0.1f, transform.position.y, 0);
            _xDir *= -1;

        }



        this.transform.position += new Vector3(_xDir, 0, 0)*_speed * Time.deltaTime;



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


        if (!_shootingQuickShot && Mathf.Abs(_target.transform.position.x - transform.position.x) < 0.5f)
        {
            StartCoroutine(QuickShot());
            _shootingQuickShot = true;
        }


    }

    //ATTACKS--------------------------------------------

    IEnumerator CascadeLasers()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i <= 9; i++)
        {
            Instantiate(CascadeLaser, transform.position + new Vector3(0, -1, 0), transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
        _shootingCascadeLasers = false;
    }



    IEnumerator QuickShot()
    {
        for(int i=0; i<=1; i++)
        {
            for (int b = 0; b <= 2; b++)
            {
                Instantiate(EnemyQuickBullet, transform.position + new Vector3(0, -1, 0), transform.rotation);
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
        Instantiate(DelayLaser, transform.position + new Vector3(0, -1, 0), transform.rotation);
        yield return new WaitForSeconds(4);
        _shootingDelayLasers = false;
    }



    //----------------------------------------------------------




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP -= 1;
            EnemyLivesGUI.text = "Remaining Lives: " + _hp;
        }

    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
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
}

