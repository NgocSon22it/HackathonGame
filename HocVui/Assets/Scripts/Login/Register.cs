using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Assets.Scripts.Login
{
    public class Register : MonoBehaviour
    {
        public TMP_InputField username, pass, confirm;
        public UI_Login PopupMessage;
        public GameObject LoginPanel, RegisterPanel;

        public void RegisterAccount()
        {
            var usernametxt = username.text;
            var passtxt = pass.text;
            var confirmtxt = confirm.text;

            if (usernametxt.IsNullOrEmpty())
            {
                PopupMessage.ShowMessage(Message.UsernameNull);
            }
            else if (usernametxt.Length < 4 || usernametxt.Contains(" "))
            {
                PopupMessage.ShowMessage(Message.UsernameLength);
            }
            else if (passtxt.IsNullOrEmpty())
            {
                PopupMessage.ShowMessage(Message.PassNull);
            }
            else if (passtxt.Length < 8 || passtxt.Contains(" "))
            {
                PopupMessage.ShowMessage(Message.PassLength);
            }
            else if (confirmtxt.CompareTo(passtxt) != 0)
            {
                PopupMessage.ShowMessage(Message.Confirm);
            }
            else
            {
                if (Account_DAO.CheckUsername(usernametxt))
                {
                    var roleID = Role.Student;

                    Account_DAO.CreateAccount(usernametxt, passtxt, ((int)roleID));

                    username.text = string.Empty;
                    pass.text = string.Empty;
                    confirm.text = string.Empty;

                    PopupMessage.ShowMessage(Message.RegisterSuccess);
                    LoginPanel.SetActive(true); 
                    RegisterPanel.SetActive(false);
                }
                else
                {
                    PopupMessage.ShowMessage(Message.UsernameExist);
                }

            }
        }
    }
}
