using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;

public class Chat_Manager : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;

    [SerializeField] TMP_Text ChatDisPlay;
    [SerializeField] TMP_InputField ChatField;

    [SerializeField] GameObject TypingChatObject;

    [SerializeField] ScrollRect scrollRect;

    public string ServerName;

    public bool IsTypingChat;

    string CurrentChat;

    public static Chat_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        isConnected = true;
        chatClient.Subscribe(new string[] { ServerName });
        Debug.Log("Kết nối thánh công!");
    }

    public void OnDisconnected()
    {
        isConnected = false;
    }

    public void DisconnectFromChat()
    {
        if (chatClient != null && chatClient.CanChat)
        {
            chatClient.Disconnect();
            ClearChat();
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string disPlayer;
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Get the current time as a string in the format "HH:mm"
        string currentTimeString = currentTime.ToString("HH:mm");

        for (int i = 0; i < senders.Length; i++)
        {
            var message = messages[i].ToString().Split(new char[] { ':' });

            var type = message[0];
            var content = message[1];

            if (type.Equals(References.Chat_ServerName))
            {
                disPlayer = string.Format("[{2}] {0}: {1}", senders[i], content, currentTimeString);

                ChatDisPlay.text += disPlayer + "\n";

                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0;
            }
        }
    }

    public void ClearChat()
    {
        ChatDisPlay.text = "";
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        TypingChatObject.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    [SerializeField] string userID;
    bool isConnected;

    public void ConnectToChat(string ServerName)
    {
        this.ServerName = ServerName;
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));
    }

    public void TypeChatOnValueChange(string value)
    {
        CurrentChat = value;
    }

    public void SummitPublicChat()
    {
        if (!string.IsNullOrEmpty(CurrentChat))
        {
            chatClient.PublishMessage(ServerName, string.Format(Message.PublicMessage, ServerName, CurrentChat));
            ChatField.text = "";
            CurrentChat = "";
        }
    }



    public void ToggleTyping(bool value)
    {
        IsTypingChat = value;
    }

    public void FocusTyping()
    {
        ChatField.Select();
        ChatField.ActivateInputField();
    }


    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
            if (Input.GetKeyDown(KeyCode.Return) && IsTypingChat == false)
            {
                ToggleTyping(true);
                FocusTyping();
            }
            else if (string.IsNullOrEmpty(CurrentChat) && Input.GetKeyDown(KeyCode.Return) && IsTypingChat == true)
            {
                ToggleTyping(false);
            }
        }

        if (!string.IsNullOrEmpty(CurrentChat) && Input.GetKeyDown(KeyCode.Return) && IsTypingChat == true)
        {
            SummitPublicChat();
            FocusTyping();
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new NotImplementedException();
    }
}
