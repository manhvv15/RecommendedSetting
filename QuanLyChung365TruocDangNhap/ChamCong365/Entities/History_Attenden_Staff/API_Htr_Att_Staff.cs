using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff
{
    public class Data
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Htr_Att_Staff> items { get; set; }
    }

    public class Htr_Att_Staff
    {
        public string IDnTen
        {
            get
            {
                return string.Format("({0}) {1}", ep_id, ep_name);
            }
        }
        public string sheet_id { get; set; }
        public string ep_id { get; set; }
        public string ts_image { get; set; }
        public BitmapImage image
        {
            get
            {
                BitmapImage img = null;
                if (!string.IsNullOrEmpty(ts_image))
                {
                    img = new Uri("https://chamcong.24hpay.vn/image/time_keeping/" + ts_image).GetThumbnail(100);
                }
                if(img==null) img=new Uri("https://chamcong.timviec365.vn/images/ep_logo.png").GetThumbnail(100);
                return img;
            }
        }
        public string at_time { get; set; }
        public string  time
        {
            get
            {
                var str = "";
                DateTime dt;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time,out dt))
                {
                    str = dt.ToString("hh:mm tt dd/MM/yyyy", new CultureInfo("en-US"));
                    
                    
                }
                return str;
            }
        }
        public string time_shift
        {
            get
            {
                var str = "";
                DateTime dt;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time, out dt))
                {
                    str = dt.ToString("hh:mm tt", new CultureInfo("en-US"));
                }
                return str;
            }
        }
        public string date_shift
        {
            get
            {
                var str = "";
                DateTime dt;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time, out dt))
                {
                    str = dt.ToString("dd/MM/yyyy", new CultureInfo("en-US"));


                }
                return str;
            }
        }
        public DateTime date
        {
            get
            {
                DateTime dt;
                if (!string.IsNullOrEmpty(at_time) && DateTime.TryParse(at_time, out dt))
                {
                    return dt;
                }
                return new DateTime(9999,12,30);
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

    public class API_Htr_Att_Staff
    {
        public Data data { get; set; }
        public object error { get; set; }
    }
}
