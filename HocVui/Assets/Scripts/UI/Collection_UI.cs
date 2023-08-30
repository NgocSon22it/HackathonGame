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
        gameObject.LeanMoveLocalY(0, duration).setEase(typeFadeIn);
    }

    public void fadeDown()
    {
        gameObject.LeanMoveLocalY(-1080, duration).setEase(typeFadeOut);
    }

    public void fadeLeft()
    {
        if (Lobby_Manager.Instance.IsCollectionNameValid())
        {
            gameObject.LeanMoveLocalX(-960f, duration).setEase(typeFadeIn);
        }
        else
        {
            Lobby_Manager.Instance.MessagePanel_On("Hãy nhập tên bộ câu hỏi!");
        }
    }

    public void fadeRight()
    {
        gameObject.LeanMoveLocalX(1920, duration).setEase(typeFadeOut);
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
