using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalScale : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeFadeIn;
    [SerializeField] LeanTweenType typeFadeOut;
    [SerializeField] GameObject Modal;

    public void open()
    {
        gameObject.SetActive(true);
        LeanTween.scale(Modal, Vector3.one, duration).setEase(typeFadeIn);
    }

    public void close()
    {
        LeanTween.scale(Modal, Vector3.zero, duration).setEase(typeFadeOut);
        Invoke(nameof(hide), duration);
    }

    private void hide()
    {
        gameObject.SetActive(false);
    }


}
