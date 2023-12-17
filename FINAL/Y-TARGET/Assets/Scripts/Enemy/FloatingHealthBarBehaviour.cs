using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarBehaviour : MonoBehaviour
{
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateHealthBar(float currentValue,float maxValue)
    {
        _slider.value = currentValue / maxValue;
    }
}
