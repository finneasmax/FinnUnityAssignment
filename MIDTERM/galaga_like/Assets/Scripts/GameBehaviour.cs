using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public GameStates State;
    public bool PlayerAlive = true;
    public bool BossAlive = true;
    public static GameBehaviour Instance;
    public float playerSpeed= 4f;
    public float BulletSpeed = 4f;
    public GameObject SpeedBoost;


    private bool _droppingPowerups=false;
    public bool PowerupActive = false;

    [SerializeField] TextMeshProUGUI SpaceToStart;
    [SerializeField] TextMeshProUGUI Pause;
    [SerializeField] TextMeshProUGUI Resume;
    [SerializeField] TextMeshProUGUI CountDownUI;
    public bool CoutningDown = false;
    [SerializeField] TextMeshProUGUI Fight;
    [SerializeField] TextMeshProUGUI GameOver;
    [SerializeField] TextMeshProUGUI Win;
    [SerializeField] TextMeshProUGUI QtoQuit;
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] TextMeshProUGUI Explanation;

    private GameObject _player;
    private SpriteRenderer _playerSprite;
    private PlayerBehaviour _playerBehaviour;

    private EnemyBehaviour _bossBehaviour;
    private GameObject _boss;
    private SpriteRenderer _bossSprite;

    private bool _spaceFlickering = false;


    public enum GameStates
    {
        Start,
        Play,
        GameOver,
        Win,
        Pause
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else Instance = this;


        _player = GameObject.Find("Player");
        _playerSprite = _player.GetComponent<SpriteRenderer>();
        _playerBehaviour = _player.GetComponent<PlayerBehaviour>();


        _boss = GameObject.Find("Enemy");
        _bossSprite = _boss.GetComponent<SpriteRenderer>();
        _bossBehaviour = _boss.GetComponent<EnemyBehaviour>();


        State = GameStates.Start;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (State == GameStates.Start && !CoutningDown)
        {
            Title.enabled = true;
            Explanation.enabled = true;
        }
        else
        {
            Title.enabled = false;
            Explanation.enabled = false;
        }

        Debug.LogFormat("{0}",State);
        if (!BossAlive)
        {
            State = GameStates.Win;
            Win.enabled = true;
            QtoQuit.enabled = true;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _playerSprite.enabled = false;
                _bossSprite.enabled = false;
                Win.enabled = false;
                QtoQuit.enabled = false;
                ResetGame();
                State = GameStates.Start;
            }
        }
        if (!PlayerAlive)
        {
            State = GameStates.GameOver;
            GameOver.enabled = true;
            QtoQuit.enabled = true;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _playerSprite.enabled = false;
                _bossSprite.enabled = false;
                GameOver.enabled = false;
                QtoQuit.enabled = false;
                ResetGame();
                State = GameStates.Start;
            }
        }
        else
        {
            if (CoutningDown)
            {
                StopCoroutine(SpaceToStartFlicker());
                SpaceToStart.enabled = false;
            }

            if (State == GameStates.Start)
            {
                PlayerAlive = true;
                BossAlive = true;


                //SpaceToStart.enabled = true;

                if (!_spaceFlickering && !CoutningDown)
                {
                    StartCoroutine(SpaceToStartFlicker());
                    _spaceFlickering = true;
                    _playerSprite.enabled = false;
                    _bossSprite.enabled = false;
                }


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!CoutningDown && State == GameStates.Start)
                    {
                        StopCoroutine(SpaceToStartFlicker());
                        SpaceToStart.enabled = false;
                        StartCoroutine(CountDown());
                        CoutningDown = true;
                    }
                }
            }
            //else
            //{
            //    StopCoroutine(SpaceToStartFlicker());
            //    SpaceToStart.enabled = false;
            //}
            if (State == GameStates.Play)
            {
                //SpaceToStart.enabled = false;

                if (Input.GetKeyDown(KeyCode.P))
                {
                    State = GameStates.Pause;
                    //State = State == GameStates.Pause ? GameStates.Play : GameStates.Pause;
                    //Pause.enabled = State == GameStates.Pause ? true : false;
                }

                if (PlayerAlive)
                {
                    _playerSprite.enabled = true;
                }
                if (BossAlive)
                {
                    _bossSprite.enabled = true;
                }

                if (!_droppingPowerups && !PowerupActive)
                {
                    StartCoroutine(SpeedBoostGen());
                    _droppingPowerups = true;
                }
            }


            if (State == GameStates.Pause)
            {
                Pause.enabled = true;
                Resume.enabled = true;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    State = GameStates.Play;
                    //State = State == GameStates.Pause ? GameStates.Play : GameStates.Pause;
                    //Pause.enabled = State == GameStates.Pause ? true : false;
                }
            }
            else
            {
                Pause.enabled = false;
                Resume.enabled = false;
            }
        }
        
    }

    public void ResetGame()
    {
        _player.transform.position = new Vector3(0, _player.transform.position.y, 0);
        _boss.transform.position = new Vector3(0, _boss.transform.position.y, 0);
        _playerBehaviour.HP = 10;
        _bossBehaviour.HP = 10;
        PlayerAlive = true;
        BossAlive = true;
        _bossBehaviour.ResetSelf();
        _playerBehaviour.ResetSelf();
    }




    IEnumerator SpeedBoostGen()
    {
        yield return new WaitForSeconds(8);
        float leftOrRight = Random.Range(0.0f, 1.0f) >= 0.5 ? -1 : 1;
        GameObject powerUp = SpeedBoost;
        SpeedBoostBehaviour powerUpBehaviour = SpeedBoost.GetComponent<SpeedBoostBehaviour>();
        //powerUpBehaviour.setXDir((int)leftOrRight*-1);
        Instantiate(SpeedBoost, new Vector3((Screen.width / 100) * leftOrRight, 0, 0), transform.rotation);
        Debug.Log("Instantiated");
        PowerupActive = true;
        _droppingPowerups = false;
        StopCoroutine(SpeedBoostGen());
        yield break;
    }

    IEnumerator CountDown()
    {
        SpaceToStart.enabled = false;
        Debug.Log("Disabled");
        _playerSprite.enabled = true;
        _bossSprite.enabled = true;
        CountDownUI.enabled = true;
        CountDownUI.text = "3";
        yield return new WaitForSeconds(1f);
        CountDownUI.text = "2";
        yield return new WaitForSeconds(1f);
        CountDownUI.text = "1";
        yield return new WaitForSeconds(1f);
        CountDownUI.enabled = false;
        Fight.enabled = true;
        State = GameStates.Play;
        yield return new WaitForSeconds(1f);
        Fight.enabled = false;
        CoutningDown = false;
    }

    IEnumerator SpaceToStartFlicker()
    {
        yield return new WaitForSeconds(0.5f);
        SpaceToStart.enabled = SpaceToStart.enabled ? false : true;
        _spaceFlickering = false;
        yield break;
    }

    //IEnumerator SpeedBoostGen()
    //{
    //    yield return new WaitForSeconds(4);
    //    Instantiate(SpeedBoost, new Vector3(0, 0, 0), transform.rotation);
    //    _droppingPowerups = false;
    //    StopCoroutine(SpeedBoostGen());
    //    yield break;
    //}
}
