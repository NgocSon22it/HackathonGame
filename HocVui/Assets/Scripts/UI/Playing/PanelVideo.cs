using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelVideo : MonoBehaviour
{
    [SerializeField] float timeshow;
    [SerializeField] GameObject menu;
    [SerializeField] float duration;

    public void show()
    {
        gameObject.SetActive(true);
        menu.LeanScale(Vector3.one, duration).setEase(LeanTweenType.pingPong);
        Invoke(nameof(fadeOut), timeshow);
        
    }

    private void fadeOut()
    {
        menu.LeanScale(Vector3.zero, duration).setEase(LeanTweenType.pingPong);
        Invoke(nameof(hide), duration);
    }
    private void hide()
    {
        gameObject.SetActive(false);
    }

}
