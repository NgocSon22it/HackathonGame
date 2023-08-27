using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Account_Entity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int RoleID { get; set; }
        public string HairID { get; set; }
        public string EyeID { get; set; }
        public string SkinID { get; set; }
        public string MouthID { get; set; }
        public bool IsFirst { get; set; }

    }
}
