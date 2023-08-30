using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Manager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Header("Instance")]
    public static Lobby_Manager Instance;

    [Header("Lobby")]
    [SerializeField] GameObject InRoom;
    [SerializeField] GameObject Outside;
    Player[] listPlayers;

    [Header("Outside")]
    [Header("List Room")]
    RoomInfo SelectedRoom;
    [SerializeField] GameObject Room_Item;
    [SerializeField] Transform Room_Content;
    public static Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    List<RoomInfo> roomList = new List<RoomInfo>();
    List<RoomInfo> checkList = new List<RoomInfo>();

    [SerializeField] TMP_InputField CreateCollection_NameInput;
    [SerializeField] GameObject CreateCollection_MessagePanel;


    [Header("Create Room")]
    [SerializeField] TMP_InputField CreateRoom_NameInput, CreateRoom_PasswordInput;

    [SerializeField] Slider CreateRoom_NumberPlayerSlider, CreateRoom_AnswerTimeSlider;
    [SerializeField] TMP_Text CreateRoom_NumberPlayerTxt, CreateRoom_AnswerTimeTxt;
    [SerializeField] Toggle CreateRoom_UseSpellToggle, CreateRoom_UsePassword;
    bool CreateRoomWithPassword;

    [Header("Question Information")]
    [SerializeField] TMP_Text Information_Question;
    [SerializeField] List<TMP_Text> Information_Answer;
    [SerializeField] List<Image> Information_IsCorrect;
    [SerializeField] List<Image> Information_IsCorrectColor;

    [Header("Find Room")]
    [SerializeField] TMP_InputField FindRoom_NameInput;
    string keyword;

    [Header("In room")]
    [SerializeField] TMP_Text Inroom_RoomNameTxt;
    [SerializeField] TMP_Text Inroom_PlayerQuantityTxt;
    [SerializeField] TMP_Text Inroom_RoomIDtxt;
    [SerializeField] List<Image> ListSpell;
    [SerializeField] GameObject StartBtn;


    [Header("SpellInformation")]
    [SerializeField] GameObject Spell_Panel;
    [SerializeField] GameObject SpellInformation_Panel;
    [SerializeField] TMP_Text SpellInformation_Name;
    [SerializeField] TMP_Text SpellInformation_Description;


    [Header("List Player")]
    [SerializeField] GameObject PlayerItem;
    [SerializeField] Transform PlayerContent;

    [Header("Paging")]
    [SerializeField] TMP_Text CurrentPaging;
    int CurrentIndex;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ConnectServer();
    }

    public void JoinRoom()
    {
        if (SelectedRoom != null)
        {
            if (SelectedRoom.CustomProperties.ContainsKey("Password"))
            {
                //OpenRoomPasswordPanel();
                Debug.Log("Co Pass");
            }
            else
            {
                PhotonNetwork.JoinRoom(SelectedRoom.Name);
            }
        }
        else
        {
            Debug.Log("K ton tai room");
        }
    }

    public void CreateRoom_OnPasswordToggleValueChange(bool value)
    {
        CreateRoom_PasswordInput.gameObject.SetActive(value);
    }

    public void CreateRoom()
    {
        string roomID = References.GenerateRandomString(5);
        string roomName = CreateRoom_NameInput.text;
        int NumberPlayer = Convert.ToInt32(CreateRoom_NumberPlayerSlider.value) + 1;
        int AnswerTime = Convert.ToInt32(CreateRoom_AnswerTimeSlider.value);
        string map = "Sông Bạch Đằng";
        bool useSpell = CreateRoom_UseSpellToggle.isOn;
        bool usePassword = CreateRoom_UsePassword.isOn;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)NumberPlayer;
        roomOptions.BroadcastPropsChangeToAll = true;

        if (roomName.Length > 0)
        {
            if (usePassword)
            {
                string roomPassword = CreateRoom_PasswordInput.text;

                if (roomPassword.Length > 0)
                {
                    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
                    roomOptions.CustomRoomPropertiesForLobby = new string[] { "AnswerTime", "Creator", "Map", "RoomID", "UseSpell", "Password" };

                    roomOptions.CustomRoomProperties.Add("RoomID", roomID);
                    roomOptions.CustomRoomProperties.Add("Creator", PhotonNetwork.NickName);
                    roomOptions.CustomRoomProperties.Add("AnswerTime", AnswerTime);
                    roomOptions.CustomRoomProperties.Add("Map", map);
                    roomOptions.CustomRoomProperties.Add("UseSpell", useSpell);
                    roomOptions.CustomRoomProperties.Add("Password", roomPassword);
                    PhotonNetwork.CreateRoom(roomName, roomOptions);
                }
                else
                {

                }

            }
            else
            {
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "AnswerTime", "Creator", "Map", "RoomID", "UseSpell" };

                roomOptions.CustomRoomProperties.Add("RoomID", roomID);
                roomOptions.CustomRoomProperties.Add("Creator", PhotonNetwork.NickName);
                roomOptions.CustomRoomProperties.Add("AnswerTime", AnswerTime);
                roomOptions.CustomRoomProperties.Add("Map", map);
                roomOptions.CustomRoomProperties.Add("UseSpell", useSpell);
                PhotonNetwork.CreateRoom(roomName, roomOptions);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        SetRandomSpell();
        SetUp_Inroom(true);

        References.Chat_ServerName = PhotonNetwork.CurrentRoom.Name;

        Chat_Manager.Instance.DisconnectFromChat();
        Chat_Manager.Instance.ConnectToChat(References.Chat_ServerName);

        Debug.Log(References.Chat_ServerName);
        foreach (Transform trans in PlayerContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < listPlayers.Length; i++)
        {
            if (listPlayers[i].IsMasterClient == false)
            {
                Instantiate(PlayerItem, PlayerContent).GetComponent<Player_Item>().SetUp(listPlayers[i]);
            }
        }

        Debug.Log("JoinRoom");

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Inroom_CountPlayer();
        Instantiate(PlayerItem, PlayerContent).GetComponent<Player_Item>().SetUp(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Inroom_CountPlayer();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Room join failed with error code {returnCode} and error message {message}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Room join failed with error code {returnCode} and error message {message}");
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        PhotonNetwork.LeaveRoom();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Chat_Manager.Instance.DisconnectFromChat();
        SelectedRoom = null;
        SetUp_Inroom(false);
        cachedRoomList.Clear();
        Debug.Log("LeftRoom");
    }

    public void SetUp_Inroom(bool value)
    {
        if (value)
        {
            Inroom_RoomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
            Inroom_CountPlayer();

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RoomID"))
            {
                Inroom_RoomIDtxt.text = PhotonNetwork.CurrentRoom.CustomProperties["RoomID"].ToString();
            }
        }

        InRoom.SetActive(value);
        Outside.SetActive(!value);
    }

    public void Inroom_CountPlayer()
    {
        listPlayers = PhotonNetwork.PlayerList;
        Inroom_PlayerQuantityTxt.text = $"{listPlayers.Length - 1} Người chơi";
    }

    public void CreateRoom_OnNumberPlayerChange()
    {
        CreateRoom_NumberPlayerTxt.text = CreateRoom_NumberPlayerSlider.value.ToString();
    }

    public void CreateRoom_OnAnswerTimeChange()
    {
        CreateRoom_AnswerTimeTxt.text = CreateRoom_AnswerTimeSlider.value.ToString() + "s";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in Room_Content)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }


        checkList.Clear();
        foreach (RoomInfo room in cachedRoomList.Values)
        {
            checkList.Add(room);
        }

        InitRoomList(checkList);

        foreach (KeyValuePair<string, RoomInfo> entry in cachedRoomList)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                Instantiate(Room_Item, Room_Content).GetComponent<Room_Item>().SetUp(cachedRoomList[entry.Key]);
            }
            else
            {
                if (entry.Value.CustomProperties.ContainsKey("RoomID"))
                {
                    if (entry.Value.CustomProperties["RoomID"].ToString().Contains(keyword))
                    {
                        Instantiate(Room_Item, Room_Content).GetComponent<Room_Item>().SetUp(cachedRoomList[entry.Key]);
                    }
                }
            }
        }
    }

    public void InitRoomList(List<RoomInfo> roomList)
    {
        this.roomList.Clear();
        this.roomList = roomList;
    }

    public void Refresh()
    {
        keyword = "";
        FindRoom_NameInput.text = "";
        OnRoomListUpdate(roomList);
    }

    public void Findroom_OnKeyWordChange(string value)
    {
        keyword = value;
    }

    public void FindRoom()
    {
        OnRoomListUpdate(roomList);
    }

    public void SelectRoom(RoomInfo roomInfo)
    {
        SelectedRoom = roomInfo;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("OnConnectedToMaster");
    }

    public void ConnectServer()
    {
        PhotonNetwork.NickName = References.GenerateRandomString(10);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.EnableCloseConnection = true;
        PhotonNetwork.JoinLobby();
        Debug.Log(PhotonNetwork.NickName + " JoinLobby");
        Debug.Log("Connect to Server");
    }

    public void SetRandomSpell()
    {
        bool UseSpell = false;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("UseSpell"))
        {
            UseSpell = (bool)PhotonNetwork.CurrentRoom.CustomProperties["UseSpell"];
        }

        if (PhotonNetwork.IsMasterClient == false)
        {
            StartBtn.SetActive(false);
            if (UseSpell)
            {
                Spell_Panel.SetActive(true);

                References.SetRandomSpell();

                for (int i = 0; i < 3; i++)
                {
                    ListSpell[i].sprite = Resources.Load<Sprite>(References.ListSpell_Own[i].Image);
                }
            }
            else
            {
                Spell_Panel.SetActive(false);
            }
        }
        else
        {
            StartBtn.SetActive(true);
        }

    }

    public void SpellInformation_On(int Index)
    {
        SpellInformation_Name.text = References.ListSpell_Own[Index].Name;
        SpellInformation_Description.text = References.ListSpell_Own[Index].Description;
        SpellInformation_Panel.SetActive(true);
    }

    public void SpellInformation_Off()
    {
        SpellInformation_Panel.SetActive(false);
    }

    public void Information_LoadCurrentQuestion(int Index)
    {
        Information_Question.text = (Index + 1) + ". " + References.ListQuestionCreate[Index].questionText;

        for (int i = 0; i < 4; i++)
        {
            Information_Answer[i].text = References.ListQuestionCreate[Index].answers[i];
        }

        foreach (var item in Information_IsCorrect)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in Information_IsCorrectColor)
        {
            item.color = Color.white;
        }


        Information_IsCorrect[References.ListQuestionCreate[Index].correctAnswerIndex].gameObject.SetActive(true);

        Information_IsCorrectColor[References.ListQuestionCreate[Index].correctAnswerIndex].color = Color.yellow;

        //CurrentPaging.text = (CurrentIndex + 1) + " / " + (References.SelectCollection.ListQuestion.Count).ToString();
    }

    public void Next()
    {
        //if (References.SelectCollection.ListQuestion.Count > 0)
        //{
        //    CurrentIndex++;
        //    if (CurrentIndex >= References.SelectCollection.ListQuestion.Count)
        //    {
        //        CurrentIndex = 0;
        //    }

        //    Information_LoadCurrentQuestion(CurrentIndex);
        //}
    }


    public void Previous()
    {
        //if (References.SelectCollection.ListQuestion.Count > 0)
        //{
        //    CurrentIndex--;
        //    if (CurrentIndex < 0)
        //    {
        //        CurrentIndex = References.SelectCollection.ListQuestion.Count - 1;
        //    }

        //    Information_LoadCurrentQuestion(CurrentIndex);
        //}
    }

    public bool IsCollectionNameValid()
    {
        return !string.IsNullOrEmpty(CreateCollection_NameInput.text);
    }

    public void CreateCollection_Message(bool value)
    {
        CreateCollection_MessagePanel.SetActive(value);
    }


    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCode.StartGameEventCode)
        {
            PhotonNetwork.LoadLevel("MainGame");
        }

        if (photonEvent.Code == EventCode.KickEventCode)
        {
            object[] eventData = (object[])photonEvent.CustomData;
            string kickMessage = (string)eventData[0];
            Debug.Log("Received Kick Message: " + kickMessage);

        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        PhotonNetwork.RaiseEvent(EventCode.StartGameEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }
}







