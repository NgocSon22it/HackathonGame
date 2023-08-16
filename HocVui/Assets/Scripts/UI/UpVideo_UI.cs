using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpVideo_UI : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;

    public void fadeTop()
    {
        gameObject.LeanMoveY(this.transform.position.y + 1080, duration).setEase(type);
    }

    public void fadeDown()
    {
        gameObject.LeanMoveY(this.transform.position.y - 1080, duration).setEase(type);
    }
}
