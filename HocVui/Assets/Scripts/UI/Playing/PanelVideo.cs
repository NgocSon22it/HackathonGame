using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class PanelVideo : MonoBehaviour
{
    public VideoPlayer VideoObj;
    [SerializeField] float timeshow;
    [SerializeField] GameObject menu;
    [SerializeField] float duration;

    public void show(string LinkVideo)
    {
        //gameObject.SetActive(true);
        VideoObj.source = VideoSource.Url;
        VideoObj.url = LinkVideo;
        gameObject.SetActive(true);
        VideoObj.Prepare();
        VideoObj.Play();
        VideoObj.loopPointReached += OnVideoEnd;
        menu.LeanScale(Vector3.one, duration).setEase(LeanTweenType.pingPong);
        //Invoke(nameof(fadeOut), 5f);
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        StartCoroutine(CloseVideo());
    }

    IEnumerator CloseVideo()
    {
        yield return new WaitForSeconds(1f);
        fadeOut();
    }

    public void fadeOut()
    {
        menu.LeanScale(Vector3.zero, duration).setEase(LeanTweenType.pingPong);
        Invoke(nameof(hide), duration);
    }

    private void hide()
    {
        gameObject.SetActive(false);
        Playing_Manager.Instance.CloseVideo();
    }

}
