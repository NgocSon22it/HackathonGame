using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBtn : MonoBehaviour
{
    public void OnHover()
    {
        gameObject.LeanScale(new Vector3(1.1f, 1.1f, 0), 0.2f).setEase(LeanTweenType.pingPong);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -20f));
    }

    public void UnHover()
    {
        gameObject.LeanScale(Vector3.one, 0.2f).setEase(LeanTweenType.pingPong);
        gameObject.transform.rotation = Quaternion.Euler(Vector3.one);
    }
}
