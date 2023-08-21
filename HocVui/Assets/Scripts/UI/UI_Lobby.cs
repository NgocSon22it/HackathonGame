using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;


    public void FadeIn(float time)
    {
        show();
        Invoke(nameof(open), time);
    }

    public void FadeOut(float time)
    {
        close();
        Invoke(nameof(hide), time);
    }

    private void open()
    {
        LeanTween.scale(gameObject, Vector3.one, duration).setEase(typeOpen);
    }

    private void close()
    {
        LeanTween.scale(gameObject, Vector3.zero, duration).setEase(typeOpen);
    }

    public void moveTop()
    {
        LeanTween.moveLocalY(gameObject, 1080f, duration).setEase(typeOpen);
    }

    public void moveDefaultY() { 
        LeanTween.moveLocalY(gameObject, 0, duration).setEase(typeClose);
    }

    private void show()
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero;
    }

    private void hide()
    {
        gameObject.SetActive(false);
    }
}
