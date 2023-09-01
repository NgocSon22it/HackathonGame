using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShowMassage : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] float durationIn;
    [SerializeField] float durationOut;
    [SerializeField] LeanTweenType AnimationIn;
    [SerializeField] LeanTweenType AnimationOut;

    Coroutine coroutine;
    public void StartAnimation()
    {
        message.SetActive(true);
        coroutine = StartCoroutine(animationShow());
    }

    IEnumerator animationShow()
    {
        message.transform.position = new Vector3(-960f, 540f, 0);
        LeanTween.moveX(message, 960f, durationIn).setEase(AnimationIn);
        yield return new WaitForSeconds(durationIn);
        LeanTween.moveX(message, 2780f, durationOut).setEase(AnimationOut);
        yield return new WaitForSeconds(durationOut);
        gameObject.SetActive(false);
        Playing_Manager.Instance.EndMessage();
    }
}
