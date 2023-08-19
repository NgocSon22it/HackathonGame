using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelShowQuestion : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;
    [SerializeField] float time;

    [SerializeField] TMP_Text TextCount;
    Coroutine startTimer;
    private void Start()
    {
        setDefault();
        startTimer = StartCoroutine(counter());
    }
    public void open()
    {
        LeanTween.moveLocal(gameObject, new Vector3(0, 0, 0), duration);
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
        
       while(time >=0)
        {
            TextCount.text = string.Format("{0}", time);
            yield return new WaitForSeconds(1f);
            time--;
        }

       if(time <0) {
            close();
        }
    }
}
