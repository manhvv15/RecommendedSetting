using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }

    public class API_Respon
    {
        public Data data { get; set; }
        public Error error { get; set; }
    }

    public class API_Response
    {
        public string company_id { get; set; }
        public bool message { get; set; }
        public string user_id { get; set; }
    }

    public class DataImage
    {
        public DataImage(string image)
        {
            this.image = image;
        }

        public string image { get; set; }
    }
}
