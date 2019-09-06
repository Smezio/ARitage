using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MultiChoice")]
public class MultiChoiceStruct : ScriptableObject
{
    [SerializeField] private string question;
    [SerializeField] private string[] answers;
    [SerializeField] private int correctAns;

    public string Question
    {
        get { return question; }
    }

    public string[] Answers
    {
        get { return answers; }
    }

    public string AnswerByNum(int number)
    {
        return answers[number];
    }

    public int CorrectAnswer
    {
        get { return correctAns; }
    }
}
