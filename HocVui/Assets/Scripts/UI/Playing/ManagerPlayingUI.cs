using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ManagerPlayingUI : MonoBehaviour
{
    public GameObject MessageUI;
    public GameObject BXH_UI;
    public GameObject QuestionUI;
  

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
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void StartMessageEvent()
    {
        MessageUI.GetComponent<PanelShowMassage>().StartAnimation();
    }

    public void StartQuestionEvent()
    {
        QuestionUI.GetComponent<QuestionPanel>().ShowQuestion();
    }

    
}
