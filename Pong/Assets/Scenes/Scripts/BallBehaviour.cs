using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public float Speed = 5.0f;
    int XDir, YDir;
    [SerializeField] float ylimit = 4.75f;
    [SerializeField] float xlimit = 5.0f;

    [SerializeField] AudioClip _wallCollision;
    [SerializeField] AudioClip _PaddleCollision;
    [SerializeField] AudioClip _lost;
    [SerializeField] AudioClip _win;

    AudioSource _source;
 

    void ResetBall()
    {
        Speed = 3.0f;
        transform.position = new Vector3(0,0,0);
        XDir = Random.Range(0.0f, 1.0f) >= 0.5f ? 1 : -1;
        YDir = Random.Range(0.0f, 1.0f) >= 0.5f ? 1 : -1;
        GameBehaviour.Instance.paddleSpeed = GameBehaviour.Instance.startPaddleSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Paddle"))
        {
            Speed += GameBehaviour.Instance.BallSpeedIncrement;
            XDir *= -1;
            _source.volume = 1f;
            _source.PlayOneShot(_PaddleCollision);
            Debug.Log("Collision!");
        }
    }

    private void Awake()
    {
        _source = this.GetComponent<AudioSource>();
    }


    void Start()
    {
        ResetBall();
    }

    public void PlayWin()
    {
        _source.volume = 1f;
        _source.PlayOneShot(_win);
    }


    public void PlayLost()
    {
        _source.volume = 0.5f;
        _source.PlayOneShot(_lost);
    }

    void Update()
    {
        if (GameBehaviour.Instance.State == GameBehaviour.GameStates.Play)
        {

            if (Mathf.Abs(transform.position.y) >= ylimit)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y > 0 ? ylimit - 0.01f : -ylimit + 0.01f, transform.position.z);
                YDir *= -1;
                _source.volume = 1f;
                _source.PlayOneShot(_wallCollision);
            }

            if (Mathf.Abs(transform.position.x) >= xlimit)
            {
                GameBehaviour.Instance.UpdateScore(transform.position.x > 0 ? 1 : 2);
                //if(!_isWinning)
                //{
                //    _source.PlayOneShot(_lost);
                //}
                ResetBall();

                //foreach (PlayerBehaviour player in GameBehaviour.Instance.Players)
                //{
                //    if (player.Score >= GameBehaviour.Instance.ScoreToWin)
                //    {
                //        _source.PlayOneShot(_win);
                //    }
                //    else
                //    {
                //        _source.PlayOneShot(_lost);
                //    }
                //}
            }

            transform.position +=
                new Vector3(Speed * XDir, Speed * YDir, 0) * Time.deltaTime;
        }
        else if (GameBehaviour.Instance.State == GameBehaviour.GameStates.Start)
        {
            ResetBall();
        }

    }
}
