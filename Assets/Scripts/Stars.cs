using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Stars : MonoBehaviour
{
    private const int MaxStars = 5;
    [SerializeField] private List<Star> StarsListSF;
    [SerializeField] private Combo ComboSF;
    private int _starsCount;
    public int StarsCount => _starsCount;

    private void Start()
    {
        ClearStars();
    }

    public void Load()
    {
        _starsCount = YandexGame.savesData.StarsCount;
        if (_starsCount == 0)
        {
            _starsCount = PlayerPrefs.GetInt("starsCount", 0);
        }
        else
        {
            PlayerPrefs.SetInt("starsCount", _starsCount);
        }
        SetActiveStars(_starsCount,false);
        ComboSF.UpdateCombo(_starsCount);
    }
    public void ResetData()
    {
        _starsCount = 0;
        PlayerPrefs.SetInt("starsCount", _starsCount);
        YandexGame.savesData.StarsCount = 0;
    }
    public void SetActiveStars(int count, bool isAnimate)
    {
        for (int i = 0; i < count; i++)
        {
            StarsListSF[i].SetActive(true,isAnimate);
        }
    }
    public void AddStar()
    {
        if(MaxStars != _starsCount) _starsCount++;
        PlayerPrefs.SetInt("starsCount", _starsCount);
        YandexGame.savesData.StarsCount = _starsCount;
        SetActiveStars(_starsCount, true);
        ComboSF.UpdateCombo(_starsCount);
    }
    public void RemoveStars()
    {
        _starsCount = 0;
        PlayerPrefs.SetInt("starsCount", 0);
        YandexGame.savesData.StarsCount = _starsCount;
        ClearStars();
        ComboSF.UpdateCombo(_starsCount);
    }
    public void ClearStars()
    {
        foreach (var star in StarsListSF)
        {
            star.SetActive( false, false);
        }
    }

}
