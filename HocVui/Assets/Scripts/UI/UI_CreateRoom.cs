using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreateRoom : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;

    public void openTimeOut(float time)
    {
        show();
        Invoke(nameof(open), time);
    }

    public void closeTimeOut(float time)
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
        LeanTween.scale(gameObject, Vector3.zero, duration).setEase(typeClose);
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
