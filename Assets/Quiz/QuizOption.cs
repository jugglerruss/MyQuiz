using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EIC.Quiz
{
    [AddComponentMenu("EIC/Quiz/QuizOption")]
    [RequireComponent(typeof(Button), typeof(Image))]
    public class QuizOption : MonoBehaviour
    {
        public bool Correct { get; set; }
        public string Answer { get; private set; }
        public Image Image { get; private set; }

        private TextMeshProUGUI _text;
        private Button _button;
        private Color _defaultColor;
        private QuizManager _quizManager;

        private void Awake()
        {
            _button = GetComponent<Button>();
            Image = GetComponent<Image>();
            _defaultColor = Image.color;
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _quizManager = GetComponentInParent<QuizManager>();
            _button.onClick.AddListener(Choose);
        }
        public void SetAnswer(string answer)
        {
            if (!_text)
            {
                Debug.LogError("Quiz option requires a Text Mesh Pro component as a child");
                return;
            }

            Answer = answer;
            _text.text = Answer;
            Reset();
        }

        public void Reset()
        {
            _button.interactable = true;
            Image.color = _defaultColor;
            Correct = false;
        }

        private void Choose()
        {
            _button.interactable = false;
            _quizManager.Choose(this); 
        }
    }
}