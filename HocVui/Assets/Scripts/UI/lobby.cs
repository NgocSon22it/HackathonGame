using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobby : MonoBehaviour
{

    [SerializeField] GameObject scene;
    [SerializeField] LeanTweenType type;
    [SerializeField] float time;

    private void Start()
    {
       
    }

    public void moveRight()
    {
        LeanTween.moveLocalX(scene, -960f, time);
    }

    public void moveLeft()
    {
        LeanTween.moveLocalX(scene, 960f, time);
    }
}
