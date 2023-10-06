using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Forgot_Password
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string vf_id { get; set; }
        public string id { get; set; }
    }
    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
    }
    public class API_Porgot_Password
    {
        public Data data { get; set; }
        public Error error { get; set; }
    }
}
