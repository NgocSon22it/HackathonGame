using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;

    public void open()
    {
        LeanTween.scale(gameObject, Vector3.one, duration).setEase(typeOpen);
    }

    public void close()
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
}
