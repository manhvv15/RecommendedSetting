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
    public class ToaDoData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ToaDo> items { get; set; }
    }
    public class ToaDo
    {
        public string cor_id { get; set; }
        public string com_id { get; set; }
        public string cor_location_name { get; set; }
        public string cor_lat { get; set; }
        public string cor_long { get; set; }
        public string cor_radius { get; set; }
        public string cor_create_time { get; set; }
        public string display_cor_create_time
        {
            get
            {
                try
                {
                    DateTime date = new DateTime(1970,1,1,0,0,0);
                    date = date.AddSeconds(long.Parse(cor_create_time));
                    return date.ToString("dd/MM/yyyy");
                }
                catch 
                {
                    return cor_create_time;
                }
            }
        }
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
    public class API_ToaDo_List
    {
        public ToaDoData data { get; set; }
        public Error error { get; set; }
    }
}
