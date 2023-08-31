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
        PlayerNameTxt.text = player.ActorNumber.ToString();

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

    public void KickPlayer()
    {
        if (PhotonNetwork.CurrentRoom.Players.ContainsKey(Convert.ToInt32(player.ActorNumber.ToString())))
        {
            PhotonNetwork.CloseConnection(player);
            object[] content = new object[] { "You have been kicked from the game." };
            PhotonNetwork.RaiseEvent(EventCode.KickEventCode, content, new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
        }
        else
        {
            Debug.LogWarning(player.UserId + "Player is not in the room.");
        }
    }
}
