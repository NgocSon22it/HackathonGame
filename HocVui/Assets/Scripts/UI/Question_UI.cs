using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_UI : MonoBehaviour
{
    [SerializeField] GameObject PreviousView;
    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;

    private void Start()
    {
      /*  transform.position = new Vector3(transform.position.x, PreviousView.transform.position.y + 1920, 0); */
    }
    public void fadeLeft()
    {
        Debug.Log(PreviousView.transform.position.x);
        this.gameObject.LeanMoveLocalX(PreviousView.transform.position.x - 1920, duration);
    }


    public void fadeRight()
    {
        this.gameObject.LeanMoveLocalX(gameObject.transform.position.x + 1080, duration);
    }
}
