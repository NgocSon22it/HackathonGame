using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_setting : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;

    private void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
    public void open()
    {
        LeanTween.scale(gameObject, Vector3.one, duration).setEase(typeOpen);
    }

    public void close() { 
        LeanTween.scale(gameObject, Vector3.zero, duration).setEase(typeClose);
    }
}
