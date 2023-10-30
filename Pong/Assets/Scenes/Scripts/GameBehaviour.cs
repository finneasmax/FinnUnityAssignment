using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
    public float startPaddleSpeed = 5;
    public float paddleSpeed = 5;
    public float BallSpeedIncrement = 0.5f;
    public int ScoreToWin = 5;
    public GameStates State;
    public GameObject Ball;
    private SpriteRenderer _ballSprite;
    private BallBehaviour ballBehaviour;
    private bool _weHaveAWinner = false;
    private bool _isStartBlinking = false;
    private bool _isCountingDown = false;
    [SerializeField] TextMeshProUGUI _pauseGUI;
    [SerializeField] TextMeshProUGUI _quitOption;
    [SerializeField] TextMeshProUGUI _startMess;
    [SerializeField] TextMeshProUGUI _gameOf;
    [SerializeField] TextMeshProUGUI _pongTitle;
    [SerializeField] TextMeshProUGUI _countDown;
    [SerializeField] TextMeshProUGUI _explanation;


    public enum GameStates
    {
        Play,
        Pause,
        Start
    }



    public PlayerBehaviour[] Players = new PlayerBehaviour[2];

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        foreach (PlayerBehaviour player in Players)
        {
            player.Score = 0;
        }
        Ball = GameObject.Find("Ball");
        ballBehaviour=Ball.GetComponent<BallBehaviour>();
        _ballSprite = Ball.GetComponent<SpriteRenderer>();
        State = GameStates.Start;
    }

    public void UpdateScore(int playerIndex)
    {
        if (playerIndex < 1 || playerIndex > 2)
        {
            Debug.Log("There is no such player");
            return;
        }
        else
        {
            Players[playerIndex - 1].Score += 1;
            CheckWinner();
        }
    }
    private void CheckWinner()
    {
        foreach (PlayerBehaviour player in Players)
        {
            if (player.Score == ScoreToWin)
            {
                Debug.LogFormat("Player {0} score: {1}", player,player.Score);
                Debug.LogFormat("Player {0} wins!!!", player);
                _weHaveAWinner = true;
                break;
            }
        }
        if(_weHaveAWinner)
        {
            ballBehaviour.PlayWin();
            ResetGame();
        }
        else
        {
            ballBehaviour.PlayLost();
        }
    }
    private void ResetGame()
    {
        _weHaveAWinner = false;
        foreach (PlayerBehaviour player in Players)
        {
            player.Score = 0;
        }
    }

    private void Update()
    {
        if (State != GameStates.Start)
        {
            _pongTitle.enabled = false;
            _gameOf.enabled = false;
            _explanation.enabled = false;
            _startMess.enabled = false;

            if (Input.GetKeyDown(KeyCode.P))
            {
                State = State == GameStates.Play ? GameStates.Pause : GameStates.Play;
            }


            if (State != GameStates.Pause)
            {
                _pauseGUI.enabled = false;
                _quitOption.enabled = false;
            }
            else if (State == GameStates.Pause)
            {
                _pauseGUI.enabled = true;
                _quitOption.enabled = true;
                if (State == GameStates.Pause && Input.GetKeyDown(KeyCode.L))
                {
                    _pauseGUI.enabled = false;
                    _quitOption.enabled = false;
                    ResetGame();
                    State = GameStates.Start;
                }
            }
        }
        else
        {
            _pongTitle.enabled = true;
            _gameOf.enabled = true;

            if (!_isStartBlinking)
            {
                StartCoroutine(StartBlink());
                _isStartBlinking = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine(StartBlink());
                State = GameStates.Play;
                //if (!_isCountingDown)
                //{
                //    StartCoroutine(CountDown());
                //    _isCountingDown = true;
                //}
            }
        }
    }

    IEnumerator StartBlink()
    {
        _startMess.enabled = true;
        _explanation.enabled = true;
        yield return new WaitForSeconds(0.5f);
        _startMess.enabled = false;
        _explanation.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _isStartBlinking = false;
    }

    IEnumerator CountDown()
    {
        _pongTitle.enabled = false;
        _gameOf.enabled = false;
        _startMess.enabled = false;
        _ballSprite.enabled = false;
        _countDown.enabled = true;
        yield return new WaitForSeconds(4);
        //_countDown.text = "1";
        //for (int i=1; i >= 3;i++)
        //{
        //    _countDown.text = i.ToString();
        //    yield return new WaitForSeconds(1);
        //}
        _countDown.enabled = false;
        _ballSprite.enabled = true;
        State = GameStates.Play;
        _isCountingDown = false;
        //StopCoroutine(CountDown());
    }
}
