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

        NumberPlayerTxt.text = Roominformation.PlayerCount + " / " + Roominformation.MaxPlayers;
    }

    public void Onclick()
    {
        Lobby_Manager.Instance.JoinRoom(Roominformation);
    }
}
