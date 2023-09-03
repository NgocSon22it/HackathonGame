using Assets.Scripts.Common;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_AllUI : MonoBehaviour
{
    [Header("Setup")]
    public Canvas canvas;

    [Header("Pick up Pile")]
    [SerializeField] GameObject Message_Panel;
    [SerializeField] TMP_Text MainMessage;

    [Header("Answer")]
    [SerializeField] GameObject Answer_Panel;
    [SerializeField] TMP_Text Answer_Message;

    [Header("BuffInfo")]
    [SerializeField] GameObject BuffInfo_Panel;
    [SerializeField] TMP_Text BuffInfo_Message;

    [Header("Setup")]
    public GameObject PopupUI;

    Coroutine MessageCoroutine;

    public static Player_AllUI Instance;

    private void Awake()
    {
        Instance = this;
        // Set the render mode to "Screen Space - Overlay"
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Camera mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
        // Set the sorting layer to "MainPlayer"
        canvas.sortingLayerName = "MainPlayer";
    }

    public void Message_On(string message)
    {
        if (MessageCoroutine != null)
        {
            StopCoroutine(MessageCoroutine);
        }

        MessageCoroutine = StartCoroutine(Popup_Message(message));
    }

    IEnumerator Popup_Message(string message)
    {
        MainMessage.text = message;
        Message_Panel.SetActive(true);

        yield return new WaitForSeconds(3f);

        Message_Panel.SetActive(false);
    }

    public void SelectedAnswer_On(int answer)
    {
        Answer_Message.text = Message.Game_SelectedAnswer + ShowAnswer(answer);
        Answer_Panel.SetActive(true);
    }

    public void SelectedAnswer_Off()
    {
        Answer_Panel.SetActive(false);
    }

    public void BuffInfo_On(string info)
    {
        BuffInfo_Message.text += info + "\n";
        BuffInfo_Panel.SetActive(true);
    }

    public void BuffInfo_Off()
    {
        Answer_Panel.SetActive(false);
    }

    public string ShowAnswer(int answer)
    {
        switch(answer)
        {
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "D";
        }
        return "0";
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void StartPopupResult(bool isCorrect, int score)
    {
        PopupUI.GetComponent<PopupResult>().run   (isCorrect, score);
    }

}
