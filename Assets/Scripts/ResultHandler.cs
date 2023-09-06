using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EIC.Quiz;
using TMPro;
using UnityEngine.SceneManagement;
using YG;

public class ResultHandler : MonoBehaviour
{
    
    [SerializeField] private Image ImageSF;
    [SerializeField] private Sprite SpriteSF;
    [SerializeField] private TextMeshProUGUI ResultTextSF;
    [SerializeField] private AudioControl AudioControlSF;
    
    private void Start()
    {
        ImageSF.sprite = SpriteSF;
        ResultTextSF.text = $"Результат: {Main.RightAnswersCount} / {Main.WrongAnswersCount + Main.RightAnswersCount}";
    }

    public void Restart()
    {
        AudioControlSF.PlayClick();
        YandexGame.ResetSaveProgress();
        
        PlayerPrefs.SetInt("countItems", 0);
        SceneManager.LoadScene(0);
    }
}
