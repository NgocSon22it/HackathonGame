using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection_UI : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType typeFadeOut;
    [SerializeField] LeanTweenType typeFadeIn;
    [SerializeField] LeanTweenType typeClose;
    public void fadeTop()
    {
        gameObject.LeanMoveY(this.transform.position.y + 1080, duration).setEase(typeFadeIn);
    }

    public void fadeDown()
    {
        gameObject.LeanMoveY(this.transform.position.y - 1080, duration).setEase(typeFadeOut);
    }

    public void fadeLeft()
    {
        gameObject.LeanMoveX(this.transform.position.x - 1920, duration).setEase(typeFadeIn);
    }

    public void fadeRight()
    {
        gameObject.LeanMoveX(this.transform.position.x + 1920, duration).setEase(typeFadeOut);
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
