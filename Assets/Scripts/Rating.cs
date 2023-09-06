using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using YG;

public class Rating : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI RatingTextSF;
    [SerializeField] private int RatingOnceSF;
    [SerializeField] private string NameLbSF;
    private int _ratingCount;
    void Start()
    {
        _ratingCount = PlayerPrefs.GetInt("ratingCount", 0);
        RatingTextSF.text = _ratingCount.ToString();
    }

    public void Load()
    {
        _ratingCount = YandexGame.savesData.Rating;
        if (_ratingCount == 0)
        {
            _ratingCount = PlayerPrefs.GetInt("ratingCount", 0);
        }
        else
        {
            PlayerPrefs.SetInt("ratingCount", _ratingCount);
        }
        PlayerPrefs.SetInt("ratingCount", _ratingCount);
        RatingTextSF.text = _ratingCount.ToString();
    }
    public void ResetData()
    {
        _ratingCount = 0;
        PlayerPrefs.SetInt("ratingCount", _ratingCount);
        RatingTextSF.text = _ratingCount.ToString();
    }
    public void AddRating(int multiplier)
    {
        _ratingCount += RatingOnceSF * Convert.ToInt32(Math.Pow(2, multiplier));
        RatingTextSF.text = _ratingCount.ToString();
        if (YandexGame.savesData.Rating < _ratingCount)
        {
            PlayerPrefs.SetInt("ratingCount", _ratingCount);
            YandexGame.savesData.Rating = _ratingCount;
            YandexGame.NewLeaderboardScores( NameLbSF, _ratingCount);
        }
    }
}
