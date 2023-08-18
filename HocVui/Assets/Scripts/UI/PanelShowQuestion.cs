using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShowQuestion : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;
    [SerializeField] float time;
    private void Start()
    {
        setDefault();
    }
    public void open()
    {
        LeanTween.moveLocalY(gameObject, 0, duration).setEase(typeOpen);
    }

    public void close()
    {
        LeanTween.moveLocalY(gameObject, -1000, duration).setEase(typeClose);
        setDefault();
    }

    private void setDefault()
    {
        gameObject.transform.position = new Vector3(0, 1000, 0);
    }

    IEnumerator counter()
    {

        yield return new WaitForSeconds(1f);
    }
}
