using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Question_Entity
{
    public string questionText;
    public List<string> answers;
    public int correctAnswerIndex;
    public Texture rawImage;
    public string LinkImage;
    public int Time;

    public Question_Entity(string question, List<string> answerChoices, int correctIndex, Texture rawImage, string linkImage)
    {
        questionText = question;
        answers = answerChoices;
        correctAnswerIndex = correctIndex;
        this.rawImage = rawImage;
        LinkImage = linkImage;
    }
}
