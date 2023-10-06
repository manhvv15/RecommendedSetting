using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DiemDanhData
    {
        public bool result { get; set; }
        public string message { get; set; }
        public bool is_success { get; set; }
        public string at_time { get; set; }
        public string image { get; set; }
        public string note { get; set; }
    }

    public class API_DiemDanh_Respon
    {
        public DiemDanhData data { get; set; }
        public Error error { get; set; }
    }
}
