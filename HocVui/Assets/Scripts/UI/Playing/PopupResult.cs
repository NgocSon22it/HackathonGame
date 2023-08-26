using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupResult : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] float duration;
    [SerializeField] float timeShow;
    [SerializeField] LeanTweenType type;

    public void run()
    {
        ui.SetActive(true);
        StartCoroutine(startAnimation());
    }

    IEnumerator startAnimation()
    {
        ui.SetActive(true);
        FadeIn();
        yield return new WaitForSeconds(timeShow);
        StartCoroutine(FadeOut());
    }

    private void FadeIn()
    {
        LeanTween.scale(ui, Vector3.one, duration).setEase(type);
    }

    IEnumerator FadeOut()
    {
        LeanTween.scale(ui, Vector3.zero, duration).setEase(type);
        yield return new WaitForSeconds(duration);
        ui.SetActive(false);
    }
}
