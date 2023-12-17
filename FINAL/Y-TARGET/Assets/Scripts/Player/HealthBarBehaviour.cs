using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarBehaviour : MonoBehaviour
{
    private Slider _slider;
    public float ScreenRatioX;
    public float ScreenRatioY;

    public TextMeshProUGUI Title;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        transform.position = new Vector3(Screen.width*ScreenRatioX, Screen.height* ScreenRatioY);
    }

    public void UpdateHealthBar(float currentValue,float maxValue)
    {
        _slider.value = currentValue / maxValue;
    }
}
