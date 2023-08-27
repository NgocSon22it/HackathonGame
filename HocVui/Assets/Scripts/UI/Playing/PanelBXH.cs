using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBXH : MonoBehaviour
{

    [SerializeField] GameObject panel;
    [SerializeField] GameObject panelModal;
    [SerializeField] float timeShow;
    [SerializeField] float timeHide;
    [SerializeField] LeanTweenType fadeInType;
    [SerializeField] LeanTweenType fadeOutType;

    public void open()
    {
        ModalFadeIn();
    }

    public void  close()
    {
       StartCoroutine(ModalFadeOut());
    }

    private void ModalFadeIn()
    {
        gameObject.SetActive(true);
        LeanTween.scale(panelModal.gameObject , Vector3.one, timeShow).setEase(fadeInType);
    }

    IEnumerator ModalFadeOut()
    {
        LeanTween.scale(panelModal.gameObject, Vector3.zero, timeHide).setEase(fadeOutType);
        yield return new WaitForSeconds(timeHide);
        gameObject.SetActive(false);
    }
}
