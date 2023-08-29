using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject MessageUI;
    [SerializeField] GameObject BXH_UI;
    [SerializeField] GameObject QuestionUI;
    [SerializeField] GameObject PopupUI;

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

    public void StartPopupResult()
    {
        PopupUI.GetComponent<PopupResult>().run();
    }
}
