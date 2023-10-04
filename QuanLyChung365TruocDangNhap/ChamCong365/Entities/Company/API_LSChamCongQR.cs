using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataLSChamCongQR
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<LSChamCongQR> items { get; set; }
    }

    public class LSChamCongQR
    {
        public string sheet_id { get; set; }
        public string ep_id { get; set; }
        public string ts_image { get; set; }
        public string display_ts_image
        {
            get
            {
                string x = ts_image;
                if (string.IsNullOrEmpty(x))
                {
                    x = "https://chamcong.timviec365.vn/images/ep_logo.png";
                }
                return x;
            }
        }
        public string at_time { get; set; }
        public string display_at_time
        {
            get
            {
                string x = DateTime.Parse(at_time).ToString("hh:mm tt - dd/MM/yyyy");
                return x;
            }
        }
        public string device { get; set; }
        public string ts_lat { get; set; }
        public string ts_long { get; set; }
        public string ts_location_name { get; set; }
        public object wifi_name { get; set; }
        public object wifi_ip { get; set; }
        public object wifi_mac { get; set; }
        public string shift_id { get; set; }
        public string ts_com_id { get; set; }
        public string note { get; set; }
        public string bluetooth_address { get; set; }
        public string status { get; set; }
        public string ts_error { get; set; }
        public string is_success { get; set; }
        public string TrangThai
        {
            get
            {
                string x = "Thành công";
                if(is_success == "0")
                {
                    if (status == "2")
                    {
                        x = "Chờ duyệt";
                    }
                    else
                        x = "Thất bại";
                }
                return x;
            }
        }
        public string ep_name { get; set; }
        public string display_ep_name
        {
            get
            {
                string x = "(" + ep_id + ")" + ep_name;
                return x;
            }
        }
        public string ep_gender { get; set; }
        public string shift_name { get; set; }
    }

    public class API_LSChamCongQR
    {
        public DataLSChamCongQR data { get; set; }
        public object error { get; set; }
    }
}
