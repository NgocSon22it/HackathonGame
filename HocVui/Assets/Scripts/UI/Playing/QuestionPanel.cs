using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class QuestionPanel : MonoBehaviour
{
    [SerializeField] GameObject QuestionField;
    [SerializeField] GameObject title;
    [SerializeField] GameObject ans1;
    [SerializeField] GameObject ans2;
    [SerializeField] GameObject ans3;
    [SerializeField] GameObject ans4;
    [SerializeField] GameObject image;

    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;
    [SerializeField] float distance;

    [SerializeField] LeanTweenType typeFadeInImage;
    [SerializeField] LeanTweenType typeFadeOutImage;

    [SerializeField] float defaultTime;
    [SerializeField] TMP_Text TextCount;
    Coroutine Timer;

    Coroutine ShowQuiz;
    Coroutine HideQuiz;

    private float time;


    public void ShowQuestion()
    {
        time = defaultTime;
        ShowQuiz = StartCoroutine(showAnswers());
        ShowImage(1);
        Timer = StartCoroutine(counter());
    }

    public void HideQuestion()
    {
        StopCoroutine(ShowQuiz);
        HideQuiz = StartCoroutine(hideAnswers());
        HideImage(0);
    }
    public void TitleMoveX(float derection)
    {
        LeanTween.moveLocalX(title, derection * distance, duration).setEase(type);

    }

    IEnumerator showAnswers()
    {
        float WaitingTime = 0.2f;

        LeanTween.moveLocalX(title, distance, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans1, distance, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans2, distance, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans3, distance, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans4, distance, duration).setEase(type);
    }

    IEnumerator hideAnswers()
    {
        float WaitingTime = 0.2f;
        LeanTween.alpha(image, 0, 1);
        LeanTween.moveLocalX(title, 0, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans1, 0, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans2, 0, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans3, 0, duration).setEase(type);
        yield return new WaitForSeconds(WaitingTime);
        LeanTween.moveLocalX(ans4, 0, duration).setEase(type);
    }
        
    private void AnswersMove(int derection, GameObject ans)
    {
        LeanTween.moveLocalX(ans, derection * distance, duration).setEase(type);
    }
    public void ShowImage(float scale)
    {
      
        LeanTween.scale(image, new Vector3(scale, scale, 0), duration).setEase(typeFadeInImage);
    }

    public void HideImage(float scale)
    {
        LeanTween.scale(image, new Vector3(scale, scale, 0), duration).setEase(typeFadeOutImage);
    }

    private void timeFadeIn()
    {
        LeanTween.scale(TextCount.gameObject, new Vector3(1.5f, 1.5f, 0), 0.5f).setEase(LeanTweenType.punch);
        Invoke(nameof(timeFadeOut), 0.5f);
    }

    private void timeFadeOut()
    {
        LeanTween.scale(TextCount.gameObject, new Vector3(1, 1, 0), 0.5f).setEase(LeanTweenType.pingPong);
    }

    IEnumerator counter()
    {

        while (time > 0)
        {
            TextCount.text = string.Format("{0}", time);
            timeFadeIn();
            yield return new WaitForSeconds(1f);
            time--;
        }

        if (time <= 0)
        {
            HideQuestion();
            TextCount.text = string.Format("Hết giờ", time);
        }
    }
}
