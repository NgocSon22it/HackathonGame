using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public static class Message
    {
        public static string UsernameNull = "Tên đăng nhập không được để trống";
        public static string UsernameExist = "Tên đăng nhập đã tồn tại";
        public static string UsernameLength = "Tên đăng nhập có ít nhất 8 kí tự, không chứa khoảng trắng";
        public static string PassLength = "Tên đăng nhập có ít nhất 8 kí tự, không chứa khoảng trắng";
        public static string PassNull = "Mật khẩu không được để trống";
        public static string Confirm = "Mật khẩu không khớp";
        public static string RegisterSuccess = "Đăng kí thành công";
        public static string LoginFail = "Tên đăng nhập hoặc mật khẩu không đúng";

        public static string NameExist = "Tên nhân vật đã tồn tại";

    }
}
