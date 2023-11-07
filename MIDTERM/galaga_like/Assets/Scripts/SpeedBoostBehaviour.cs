using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBehaviour : MonoBehaviour
{
    private float _xDir=1;
    private float _xxDir = 1;
    private float _yDir = 1;
    private float _xWeight;
    private float _yWeight;
    private float _speed=3;

    private bool _switchingDirections=false;

    private GameObject _player;
    private PlayerBehaviour _playerBehaviour;

    private AudioSource SpeedBoostSource;

    [SerializeField] AudioClip SpeedBoostSound;


    void Start()
    {
        _player = GameObject.Find("Player");
    }

   
    void Update()
    {
        if (GameBehaviour.Instance.State != GameBehaviour.GameStates.Pause)
        {
            if (GameBehaviour.Instance.PlayerAlive)
            {
                _playerBehaviour = _player.GetComponent<PlayerBehaviour>();
            }
            Debug.Log("Existing...");
            Debug.LogFormat("{0}", transform.position);
            if (!_switchingDirections)
            {
                StartCoroutine(SwitchDirection());
                _switchingDirections = true;
            }
            transform.position += new Vector3(_xDir * _xxDir, _yDir, 0) * Time.deltaTime * _speed;
            if (Mathf.Abs(transform.position.x) > (Screen.width / 100) + 3 || Mathf.Abs(transform.position.y) > (Screen.height / 100) + 3)
            {
                Debug.Log("Destroyed");
                GameBehaviour.Instance.PowerupActive = false;
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator SwitchDirection()
    {
        yield return new WaitForSeconds(1/_speed);
        _xxDir=Random.Range(0.0f, 1.0f)>=0.3? -1 : 1;
        _yWeight = Random.Range(transform.position.y>= 0 ? 0 : 1, 2);

        switch (_yWeight)
        {
            case 0:
                _yDir = -1;
                break;
            case 1:
                _yDir = 1;
                break;
            case 2:
                _yDir = 0;
                break;
        }
        _switchingDirections = false;
    }

    public void setXDir(int xDir)
    {
        _xDir = xDir;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Debug.Log("Poof!");
            float val = Random.Range(0.0f, 1.0f);
            if (val >= 0.5f)
            {
                _playerBehaviour.SpedUp();
            }
            else
            {
                _playerBehaviour.UzieD();
            }
            SpeedBoostSource = this.GetComponent<AudioSource>();
            SpeedBoostSource.volume = 1f;
            SpeedBoostSource.clip = SpeedBoostSound;
            SpeedBoostSource.Play();
            Destroy(this.gameObject);
        }
    }
}
