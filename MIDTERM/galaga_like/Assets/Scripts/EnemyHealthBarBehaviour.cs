using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarBehaviour : MonoBehaviour
{
    public GameObject Character;
    private float _width;
    private float _startwidth=500;
    private float _barSizeRatio=0.8f;
    private int _onOff = 0;
    private float _yPosRatio = -0.9f;
    private float _xPosRatio = 0;
    private EnemyBehaviour _behaviour;

    void Start()
    {
        _behaviour=Character.GetComponent<EnemyBehaviour>();
    }

    
    void Update()
    {
        if (GameBehaviour.Instance.State == GameBehaviour.GameStates.Play)
        {
            _onOff = 1;
        }
        else if (GameBehaviour.Instance.State == GameBehaviour.GameStates.Start)
        {
            _onOff = 0;
        }
        float leftAnchorOffset= -((1 - _behaviour.RatioHP) * Screen.width * (_barSizeRatio * _onOff)) / 2;
        transform.localScale = new Vector3(_behaviour.RatioHP * (Screen.width * (_barSizeRatio * _onOff)), transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3(leftAnchorOffset+ (_xPosRatio*Screen.width), (Screen.height / 2) * _yPosRatio, 0);
    }
}
