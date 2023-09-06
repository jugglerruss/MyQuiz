using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ProgressPanel : MonoBehaviour
{
    [SerializeField] private Image TrueItemSF;
    [SerializeField] private Image FalseItemSF;
    private RectTransform _rectTransformSF;
    private int[] _itemsAnswers;
    private int _countItems;
    private int _maxProgress;
    private float _widthItems;

    private void Start()
    {
        _rectTransformSF = GetComponent<RectTransform>();
    }

    public void Load( int questionsCount )
    {
        var answers = YandexGame.savesData.itemsAnswers.Where(a => a != 0).ToArray();
        _countItems = answers.Length;
        Debug.Log("countAnswers "+_countItems);
        PlayerPrefs.SetInt("countItems", _countItems);
        _maxProgress = _countItems + questionsCount;
        for (int i = 0; i < _countItems; i++)
        {
            ViewProgress(i,answers[i] == 1);
        }
    }
    public void AddProgress(bool isCorrect)
    {
        ViewProgress(_countItems, isCorrect);
        _countItems++;
        YandexGame.savesData.itemsAnswers[_countItems] = isCorrect ? 1 : 2;
        var answers = YandexGame.savesData.itemsAnswers.Where(a => a != 0).ToArray();
        Debug.Log("answers.Length "+answers.Length);
    }

    private void ViewProgress(int countItems, bool isCorrect)
    {
        if (_widthItems == 0)
        {
            _widthItems = _rectTransformSF.rect.width / _maxProgress;
        }
        var pos = new Vector2(countItems * _widthItems, 0);
        var item = Instantiate(isCorrect ? TrueItemSF : FalseItemSF,transform);
        item.rectTransform.localPosition = pos;
        item.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _widthItems);
    }
}
