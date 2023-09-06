using System.Collections;
using UnityEngine;
using EIC.Quiz;
using UnityEngine.Serialization;

/// <summary>
/// Quiz demo class
/// Subscribe to these events
/// </summary>

public class QuizDemoHandler : MonoBehaviour
{
    [FormerlySerializedAs("quizManager")] [SerializeField] private QuizManager QuizManagerSF;

    private void OnEnable()
    {
        QuizManagerSF.OnChoose += OnChoose;
    }

    private void OnDisable()
    {
        QuizManagerSF.OnChoose -= OnChoose;
    }

    private void OnChoose(bool correct, QuizResult quizResult)
    {
        Debug.Log($"{(correct ? "Right" : "Wrong")} answer!");
        
        if (quizResult.complete)
        {
            Debug.Log("The end!");
            Debug.Log($"Right answers: {quizResult.rightAnswers} | Wrong answers: {quizResult.wrongAnswers}");
            return;
        }

        StartCoroutine(NextQuestion(1));
    }

    private IEnumerator NextQuestion(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        QuizManagerSF.PopQuestion();
    }
}