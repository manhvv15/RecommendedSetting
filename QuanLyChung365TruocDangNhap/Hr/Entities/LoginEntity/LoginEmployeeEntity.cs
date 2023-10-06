using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity
{
    public class LoginEmployeeEntity
    {
        public Data4 data { get; set; }

        public ErrorLogin error { get; set; }
    }

    public class ErrorLogin
    {
        public string message { get; set; }
    }

        public class Data2
    {
        
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public UserInfo2 user_info { get; set; }

    }
    public class Data4
    {
        public bool result { get; set; }
        public string message { get; set; }
        public Data2 data { get; set; }

    }

    public class UserInfo2
    {
        public string ep_id { get; set; }

        public string ep_email { get; set; }

        public string ep_name { get; set; }

        public string ep_phone { get; set; }

        public string com_id { get; set; }

        public string dep_id { get; set; }

        public string ep_pass { get; set; }

        public string ep_pass_encrypt { get; set; }
        public string ep_address { get; set; }
        public string ep_birth_day { get; set; }

        public string ep_gender { get; set; }
        public string ep_married { get; set; }
        public string ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_authentic { get; set; }

        public string role_id { get; set; }
        public string ep_image { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string position_id { get; set; }
        public string group_id { get; set; }
        public string ep_description { get; set; }
        public string ep_featured_recognition { get; set; }

        public string ep_status { get; set; }
        public string ep_signature { get; set; }
        public string allow_update_face { get; set; }

        public string version_in_use { get; set; }
        public string com_name { get; set; }
        public string dep_name { get; set; }
        public string ToMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
    }
}
