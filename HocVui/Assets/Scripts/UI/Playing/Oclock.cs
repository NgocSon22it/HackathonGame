using Assets.Scripts.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Oclock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int time;
    [SerializeField] GameObject bar;
    [SerializeField] GameObject barOclock;
    [SerializeField] GameObject TimeOut;
    [SerializeField] GameObject Timer;

    Coroutine timer;
    Coroutine text;
    private float DefaultWidth;
    private float DefaultHeight;
    private float DefaultGoc;
    private float goc;

    public static Oclock Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Init()
    {

        RectTransform rt = bar.GetComponent<RectTransform>();
        TimeOut.SetActive(false);
        Timer.SetActive(false);
        DefaultWidth = rt.rect.width;
        DefaultHeight = rt.rect.height;
        DefaultGoc = 360f / (float)time;
        goc = 270f;
    }

    public void run(int time)
    {
        this.time = time;
        Init();
        gameObject.SetActive(true);
        Timer.SetActive(true);
        timer = StartCoroutine(Counter(time));
    }

    public void close()
    {
        gameObject.SetActive(false);
        hideTimeOut();
        StopAllCoroutines();
    }

    private void handleBar(float second)
    {
        float newWidth = ((float)second / (float)time);
        bar.GetComponent<Image>().fillAmount = newWidth;
    }

    private void handeBarOclock(float second)
    {
        barOclock.transform.rotation = Quaternion.Euler(0f, 0f, second * DefaultGoc + goc);
    }

    private void showTimeOut()
    {
        TimeOut.SetActive(true);
        text = StartCoroutine(InfinityFade(0.45f));
        close();
    }

    IEnumerator InfinityFade(float duration)
    {
        while (true)
        {
            TimeOut.LeanScale(new Vector3(1.2f, 1.2f, 0), duration).setEase(LeanTweenType.pingPong);
            yield return new WaitForSeconds(duration);
            TimeOut.LeanScale(new Vector3(1, 1, 0), duration).setEase(LeanTweenType.pingPong);
            yield return new WaitForSeconds(duration);

        }
    }

    private void hideTimeOut()
    {
        TimeOut.SetActive(false);
        StopCoroutine(text);
        Playing_Manager.Instance.EndTimeAnswer();
    }
    IEnumerator Counter(float counter)
    {
        while (counter > 0)
        {
            yield return new WaitForSeconds(0.1f);
            counter -= 0.1f;
            handleBar(counter);
            handeBarOclock(counter);
            if (counter <= 0)
            {
                Timer.SetActive(false);
                showTimeOut();
                StopCoroutine(timer);
            }
        }
    }
}
