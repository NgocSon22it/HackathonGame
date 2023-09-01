using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupResult : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] float duration;
    [SerializeField] float timeShow;
    [SerializeField] LeanTweenType type;

    [SerializeField] float score;
    [SerializeField] float totalScore;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text totalScoreText;

    public void run()
    {
        ui.SetActive(true);
        ScoreText();
        scoreText.transform.localPosition = new Vector3(0, -40, 0);
        scoreText.gameObject.SetActive(true);
        StartCoroutine(startAnimation());
    }
    IEnumerator startAnimation()
    {
        ui.SetActive(true);
        FadeIn();
        yield return new WaitForSeconds(duration);
        StartCoroutine (FadeDownScore());
        yield return new WaitForSeconds(timeShow);
        StartCoroutine(FadeOut());
    }

    private void FadeIn()
    {
        LeanTween.scale(ui, Vector3.one, duration).setEase(type);
    }

    private void ScoreText()
    {
        scoreText.text = "+"+ score.ToString() + " Điểm";
        totalScoreText.text = totalScore.ToString() + " Điểm";
    }

    private void handleTotal()
    {
        totalScore += score;
        ScoreText();
    }

    IEnumerator FadeDownScore()
    {
        yield return new WaitForSeconds(duration);
        LeanTween.moveLocalY(scoreText.gameObject, -120f, duration).setEase(LeanTweenType.easeInQuint);
        yield return new WaitForSeconds(duration);
        scoreText.gameObject.SetActive(false);
        handleTotal();
    }


    IEnumerator FadeOut()
    {
        LeanTween.scale(ui, Vector3.zero, duration).setEase(type);
        yield return new WaitForSeconds(duration);
        ui.SetActive(false);
    }
}
