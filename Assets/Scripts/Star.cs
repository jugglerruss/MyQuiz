using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] private Image StarImageSF;
    [SerializeField] private ParticleSystem ParticleSystemSF;
    private GameObject _go;

    private void Awake()
    {
        _go = StarImageSF.gameObject;
    }

    public void SetActive(bool isActive, bool isAnimate)
    {
        if (isActive && isAnimate)
        {
            ParticleSystemSF.Play();
        }
        if (isActive == _go.activeSelf) return;
        _go.SetActive(isActive);
    }
}
