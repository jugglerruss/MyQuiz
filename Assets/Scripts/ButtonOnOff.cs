using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOff : MonoBehaviour
{
    [SerializeField] private Image ImageOffSF;
    [SerializeField] private String VariableNameSF;

    private Image _image;
    private bool _isOn;
    void Start()
    {
        _image = GetComponent<Image>();
        _isOn = PlayerPrefs.GetInt(VariableNameSF, 1) == 1;
        ImageOffSF.gameObject.SetActive(!_isOn);
    }
    public void Toggle()
    {
        _isOn = !_isOn;
        ImageOffSF.gameObject.SetActive(!_isOn);
    }
}
