using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Question : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;

    private void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
    public void open()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 0), duration).setEase(type);
    }

    public void close()
    {
        LeanTween.scale(gameObject, Vector3.zero, duration).setEase(type);
    }
}
