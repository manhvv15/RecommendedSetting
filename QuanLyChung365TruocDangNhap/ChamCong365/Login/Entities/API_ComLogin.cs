using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Login.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class UserInfoComLogin
    {
        public string com_id { get; set; }
        public string com_parent_id { get; set; }
        public string com_name { get; set; }
        public string com_email { get; set; }
        public string type_timekeeping { get; set; }
        public string id_way_timekeeping { get; set; }
        public string com_pass { get; set; }
        public string com_pass_encrypt { get; set; }
        public string com_phone { get; set; }
        public string com_logo { get; set; }
        public string com_address { get; set; }
        public string com_role_id { get; set; }
        public object com_size { get; set; }
        public object com_description { get; set; }
        public string com_create_time { get; set; }
        public object com_update_time { get; set; }
        public string com_authentic { get; set; }
        public object com_lat { get; set; }
        public object com_lng { get; set; }
        public string com_path { get; set; }
        public string base36_path { get; set; }
        public object com_qr_logo { get; set; }
        public string enable_scan_qr { get; set; }
        public string from_source { get; set; }
        public string com_vip { get; set; }
        public string com_ep_vip { get; set; }
    }

    public class DataComLogin
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public UserInfoComLogin user_info { get; set; }
    }
    public class ErrorComLogin
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class API_ComLogin
    {
        public DataComLogin data { get; set; }
        public ErrorComLogin error { get; set; }
    }
}
