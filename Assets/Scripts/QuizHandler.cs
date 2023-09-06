using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EIC.Quiz;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;
using Random = System.Random;

public class QuizHandler : MonoBehaviour
{
    [SerializeField] private QuizManager QuizManagerSF;
    [SerializeField] private Image ImageSF;
    [SerializeField] private Sprite SpriteSF;
    [SerializeField] private AudioControl AudioControlSF;
    [SerializeField] private ProgressPanel ProgressPanelSF;
    [SerializeField] private Stars StarsSF;
    [SerializeField] private Rating RatingSF;
    [SerializeField] private Reward RewardSF;
    [SerializeField] private TextMeshProUGUI TryOneMoreTextSF;

    private void Start()
    {
        ImageSF.sprite = SpriteSF;
        if (YandexGame.SDKEnabled)
        {
            GetLoad();
        }

    }

    private void OnEnable()
    {
        QuizManagerSF.OnChoose += OnChoose;
        RewardSF.OnHide += QuizManagerSF.SetOptionsInteractable;
        YandexGame.GetDataEvent += GetLoad;
    }

    private void OnDisable()
    {
        YandexGame.SaveProgress();
        QuizManagerSF.OnChoose -= OnChoose;
        RewardSF.OnHide -= QuizManagerSF.SetOptionsInteractable;
        YandexGame.GetDataEvent -= GetLoad;
    }
    private void Update()
    {
        Hack();
    }
    public void GetLoad()
    {
        RatingSF.Load();
        StarsSF.Load();
        QuizManagerSF.Load();
        //ProgressPanelSF.Load(QuizManagerSF.QuestionStackCount);
    }
    private void OnChoose(bool correct, QuizResult quizResult)
    {
        //ProgressPanelSF.AddProgress(correct);
        if (correct)
        {
            AudioControlSF.PlayTrue();
            RatingSF.AddRating(StarsSF.StarsCount);
            if (ShowResult(quizResult)) return;
            StarsSF.AddStar();
            TryOneMoreTextSF.gameObject.SetActive(false);
            StartCoroutine(NextQuestion(1));
        }
        else
        {
            AudioControlSF.PlayFalse();
            if (ShowResult(quizResult)) return;
            TryOneMoreTextSF.gameObject.SetActive(true);
            if (StarsSF.StarsCount > 2)
            {
                if (RewardSF.Show(true))
                    QuizManagerSF.SetOptionsInteractable(false);
                else
                    StarsSF.RemoveStars();
            }
            else
            {
                StarsSF.RemoveStars();
            }
        }
        //QuizManagerSF.PopQuestion();
    }

    private bool ShowResult(QuizResult quizResult)
    {
        if (!quizResult.complete) return false;
        Main.RightAnswersCount = quizResult.rightAnswers;
        Main.WrongAnswersCount = quizResult.wrongAnswers;
        RatingSF.ResetData();
        StarsSF.ResetData();
        QuizManagerSF.ResetData();
        SceneManager.LoadScene(1);
        return true;
    }

    private IEnumerator NextQuestion(float seconds)
    {
        YandexGame.SaveProgress();
        yield return new WaitForSeconds(seconds);
        QuizManagerSF.PopQuestion();
    }
    
    
    private void Hack()
    {
        if (Input.GetKey(KeyCode.P))
        {
            var qr = new QuizResult();
            qr.complete = true;
            qr.rightAnswers = 10;
            qr.wrongAnswers = 10;
            ShowResult(qr);
        }
    }

}
