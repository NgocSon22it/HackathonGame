﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection_UI : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeFadeOut;
    [SerializeField] LeanTweenType typeFadeIn;
    [SerializeField] LeanTweenType typeClose;

    public static Collection_UI Instances;

    private void Awake()
    {
        Instances = this;
    }
    public void fadeTop()
    {
        gameObject.LeanMoveLocalY(0, duration).setEase(typeFadeIn);
    }

    public void fadeDown()
    {
        gameObject.LeanMoveLocalY(-1080, duration).setEase(typeFadeOut);
    }

    public void fadeLeft()
    {
       
            gameObject.LeanMoveLocalX(-960f, duration).setEase(typeFadeIn);
     
    }

    public void fadeRight()
    {
        gameObject.LeanMoveLocalX(960, duration).setEase(typeFadeOut);
    }

    public void close()
    {
        this.resetPosition();
    }

    private void resetPosition()
    {
        this.transform.position = new Vector3(1920, -540, 0);
    }
}
