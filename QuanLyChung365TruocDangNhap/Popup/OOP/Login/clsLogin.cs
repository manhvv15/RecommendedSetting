using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Popup.OOP.Login
{
    public class clsLogin
    {
        public class ComInfo
        {
            public int com_id { get; set; }
            public string com_email { get; set; }
        }

        public class Data
        {
            public bool result { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
            public int _id { get; set; }
            public ComInfo com_info { get; set; }
            public int authentic { get; set; }
            public UserInfo user_info { get; set; }
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string type { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
            public object error { get; set; }
        }

        public class UserInfo
        {
            public int com_id { get; set; }
            public string com_name { get; set; }
            public string com_email { get; set; }
            public string com_phone_tk { get; set; }
            public string type_timekeeping { get; set; }
            public string id_way_timekeeping { get; set; }
            public object com_logo { get; set; }
            public string com_pass { get; set; }
            public object com_address { get; set; }
            public int com_role_id { get; set; }
            public int com_size { get; set; }
            public string com_description { get; set; }
            public int com_create_time { get; set; }
            public int com_update_time { get; set; }
            public int com_authentic { get; set; }
            public object com_lat { get; set; }
            public object com_lng { get; set; }
            public string com_qr_logo { get; set; }
            public int enable_scan_qr { get; set; }
            public int com_vip { get; set; }
            public int com_ep_vip { get; set; }
            public int ep_crm { get; set; }
            public int scan { get; set; }
            public string com_pass_encrypt { get; set; }
            public string com_path { get; set; }
            public string base36_path { get; set; }
            public string from_source { get; set; }
            public string com_id_tv365 { get; set; }
            public string com_quantity_time { get; set; }
            public string com_kd { get; set; }
            public string check_phone { get; set; }
        }
    }
}
