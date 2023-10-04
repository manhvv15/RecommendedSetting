using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class XuatCongData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<XuatCong> items { get; set; }
    }

    public class XuatCong
    {
        public string sheet_id { get; set; }
        public string ep_id { get; set; }
        public string ts_image { get; set; }
        public string at_time { get; set; }
        public string device { get; set; }
        public string ts_lat { get; set; }
        public string ts_long { get; set; }
        public string ts_location_name { get; set; }
        public string wifi_name { get; set; }
        public string wifi_ip { get; set; }
        public string wifi_mac { get; set; }
        public string shift_id { get; set; }
        public string note { get; set; }
        public string bluetooth_address { get; set; }
        public string status { get; set; }
        public string ts_error { get; set; }
        public string is_success { get; set; }
        public string dep_name { get; set; }
        public string ep_name { get; set; }
        public string ep_gender { get; set; }
    }

    public class API_XuatCong_List
    {
        public XuatCongData data { get; set; }
        public Error error { get; set; }
    }
}
