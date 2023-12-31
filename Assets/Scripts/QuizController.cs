using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    private const string QUESTIONS_PATH = "/Resources/Questions/Questions1.json";
    
    public static QuizController Instance;

    public static event EventHandler<OnQuestionAnsweredEventArgs> OnQuestionAnswered;
    public class OnQuestionAnsweredEventArgs : EventArgs
    {
        public bool isCorrect;
    }
    public static event EventHandler<OnEndGameEventArgs> OnEndGame;
    public class OnEndGameEventArgs : EventArgs
    {
        public int correctAnswersNumber;
    }
    
    private Question[] questionArray;
    private int currentQuestionIndex = 0;

    private int correctAnswersNumber;

    private void Awake()
    {
        Instance = this;
        
        questionArray = DataReader.GetQuestionArrayFromJson(Application.dataPath + QUESTIONS_PATH);
    }

    private void EndGame()
    {
        GameHandler.Instance.IsEndGame = true;
        OnEndGame?.Invoke(this, new OnEndGameEventArgs(){correctAnswersNumber = correctAnswersNumber});
        
        currentQuestionIndex = 0;
        correctAnswersNumber = 0;
    }

    public Question GetCurrentQuestion()
    {
        return questionArray[currentQuestionIndex];
    }

    public void AnswerQuestion(int answerIndex)
    {
        bool isCorrect = questionArray[currentQuestionIndex].answers[answerIndex].correct;
        if (isCorrect)
        {
            correctAnswersNumber++;
        }
        
        currentQuestionIndex++;
        OnQuestionAnswered?.Invoke(this, new OnQuestionAnsweredEventArgs(){isCorrect = isCorrect});

        if (currentQuestionIndex >= questionArray.Length)
        {
            EndGame();
        }
        
        GameHandler.Instance.NextState();
    }
    
}
