﻿using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Scripts.Login
{
    public class Login : MonoBehaviour
    {
        public TMP_InputField username, pass;
        public UI_Login PopupMessage;

        public void LoginAccount()
        {
            var usernametxt = username.text;
            var passtxt = pass.text;

            if (usernametxt.IsNullOrEmpty())
            {
                PopupMessage.ShowMessage(Message.UsernameNull);
            }
            else if (usernametxt.Length <= 4 || usernametxt.Contains(" "))
            {
                PopupMessage.ShowMessage(Message.UsernameLength);
            }
            else if (passtxt.IsNullOrEmpty())
            {
                PopupMessage.ShowMessage(Message.PassNull);
            }
            else if (passtxt.Length <= 4 || passtxt.Contains(" "))
            {
                PopupMessage.ShowMessage(Message.PassLength);
            }
            else
            {
                if (Account_DAO.Login(usernametxt, passtxt) != null)
                {
                    Debug.Log("Login success");
                }
                else
                {
                    PopupMessage.ShowMessage(Message.LoginFail);
                }

            }
        }
    }
}