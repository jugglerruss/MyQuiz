using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace EIC.Quiz
{
    [AddComponentMenu("EIC/Quiz/QuizManager")]
    [RequireComponent(typeof(CanvasGroup))]
    public class QuizManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI QuestionTextSF;
        [SerializeField] private Color RightAnswerColorSF;
        [SerializeField] private Color WrongAnswerColorSF;
        [FormerlySerializedAs("questionsFilePath")]
        [Tooltip("From Resources folder. Leave empty to setup manually")]
        [SerializeField] private string QuestionsFilePathSF;
        [Tooltip("Allow multiple failed attempts for each question")]
        [field: SerializeField] public bool MultipleAttempts { get; set; }

        public event Action<bool, QuizResult> OnChoose;

        public int QuestionStackCount => _quizDataItems.Count;
        
        private CanvasGroup _canvasGroup;
        private QuizOption[] _options;
        private Stack<QuizDataItem> _quizDataItems;
        private QuizDataItem _currentQuizDataItem;
        private QuizResult _quizResult;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _options = GetComponentsInChildren<QuizOption>();
        }
        public void Load()
        {
            _quizDataItems = YandexGame.savesData.quizDataItems;
            if (_quizDataItems != null)
            {
                Debug.Log(_quizDataItems.Count);
                _currentQuizDataItem = YandexGame.savesData.currentQuizDataItem;
                if (_currentQuizDataItem.question == null)
                {
                    PopQuestion();
                }else{
                    SetQuestion();
                    SetOptions();
                }
                return;
            }
            if (string.IsNullOrEmpty(QuestionsFilePathSF)) return;
            Setup(QuestionsFilePathSF);
        }
        
        public void ResetData()
        {
            _quizDataItems = null;
        }
        public void Setup(string resourcePath)
        {
            var qd = LoadResourceFromJson(resourcePath);
            SetQuestion(qd);
            PopQuestion();
        }

        private static QuizDataItem[] LoadResourceFromJson(string resourcePath)
        {
            var json = Resources.Load<TextAsset>(resourcePath);
            return JsonUtility.FromJson<QuizData>(json.text).questions;
        }

        private void SetQuestion(IList<QuizDataItem> quizDataItems)
        {
            _quizDataItems = new Stack<QuizDataItem>();
            _quizResult = default;
            
            var rng = new System.Random();
            var n = quizDataItems.Count;
            
            while (n > 1) 
            {
                var k = rng.Next(n--);
                (quizDataItems[n], quizDataItems[k]) = (quizDataItems[k], quizDataItems[n]);
            }

            foreach (var qdi in quizDataItems)
            {
                _quizDataItems.Push(qdi);
            }
        }

        public void RefreshQuestion()
        {
            if (_quizDataItems == null)
            {
                Debug.LogError("Questions are not set");
                return;
            }

            if (_quizDataItems.Count == 0)
            {
                Debug.LogWarning("Question stack is empty");
                return;
            }

            var tempQdi = _currentQuizDataItem;
            var tempResult = _quizResult;
            
            SetQuestion(_quizDataItems.ToArray());
            _quizResult = tempResult;
            PopQuestion();
            _quizDataItems.Push(tempQdi);
        }

        public void PopQuestion()
        {
            if (_quizDataItems == null)
            {
                Debug.LogError("Questions are not set");
                return;
            }

            if (_quizDataItems.Count == 0)
            {
                Debug.LogWarning("Question stack is empty");
                return;
            }
            
            _currentQuizDataItem = _quizDataItems.Pop();
            YandexGame.savesData.currentQuizDataItem = _currentQuizDataItem;

            SetQuestion();
            SetOptions();
        }

        private void SetQuestion()
        {
            QuestionTextSF.text = _currentQuizDataItem.question;
        }
        private void SetOptions()
        {
            if (_currentQuizDataItem.options.Length != _options.Length)
            {
                Debug.LogError("The number of quiz option buttons must be the same as the number of options in the resource file");
                return;
            }
            for (var i = 0; i < _options.Length; i++)
            {
                _options[i].SetAnswer(_currentQuizDataItem.options[i]);
            }

            var correctOption = _currentQuizDataItem.correctOption;
            var nOptions = _currentQuizDataItem.options.Length;
            var nOptionButtons = _options.Length;

            if (correctOption > nOptions || correctOption > nOptionButtons)
            {
                Debug.LogError(
                    $"Invalid option. The correct option must be between 1 and {_currentQuizDataItem.options.Length}");
                return;
            }

            _options[correctOption - 1].Correct = true;
            SetOptionsInteractable(true);
        }

        public void Choose(QuizOption quizOption)
        {
            _quizResult.complete = _quizDataItems.Count == 0;
            SetOptionsInteractable(MultipleAttempts);
            if (quizOption.Correct)
            {
                quizOption.Image.color = RightAnswerColorSF;
                _quizResult.rightAnswers++;
                SetOptionsInteractable(false);
            }
            else
            {
                quizOption.Image.color = WrongAnswerColorSF;
                _quizResult.wrongAnswers++;
            }

            OnChoose?.Invoke(quizOption.Correct, _quizResult);
            YandexGame.savesData.quizDataItems = _quizDataItems;
        }

        public void SetOptionsInteractable(bool isInteract)
        {
            _canvasGroup.interactable = isInteract;
        }
    }
}