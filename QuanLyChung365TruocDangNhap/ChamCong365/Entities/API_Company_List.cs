using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CompanyData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ChildCompany> items { get; set; }
    }

    public class Deputy
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }

    public class ChildCompany
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
        public List<Deputy> deputy { get; set; }
    }

    public class Manager
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }

    public class API_Company_List
    {
        public CompanyData data { get; set; }
        public Error error { get; set; }
    }
}
