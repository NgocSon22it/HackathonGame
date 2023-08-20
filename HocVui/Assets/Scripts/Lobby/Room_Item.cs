using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room_Item : MonoBehaviour
{
    [SerializeField] TMP_Text Room_IDTxt;
    [SerializeField] TMP_Text Room_NameTxt;
    [SerializeField] TMP_Text Room_MapTxt;
    [SerializeField] TMP_Text NumberPlayerTxt;

    RoomInfo Roominformation;

    public void SetUp(RoomInfo _Roominformation)
    {
        Roominformation = _Roominformation;
        Room_IDTxt.text = Roominformation.CustomProperties["RoomID"].ToString();
        Room_NameTxt.text = Roominformation.Name;
        Room_MapTxt.text = Roominformation.CustomProperties["Map"].ToString();
        NumberPlayerTxt.text = (Roominformation.PlayerCount - 1) + " / " + (Roominformation.MaxPlayers - 1);
    }

    public void Onclick()
    {
        Lobby_Manager.Instance.SelectRoom(Roominformation);
    }
}
