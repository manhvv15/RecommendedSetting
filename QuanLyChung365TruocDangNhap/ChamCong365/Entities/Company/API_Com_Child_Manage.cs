using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data_Com_Child_Manage
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_Data_Com_Child_Manage> items { get; set; }
    }

    public class Item_Data_Com_Child_Manage
    {
        public string com_id { get; set; }
        public string com_parent_id { get; set; }
        public string com_name { get; set; }
        public string com_email { get; set; }
        public string id_way_timekeeping { get; set; }
        public string com_pass { get; set; }
        public string com_pass_encrypt { get; set; }
        public string com_phone { get; set; }
        public string com_logo { get; set; }

        public BitmapImage image
        {
            get
            {
                if (!string.IsNullOrEmpty(com_logo)) return new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + com_logo).GetThumbnail(200);
                else return new Uri("https://chamcong.timviec365.vn/images/logo_com.png").GetThumbnail(200);
            }
        }
        public string com_address { get; set; }
        public string com_role_id { get; set; }
        public string com_size { get; set; }
        public string com_description { get; set; }
        public string com_create_time { get; set; }
        public object com_update_time { get; set; }
        public string com_authentic { get; set; }
        public object com_lat { get; set; }
        public object com_lng { get; set; }
        public string com_path { get; set; }
        public string base36_path { get; set; }
        public string com_qr_logo { get; set; }
        public string total_emp { get; set; }
        public List<Manager> manager { get; set; }
        public List<object> deputy { get; set; }
        public string type_com
        {
            get
            {
                string x = "Công ty mẹ";
                if (!string.IsNullOrEmpty(com_parent_id))
                {
                    x = "Công ty con";
                }
                return x;
            }
        }
    }

    public class Manager
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }

    public class API_Com_Child_Manage
    {
        public Data_Com_Child_Manage data { get; set; }
        public object error { get; set; }
    }
}
