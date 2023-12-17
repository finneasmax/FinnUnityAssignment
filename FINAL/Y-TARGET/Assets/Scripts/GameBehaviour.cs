using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] GameObject _pillar;
    private GameObject _arena;
    private NavMeshSurface _surface;

    public static GameBehaviour Instance;

    private GameObject _player;
    private PlayerBehaviour _playerBehaviour;

    public TextMeshProUGUI _gameOverMess;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else Instance = this;
    }


    void Start()
    {
        _player = GameObject.Find("Player");
        _playerBehaviour = _player.GetComponent<PlayerBehaviour>();
        _arena = GameObject.Find("DumbArena");
        _surface = _arena.GetComponent<NavMeshSurface>();
        for (int i = 0; i <= 50; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-245, 245), Random.Range(5, 700), Random.Range(-245, 245));
            Quaternion rot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject NewPillar = _pillar;
            NewPillar.transform.localScale = new Vector3(Random.Range(600, 1000), Random.Range(3, 30), Random.Range(3, 30));
            Instantiate(NewPillar, pos, rot, _arena.transform);
        }
        if (_surface!=null)
        {
            _surface.BuildNavMesh();
        }
    }

    public void CheckWinner()
    {
        if(!_playerBehaviour.isAlive)
        {
            Debug.Log("You Died");
            _gameOverMess.enabled = true;
        }
    }
}
