using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class List_Staff_All
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_List_Staff_All> items { get; set; }
    }

    public class Item_List_Staff_All
    {
        public string _id { get; set; }
        public bool IsSelected { get; set; }
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string IDnTen
        {
            get
            {
                return string.Format("({0}) {1}", ep_id, ep_name);
            }
        }
        public BitmapImage image
        {
            get
            {
                BitmapImage img = null;
                if(!string.IsNullOrEmpty(ep_image))img= new Uri("https://chamcong.24hpay.vn/upload/employee/" + ep_image).GetThumbnail(100);
                if (img == null) img = new Uri("https://chamcong.timviec365.vn/images/ep_logo.png").GetThumbnail(100);
                return img;
            }
        }
        public string ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string ep_gender { get; set; }
        public string ep_married { get; set; }
        public string ep_birth_day { get; set; }
        public object ep_description { get; set; }
        public string create_time { get; set; }
        public string role_id { get; set; }
        public string group_id { get; set; }
        public string start_working_time { get; set; }
        public string display_start_working_time
        {
            get
            {
                try
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
                    date = date.AddSeconds(long.Parse(start_working_time));
                    return date.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return start_working_time;
                }
            }
        }
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string update_time { get; set; }
        public string allow_update_face { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public object gr_name { get; set; }
        public object parent_gr { get; set; }
        public string shift_id { get; set; }
    }

    public class API_List_Staff_All
    {
        public List_Staff_All data { get; set; }
        public object error { get; set; }
    }
}
