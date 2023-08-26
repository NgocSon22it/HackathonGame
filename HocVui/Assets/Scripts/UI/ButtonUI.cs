using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject btn;
    [SerializeField] float scaleValue;
    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;


    public void startEvent()
    {
        LeanTween.scale(btn, new Vector3(scaleValue, scaleValue, 0), duration).setEase(type);
    }

    public void endEvent()
    {
        LeanTween.scale(btn, Vector3.one, duration).setEase(type);
    }

}
