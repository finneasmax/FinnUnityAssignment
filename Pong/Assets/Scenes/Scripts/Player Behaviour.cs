using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            _scoreGUI.text = Score.ToString();
        }
    }



    [SerializeField] TextMeshProUGUI _scoreGUI;


    private void Update()
    {
        if(GameBehaviour.Instance.State != GameBehaviour.GameStates.Start)
        {
            _scoreGUI.enabled=true;
        }
        else
        {
            _scoreGUI.enabled = false;
        }
    }
}
