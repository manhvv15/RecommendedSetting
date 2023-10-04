using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Error
    {
        public int Code { get; set; }
        public string message { get; set; }
    }
    public class Data
    {
        public bool _isEmptyOrInvalid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            var jwtToken = new JwtSecurityToken(token);
            return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
        }
        public async void refreshToken()
        {
            try
            {
                HttpClient http = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                HttpResponseMessage respon;
                http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);
                form.Add("refresh_token", refresh_token);
                respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/employee/getNewToken", new FormUrlEncodedContent(form));
                API_Login_Company api = JsonConvert.DeserializeObject<API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    access_token = api.data.data.access_token;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public bool result { get; set; }
        public string message { get; set; }
        private string _access_token;
        public string access_token
        {
            set
            {
                _access_token = value;
            }
            get
            {
                if (_isEmptyOrInvalid(_access_token)) refreshToken();
                return _access_token;
            }
        }
        public string refresh_token { get; set; }
        public string type { get; set; }
        public UserInfo user_info { get; set; }
    }

    public class Data1
    {
        public bool result { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class API_Login_Company
    {
        public Data1 data { get; set; }
        public Error error { get; set; }
    }

    public class UserInfo
    {
        public string com_id { get; set; }
        public object com_parent_id { get; set; }
        public string com_name { get; set; }
        public string com_email { get; set; }
        public string type_timekeeping { get; set; }
        public string id_way_timekeeping { get; set; }
        public string com_pass { get; set; }
        public string com_pass_encrypt { get; set; }
        public string com_phone { get; set; }
        public string com_logo { get; set; }
        public BitmapImage logo
        {
            get
            {
                if (!string.IsNullOrEmpty(com_logo)) return new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + com_logo).GetThumbnail(300);
                else return new Uri("https://chamcong.timviec365.vn/images/logo_com.png").GetThumbnail(300);
            }
        }
        public string com_address { get; set; }
        public string com_role_id { get; set; }
        public string com_size { get; set; }
        public string com_description { get; set; }
        public string com_create_time { get; set; }
        public object com_update_time { get; set; }
        public string com_authentic { get; set; }
        public object com_lat { get; set; }
        public object com_lng { get; set; }
        public string com_path { get; set; }
        public string base36_path { get; set; }
        public string com_qr_logo { get; set; }
        public string enable_scan_qr { get; set; }
        public string from_source { get; set; }
        public string com_vip { get; set; }
        public string com_ep_vip { get; set; }
    }
}
