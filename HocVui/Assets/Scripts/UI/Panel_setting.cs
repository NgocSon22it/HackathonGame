using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_setting : MonoBehaviour
{
    [SerializeField] GameObject ojb;
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeOpen;
    [SerializeField] LeanTweenType typeClose;
    [SerializeField] GameObject menu;
    

    public void fadeIn()
    {
        ojb.SetActive(true);
        menu.transform.localScale = Vector3.zero;
        LeanTween.scale(menu, Vector3.one, duration).setEase(typeOpen);

        //StartCoroutine(Waiting5f());
    }

    public void fadeOut()
    {
        LeanTween.scale(menu, Vector3.zero, duration).setEase(typeClose);
        Invoke(nameof(close), duration);
    }

    //IEnumerator Waiting5f()
    //{
    //    yield return new WaitForSeconds(5f);
    //    Playing_Manager.Instance.EndListResult();

    //}

    public void open()
    {
        ojb.SetActive(true);
    }

    public void close()
    {
        ojb.SetActive(false);
    }

}