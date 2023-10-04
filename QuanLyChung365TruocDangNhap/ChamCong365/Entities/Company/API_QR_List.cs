using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ConfigTimekeeping
    {
        public string enable_scan_qr { get; set; }
        public List<ListCor> list_cor { get; set; }
        public List<ListShift> list_shift { get; set; }
    }

    public class QRData
    {
        public bool result { get; set; }
        public string message { get; set; }
        public ConfigTimekeeping config_timekeeping { get; set; }
    }

    public class ListCor
    {
        public string cor_id { get; set; }
        public string com_id { get; set; }
        public string cor_location_name { get; set; }
        public string cor_lat { get; set; }
        public string cor_long { get; set; }
        public string cor_radius { get; set; }
        public string cor_create_time { get; set; }
        public string is_default { get; set; }
        public string status { get; set; }
        public string qr_logo { get; set; }
        public string qr_status { get; set; }
        public BitmapImage qr
        {
            get
            {
                return new Uri("https://chamcong.24hpay.vn/images/qr_timekeeping/" + qr_logo).GetThumbnail(100);
            }
        }
    }

    public class ListShift
    {
        public string shift_id { get; set; }
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string over_night { get; set; }
        public string status { get; set; }
    }

    public class API_QR_List
    {
        public QRData data { get; set; }
        public Error error { get; set; }
    }
}
