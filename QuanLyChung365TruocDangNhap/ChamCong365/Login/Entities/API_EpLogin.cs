using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Login.Entities
{
    public class UserInfoEpLogin
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
        public object update_time { get; set; }
        public object start_working_time { get; set; }
        public string position_id { get; set; }
        public object group_id { get; set; }
        public object ep_description { get; set; }
        public string ep_featured_recognition { get; set; }
        public string ep_status { get; set; }
        public string ep_signature { get; set; }
        public string allow_update_face { get; set; }
        public string version_in_use { get; set; }
        public string from_source { get; set; }
        public string com_name { get; set; }
        public string dep_name { get; set; }
    }

    public class DataEpLogin
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public UserInfoEpLogin user_info { get; set; }
    }

    public class ErrorEpLogin
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class API_EpLogin
    {
        public DataEpLogin data { get; set; }
        public ErrorEpLogin error { get; set; }
    }
}
