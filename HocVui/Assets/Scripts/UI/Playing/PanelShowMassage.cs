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

    private void Start()
    {
        resetPositon();
    }
    public void StartAnimation()
    {
        message.SetActive(true);
        coroutine = StartCoroutine(animationShow());
    }

    IEnumerator animationShow()
    {
        resetPositon();
        LeanTween.moveLocal(message, new Vector3(150, 0, 0), durationIn).setEase(AnimationIn);
        yield return new WaitForSeconds(durationIn);
        LeanTween.moveLocal(message, new Vector3(1920, 0, 0), durationOut).setEase(AnimationOut);
        yield return new WaitForSeconds(durationOut);
        StopAnimation();

    }

    private void StopAnimation()
    {
        gameObject.SetActive(false);
        StopCoroutine(coroutine);

        Playing_Manager.Instance.EndMessage();
    }

    private void resetPositon()
    {
        gameObject.transform.position = new Vector3(-1920f, 0, 0);
    }
}