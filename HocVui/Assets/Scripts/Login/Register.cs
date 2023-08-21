using Assets.Scripts.Common;
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
    public class Register : MonoBehaviour
    {
        public TMP_InputField username, pass, confirm;
        public UI_Message PopupMessage;

        public void Validation()
        {
            var usernametxt = username.text;
            var passtxt = pass.text;
            var confirmtxt = confirm.text;

            if(usernametxt.IsNullOrEmpty())
            {
                PopupMessage.ShowMessage(Message.UsernameNull);
            }else if(passtxt.IsNullOrEmpty())
            {
                PopupMessage.ShowMessage(Message.PassNull);
            }else if(confirmtxt.CompareTo(passtxt) != 0) {
                PopupMessage.ShowMessage(Message.Confirm);
            }else
            {
                //Account_DAO.CreateAccount;
            }
        }
    }
}
