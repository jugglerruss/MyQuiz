using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Reward : MonoBehaviour
{
    [SerializeField] private float PulseSpeedSF;
    [SerializeField] private Transform ButtonAnimSF;

    private Vector2 _scale;
    private bool _isAvailable;
    public Action<bool> OnHide;
    private void Awake()
    {
        _scale = ButtonAnimSF.localScale;
        _isAvailable = true;
    }

    private void Update()
    {
        var plusScale = (1.4f+Mathf.Sin(Time.time * PulseSpeedSF)) / 4.0f;
        ButtonAnimSF.localScale = new Vector3(_scale.x + plusScale,_scale.y + plusScale);
    }

    public bool Show(bool isShow)
    {
        if (!_isAvailable) return false;
        var localPos = transform.localPosition;
        var posY = isShow ? localPos.y - 3000 : localPos.y + 3000;
        transform.localPosition = new Vector3(localPos.x, posY);
        return true;
    }
    public void Hide()
    {
        var localPos = transform.localPosition;
        var posY = localPos.y + 3000;
        transform.localPosition = new Vector3(localPos.x, posY);
        OnHide?.Invoke(true);
    }
    public void ShowReward()
    {
        YandexGame.RewVideoShow(0);
        _isAvailable = false;
        StartCoroutine(WaitForSeconds(6));
    }
    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isAvailable = true;
    }
}
