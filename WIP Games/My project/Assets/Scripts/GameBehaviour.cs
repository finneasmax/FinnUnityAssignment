using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public Button WinButton;

    private int _itemsCollected= 0;
    private int _playerHP = 10;

    public int MaxItems = 4;

    public TMP_Text HealthText;
    public TMP_Text ItemText;
    public TMP_Text ProgressText;

    public int Items {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = "Items collected: " + Items;
            if (_itemsCollected >= MaxItems)
            {
                ProgressText.text = "You've found all the items!";

                WinButton.gameObject.SetActive(true);
            }
            else
            {
                ProgressText.text = "Item found, only " + (MaxItems - _itemsCollected) + " more!";
            }
        }
    }
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            HealthText.text = "Player Health  " + HP;
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }
    private void Start()
    {
        ItemText.text += _itemsCollected;
        HealthText.text += _playerHP;
    }

}
