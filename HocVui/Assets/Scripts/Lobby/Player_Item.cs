using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Item : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] TMP_Text PlayerNameTxt;

    [SerializeField] GameObject KickBtn;

    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        PlayerNameTxt.text = player.NickName.ToString();

        if(PhotonNetwork.IsMasterClient)
        {
            KickBtn.SetActive(true);
        }
        else
        {
            KickBtn.SetActive(false);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
           Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public void KickPlayer_Confirm()
    {
        Lobby_Manager.Instance.SelectedPlayer = player;
        Lobby_Manager.Instance.confirmType = Confirm_Type.Kick;
        Lobby_Manager.Instance.ConfirmPanel_On($"Bạn có muốn mời người chơi {Lobby_Manager.Instance.SelectedPlayer.NickName} ra khỏi phòng!");
    }
}
