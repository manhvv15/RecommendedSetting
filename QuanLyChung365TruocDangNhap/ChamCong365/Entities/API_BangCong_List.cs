using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BangCongData
    {
        public int itemsPerPage { get; set; }
        public int totalItems { get; set; }
        public List<BangCong> items { get; set; }
    }

    public class BangCong
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
        public string IDnTen
        {
            get
            {
                return string.Format("({0}) {1}", ep_id, ep_name);
            }
        }
        public string ep_gender { get; set; }
        public string ep_image { get; set; }
        public BitmapImage image
        {
            get
            {
                BitmapImage img = null;
                //if (!string.IsNullOrEmpty(ep_image)) img = new Uri("https://chamcong.24hpay.vn/upload/employee/" + ep_image).GetThumbnail(200);
                if (img == null) img = new Uri("https://chamcong.timviec365.vn/images/ep_logo.png").GetThumbnail(300);
                return img;
            }
        }
        public string _ts_date;
        public DateTime date
        {
            get
            {
                DateTime d;
                if (!string.IsNullOrEmpty(ts_date) && DateTime.TryParse(ts_date, out d)) return d;
                return new DateTime(9999,12,31);
            }
        }
        public string ts_date
        {
            get
            {
                var date = _ts_date;

                DateTime d;
                if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out d)) date = d.ToString("dd/MM/yyyy");
                return date;
            }
            set
            {
                _ts_date = value;
            }
        }
        public string day_of_week
        {
            get
            {
                var date = _ts_date;

                DateTime d;
                if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out d)) date = d.ToString("ddd");
                return date;
            }
        }

        private List<string> _lst_time;
        public List<string> lst_time
        {
            get
            {
                return _lst_time;
            }
            set
            {
                var list = value;
                for (int i = 0; i < list.Count; i++)
                {
                    DateTime d;
                    if (!string.IsNullOrEmpty(list[i]) && DateTime.TryParse(list[i], out d))
                    {
                        list[i] = d.ToString("hh:mm tt");
                    }
                }
                _lst_time = list;
            }
        }
    }

    public class API_BangCong_List
    {
        public BangCongData data { get; set; }
        public Error error { get; set; }
    }
}
