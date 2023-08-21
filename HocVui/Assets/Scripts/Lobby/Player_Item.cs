using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Item : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] TMP_Text PlayerNameTxt;

    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        PlayerNameTxt.text = player.NickName;
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
}
