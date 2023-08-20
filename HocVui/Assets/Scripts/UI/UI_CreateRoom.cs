using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreateRoom : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;

    public void open()
    {
        LeanTween.moveLocalX(gameObject, -960f, duration).setEase(typeOpen);
    }

    public void close()
    {
        LeanTween.moveLocalX(gameObject, 960f, duration).setEase(typeClose);
    }
}
