using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_PlayerBehaviour : MonoBehaviour
{
    private int _hp = 10;
    private int _maxHp = 10;
    public HealthBarBehaviour _healthBar;
    public bool isAlive = true;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<HealthBarBehaviour>();
    }

    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("YOW!");
            _hp -= 1;
            _healthBar.UpdateHealthBar(_hp, _maxHp);
            GameBehaviour.Instance.CheckWinner();
        }

        if(_hp<1)
        {
            isAlive = false;
        }
    }
}
