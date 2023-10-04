using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity
{
    public class LoginCompanyEntity
    {
        public Data1 data { get; set; }

        public Error error { get; set; }
    }
    public class Data
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public UserInfo user_info { get; set; }
    }
    public class Data1
    {
        public bool result { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        
    }
    //public class UserInfo
    //{
    //    public string com_id { get; set; }

    //    public string com_parent_id { get; set; }

    //    public string com_name { get; set; }

    //    public string com_email { get; set; }

    //    public string type_timekeeping { get; set; }

    //    public string id_way_timekeeping { get; set; }

    //    public string com_pass { get; set; }

    //    public string com_pass_encrypt { get; set; }

    //    public string com_phone { get; set; }
    //    public string com_logo { get; set; }
    //    public string com_address { get; set; }

    //    public string com_role_id { get; set; }
    //    public string com_size { get; set; }
    //    public string com_description { get; set; }
    //    public string com_create_time { get; set; }
    //    public string com_update_time { get; set; }

    //    public string com_authentic { get; set; }
    //    public string com_lat { get; set; }
    //    public string com_lngcom_path { get; set; }
    //    public string base36_path { get; set; }
    //    public string com_qr_logo { get; set; }
    //    public string enable_scan_qr { get; set; }
    //}
    public class Error
    {
        public int code { get; set; }

        public string message { get; set; }
    }
}
