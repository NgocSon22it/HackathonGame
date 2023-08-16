using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] float scaleValue;
    [SerializeField] float durationValue;

    private void Start()
    {
        obj = GetComponent<GameObject>();
     
    }
    
    void fadeInScale()
    {
        LeanTween.scale(obj, new Vector3(scaleValue, scaleValue, 0), durationValue);
    }

}
