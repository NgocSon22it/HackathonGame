using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;

    public void open()
    {
        FadeIn();
    }

    public void close()
    {
        StartCoroutine(FadeOut());
    }

    private void  FadeIn()
    {
        ui.SetActive(true);
        LeanTween.scale(ui, Vector3.one, duration).setEase(type);
    }

    IEnumerator FadeOut()
    {
        LeanTween.scale(ui, Vector3.zero, duration).setEase(type);
        yield return new WaitForSeconds(duration);
        ui.SetActive(false);
    }
}
