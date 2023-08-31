using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelVideo : MonoBehaviour
{
    [SerializeField] float timeshow;
    [SerializeField] GameObject menu;
    public float time;

    public void show()
    {
        gameObject.SetActive(true);
        menu.LeanScale(Vector3.one, 0.4f).setEase(LeanTweenType.pingPong);
    }



    IEnumerator hide()
    {
        while (true)
        {

            yield return new WaitForSeconds(1f);
        }
    }
}
