using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    EnemyBaseState _currentState;

    bool _isAlive;
    public bool IsAlive { get => _isAlive; set { _isAlive = value; } }

    public EnemyAliveState AliveState = new();

    public GameObject Eyes;
    private GameObject _player;

    private void Start()
    {
        SetState(AliveState);
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SetState(EnemyBaseState newState)
    {
        _currentState = newState;
        _currentState.EnterState(this);
    }

    private void PlayerWatch()
    {
        Eyes.transform.LookAt(_player.transform.position);

        transform.LookAt(_player.transform.position);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;
        transform.eulerAngles = eulerAngles;
    }
}
