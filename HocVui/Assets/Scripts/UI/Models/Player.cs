using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    private string username;
    private string avatarURL;

    PlayerGame(string username, string avatarURL)
    {
        this.username = username;
        this.avatarURL = avatarURL; 
    }

    public PlayerGame GetPlayer()
    {
        return new PlayerGame(username, avatarURL);
    }
}
