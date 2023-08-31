using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{


    public void startEvent()
    {
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 0), 0.2f).setEase(LeanTweenType.pingPong);
    }

    public void endEvent()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.2f).setEase(LeanTweenType.pingPong);
    }

}
