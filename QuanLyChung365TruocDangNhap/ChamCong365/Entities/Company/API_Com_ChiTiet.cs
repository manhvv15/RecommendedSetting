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
    public class ComChiTietData
    {
        public bool result { get; set; }
        public string message { get; set; }
        public DetailCompany data { get; set; }
        public List<ListDepartment> list_department { get; set; }
    }

    public class DetailCompany
    {
        public string com_id { get; set; }
        public object com_parent_id { get; set; }
        public string com_name { get; set; }
        public string com_email { get; set; }
        public string type_timekeeping { get; set; }
        public string id_way_timekeeping { get; set; }
        public string com_pass { get; set; }
        public string com_pass_encrypt { get; set; }
        public string com_phone { get; set; }
        public string com_logo { get; set; }
        public BitmapImage logo
        {
            get
            {
                if (!string.IsNullOrEmpty(com_logo)) return new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + com_logo).GetThumbnail(300);
                else return null;
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
        public string enable_scan_qr { get; set; }
        public string from_source { get; set; }
        public string com_vip { get; set; }
        public string com_ep_vip { get; set; }
    }

    public class ListDepartment
    {
        public string dep_id { get; set; }
        public string dep_name { get; set; }
    }

    public class API_Com_ChiTiet
    {
        public ComChiTietData data { get; set; }
        public Error error { get; set; }
    }
}
