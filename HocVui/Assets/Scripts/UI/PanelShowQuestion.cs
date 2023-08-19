using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelShowQuestion : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;
    [SerializeField] float time;

    [SerializeField] TMP_Text TextCount;
    Coroutine startTimer;

    [SerializeField] Image objImage;

    private void Start()
    {
        objImage.transform.position = new Vector3(0f, 1000f, 0f);
        open();

        startTimer = StartCoroutine(counter());
    }

    public void open()
    {
        objImage.transform.position = new Vector3(0f, 1000f, 0f);
        LeanTween.moveLocalY(objImage.gameObject, 0f, duration).setEase(typeOpen);
    }

     public void close()
    {
        LeanTween.moveLocalY(objImage.gameObject, -1000f, duration).setEase(typeClose);
    }

    IEnumerator counter()
    {
        
       while(time > 0)
        {
            TextCount.text = string.Format("{0}", time);
            yield return new WaitForSeconds(1f);
            time--;
        }

       if(time <= 0)
        {
            close();
        }
    }
}
