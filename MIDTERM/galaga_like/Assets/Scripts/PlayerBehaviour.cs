using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerLivesGUI;

    private AudioSource _source;

    [SerializeField] AudioClip HurtSound;

    public KeyCode ControlRight;
    public KeyCode ControlLeft;
    public KeyCode Fire;
    public KeyCode Dodge;
    private float _hurrySpeed = 4;

    public GameObject CurrentBullet;

    private SpriteRenderer _sprite;
    private Rigidbody2D _body;
    private Collider2D _collider;

    private bool _isShooting;
    private bool _isDodging;
    private bool _spedUp = false;
    private bool _canFire = true;
    private bool _uziRegulating = false;
    private bool _hasUzi = false;

    private float speed = 1;
    private float _xBounds;

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
        _xBounds = Screen.width / 136;

        _sprite = this.GetComponent<SpriteRenderer>();
        _body = this.GetComponent<Rigidbody2D>();
        _collider = this.GetComponent<Collider2D>();

        PlayerLivesGUI.text = "Remaining Lives: " + HP;
    }

    
    void Update()
    {
        _xBounds = Screen.width / 136;



        //Play State..........
        if (GameBehaviour.Instance.State==GameBehaviour.GameStates.Play && GameBehaviour.Instance.PlayerAlive)
        {


            if (HP < 1)
            {
                Debug.Log("You Died");
                _body.simulated = false;
                _collider.enabled = false;
                _sprite.enabled = false;
                GameBehaviour.Instance.PlayerAlive = false;
                //Destroy(this.transform.gameObject);
            }

            if (Input.GetKey(ControlLeft) && transform.position.x > -_xBounds)
            {
                transform.position -= new Vector3(GameBehaviour.Instance.playerSpeed, 0, 0) * Time.deltaTime * speed;
            }
            else if (Input.GetKey(ControlRight) && transform.position.x < _xBounds)
            {
                transform.position += new Vector3(GameBehaviour.Instance.playerSpeed, 0, 0) * Time.deltaTime * speed;
            }

            if (_hasUzi)
            {
                if (Input.GetKey(Fire) && _canFire)
                {
                    Instantiate(CurrentBullet, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
                    _canFire = false;
                    if (!_uziRegulating)
                    {
                        StartCoroutine(UziRegulator());
                        _uziRegulating = true;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(Fire))
                {
                    Instantiate(CurrentBullet, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
                }
            }

            if (Input.GetKeyDown(Dodge) && !_isDodging)
            {
                StartCoroutine(Dodging());
                _isDodging = true;
            }
        }

    }

    IEnumerator Dodging()
    {
        GameBehaviour.Instance.playerSpeed += _hurrySpeed;
        yield return new WaitForSeconds(0.2f);
        GameBehaviour.Instance.playerSpeed -= _hurrySpeed;
        yield return new WaitForSeconds(0.2f);
        _isDodging = false;
    }

    IEnumerator BeingSped()
    {
        speed += _hurrySpeed*0.5f;
        yield return new WaitForSeconds(10);
        speed -= _hurrySpeed * 0.5f;
        _spedUp = false;
        GameBehaviour.Instance.PowerupActive = false;
        yield break;
    }

    IEnumerator UziRegulator()
    {
        yield return new WaitForSeconds(0.06f);
        _canFire = true;
        _uziRegulating = false;
        yield break;
    }


    IEnumerator BeingUzieD()
    {
        _hasUzi = true;
        yield return new WaitForSeconds(8);
        _hasUzi = false;
        GameBehaviour.Instance.PowerupActive = false;
        yield break;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isDodging && _hp >= 1)
        {
            Debug.Log("Ouch!");
            HP -= 1;
            _source = this.GetComponent<AudioSource>();
            _source.volume = 1f;
            _source.clip = HurtSound;
            _source.Play();
            PlayerLivesGUI.text = "Remaining Lives: " + HP;
        }
        else if(_hp >= 1)
        {
            Debug.Log("Dodged");
        }
    }

    public void SpedUp()
    {
        if(!_spedUp)
        {
            StartCoroutine(BeingSped());
            _spedUp = true;
        }
    }

    public void UzieD()
    {
        if(!_hasUzi)
        {
            StartCoroutine(BeingUzieD());
            _hasUzi = true;
        }
    }

    public void ResetSelf()
    {
        _body.simulated = true;
        _collider.enabled = true;
    }

}
