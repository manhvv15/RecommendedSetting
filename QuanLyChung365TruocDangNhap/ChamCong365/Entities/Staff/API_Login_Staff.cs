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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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
                API_Login_Staff api = JsonConvert.DeserializeObject<API_Login_Staff>(respon.Content.ReadAsStringAsync().Result);
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
        public UserInfo user_info { get; set; }
        public string type { get; set; }
    }
    public class Error
    {
        public int Code { get; set; }
        public string message { get; set; }
    }
    public class Data1
    {
        public bool result { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class API_Login_Staff
    {
        public Data1 data { get; set; }
        public Error error { get; set; }
    }

    public class UserInfo
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
        public BitmapImage image
        {
            get
            {
                BitmapImage img = null;
                if (!string.IsNullOrEmpty(ep_image)) img = new Uri("https://chamcong.24hpay.vn/upload/employee/" + ep_image).GetThumbnail(300);
                if (img == null) img = new Uri("https://chamcong.timviec365.vn/images/ep_logo.png").GetThumbnail(300);
                return img;
            }
        }
        public string create_time { get; set; }
        public object update_time { get; set; }
        public string start_working_time { get; set; }
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
}
