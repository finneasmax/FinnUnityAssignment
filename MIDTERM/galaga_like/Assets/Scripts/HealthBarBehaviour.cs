using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBehaviour : MonoBehaviour
{
    public GameObject Character;
    private float _width;
    private float _startwidth = 500;
    private float _barSizeRatio=0.3f;
    private float _yPosRatio = 0.9f;
    private float _xPosRatio = -0.6f;
    private int _onOff=0;
    private PlayerBehaviour _behaviour;

    void Start()
    {
        _behaviour = Character.GetComponent<PlayerBehaviour>();
    }


    void Update()
    {
        if (GameBehaviour.Instance.State==GameBehaviour.GameStates.Play)
        {
            _onOff = 1;
        }
        else if(GameBehaviour.Instance.State == GameBehaviour.GameStates.Start)
        {
            _onOff = 0;
        }

        float leftAnchorOffset = -((1 - _behaviour.RatioHP) * Screen.width * (_barSizeRatio*_onOff)) / 2;
        transform.localScale = new Vector3(_behaviour.RatioHP * (Screen.width * (_barSizeRatio * _onOff)), transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3(leftAnchorOffset+(_xPosRatio*(Screen.width/2)), (Screen.height / 2) * _yPosRatio, 0);
    }
}
