using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MultiplierTextSF;
    [SerializeField] private float PulseSpeedSF;
    [SerializeField] private float AnimateDeltaSF;
    [SerializeField] private Transform HidePosSF;
    [SerializeField] private Transform ShowPosSF;
    private Transform TransformThis => transform;
    
    private bool _isHide;
    private int _starsCount;
    private Coroutine _moveCoroutine;
    private void Start()
    {
        InstantHide();
    }
    private void InstantHide()
    {
        _isHide = true;
        TransformThis.localPosition = HidePosSF.localPosition;
    }
    private void Hide()
    {
        _isHide = true;
        StartAnimateMove(HidePosSF.localPosition);
    }
    private void Show()
    {
        _isHide = false;
        StartAnimateMove(ShowPosSF.localPosition);
    }
    public void StartAnimateMove(Vector2 target)
    {
        if (_moveCoroutine != null) 
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(AnimateMove(target));
    }
    public void UpdateCombo(int starsCount)
    {
        _starsCount = starsCount;
        if (starsCount == 0)
        {
            if(!_isHide) Hide();
            StopCoroutine(AnimatePulseText());
        }
        else
        {
            if(_isHide) Show();
            if(_starsCount>1) StartCoroutine(AnimatePulseText());
        }
        UpdateComboText(starsCount);
    }

    private void UpdateComboText(int starsCount)
    {
        MultiplierTextSF.text = $"x{Convert.ToInt32(Math.Pow(2, starsCount))}";
    }
    private IEnumerator AnimateMove(Vector2 target)
    {
        while (Math.Abs(transform.localPosition.y - target.y) > 0.1)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, AnimateDeltaSF);
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator AnimatePulseText()
    {
        while (_starsCount > 1)
        {
            var plusScale = (1+Mathf.Sin(Time.time * PulseSpeedSF)) * _starsCount * 0.1f  / 2.0f;
            MultiplierTextSF.transform.localScale = new Vector3(1 + plusScale,1 + plusScale);
            yield return new WaitForFixedUpdate();
        }
    }

}
