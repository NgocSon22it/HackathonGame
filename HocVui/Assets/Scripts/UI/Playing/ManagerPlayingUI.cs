using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ManagerPlayingUI : MonoBehaviour
{
    public GameObject MessageUI;
    public GameObject BXH_UI;
    public GameObject QuestionUI;
    public GameObject PopupUI;

    public static ManagerPlayingUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPanelBXH()
    {
        BXH_UI.GetComponent<PanelBXH>().StartAnimation();
    }

    public void HidePanelBXH()
    {
        BXH_UI.GetComponent<PanelBXH>().StopAnimation();
    }
    public void StartMessageEvent()
    {
        MessageUI.GetComponent<PanelShowMassage>().StartAnimation();
    }

    public void StartQuestionEvent()
    {
        QuestionUI.GetComponent<QuestionPanel>().ShowQuestion();
    }

    public void StartPopupResult(bool isCorrect, int score)
    {
        PopupUI.GetComponent<PopupResult>().run(isCorrect, score);
    }
}
