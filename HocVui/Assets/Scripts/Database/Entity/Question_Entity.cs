using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question_Entity
{
    public string questionText;
    public List<string> answers;
    public int correctAnswerIndex;

    public Question_Entity(string question, List<string> answerChoices, int correctIndex)
    {
        questionText = question;
        answers = answerChoices;
        correctAnswerIndex = correctIndex;
    }
}
