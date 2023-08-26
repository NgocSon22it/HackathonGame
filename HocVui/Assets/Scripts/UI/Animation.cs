using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Animation : MonoBehaviour
{

    [SerializeField] GameObject obj;
    [SerializeField] LeanTweenType type;
    [SerializeField] float duration;

    public void fadeInScale(float scale)
    {
        LeanTween.scale(gameObject, new Vector3(scale, scale, 0), duration).setEase(type);
    }
        
    public void fadeOutScale(float scale)
    {
        LeanTween.scale(gameObject, new Vector3(scale, scale, 0), duration).setEase(type);
    }
}
