using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Scripts.Login
{
    public class UI_Login : Panel_setting
    {
        public TMP_Text MessageError;

        public void ShowMessage(string message)
        {
            MessageError.text = message;
            fadeIn();
        }

        public void CloseMessage()
        {
            MessageError.text = string.Empty;
            fadeOut();
        }
    }
}
