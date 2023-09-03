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
        public static string UsernameLength = "Tên đăng nhập có ít nhất 4 kí tự, không chứa khoảng trắng";
        public static string PassLength = "Tên đăng nhập có ít nhất 8 kí tự, không chứa khoảng trắng";
        public static string PassNull = "Mật khẩu không được để trống";
        public static string Confirm = "Mật khẩu không khớp";
        public static string RegisterSuccess = "Đăng kí thành công";
        public static string LoginFail = "Tên đăng nhập hoặc mật khẩu không đúng";

        public static string NameExist = "Tên nhân vật đã tồn tại";
        public static string NameNull = "Tên nhân vật không được để trống";

        public static string PublicMessage = "{0}:{1}";




        public static string Game_PickUpPile_Already = "Bạn đã nhặt cọc rồi";
        public static string Game_PuttingPile_Already = "Bạn đã cắm cọc rồi";
        public static string Game_PuttingPile_DoNotHavePile = "Bạn cần nhặt cọc để cắm!";
        public static string Game_SelectedAnswer = "Bạn chọn đáp án ";

        public static string Buff_X2Score = "Nhận gấp đôi số điểm cho câu hỏi này";
        public static string Buff_TimeFreeze = "Điểm số của bạn sẽ không bị ảnh hưởng bởi thời gian trong câu hỏi này";

        public static string Game_SpellUseAlready = "Phép bổ trợ {0} đã sử dụng rồi";

        public static string Game_WrongTarget = "Phép bổ trợ {0} chỉ sử dụng lên {1}";
        public static string Game_PileBaseLock = "Bãi cắm cọc đang bị khóa!";

        public static string Lobby_RoomName = "Hãy nhập tên phòng!";
        public static string Lobby_ChooseRoom = "Hãy chọn phòng!";
        public static string Lobby_WrongPassword = "Mật khẩu không đúng!";
        public static string Lobby_RoomPassword = "Hãy nhập mật khẩu!";
        public static string Lobby_RoomFull = "Phòng đã đầy!";
        public static string Lobby_RoomError = "Gặp vấn đề khi tạo phòng, hãy thử lại!";
        public static string Lobby_HostLeave = "Phòng sẽ giải tán nếu bạn thoát!";
        public static string Lobby_Leave = "Bạn có xác nhận rời phòng!";
        public static string Lobby_RoomStop = "Phòng đã giải tán!";









    }
}
