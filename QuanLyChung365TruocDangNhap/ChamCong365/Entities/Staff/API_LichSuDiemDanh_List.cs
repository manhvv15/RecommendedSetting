using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LichSuDiemDanhData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<LichSuDiemDanh> items { get; set; }
    }

    public class LichSuDiemDanh
    {
        public string sheet_id { get; set; }
        public string ep_id { get; set; }
        public string ts_image { get; set; }
        public BitmapImage image
        {
            get
            {
                if (!string.IsNullOrEmpty(ts_image)) return new Uri("https://chamcong.24hpay.vn/image/time_keeping/" + ts_image).GetThumbnail(200);
                else return null;
            }
        }
        public string at_time { get; set; }
        public DateTime attime
        {
            get
            {
                DateTime aa;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time, out aa)) return aa;
                return new DateTime(9999,12,30);
            }
        }
        public string time
        {
            get
            {
                var x = "";
                DateTime tt;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time, out tt))
                {
                    x = tt.ToString("H:mm tt");
                }
                return x;
            }
        }
        public string date
        {
            get
            {
                var x = "";
                DateTime tt;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time, out tt))
                {
                    x = tt.ToString("dd-MM-yyyy");
                }
                return x;
            }
        }
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
        public string ep_name { get; set; }
        public string ep_gender { get; set; }
        public string shift_name { get; set; }
    }

    public class API_LichSuDiemDanh_List
    {
        public LichSuDiemDanhData data { get; set; }
        public Error error { get; set; }
    }
}
