using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class listCompany
    {
        public Data7 data { get; set; }
    }
    public class Data7
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Items7> items { get; set; }
    }
    public class Items7
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
        public string com_update_time { get; set; }
        public string com_authentic { get; set; }
        public string com_lat { get; set; }
        public string com_lng { get; set; }
        public string com_path { get; set; }
        public string base36_path { get; set; }
        public string com_qr_logo { get; set; }
        public string total_emp { get; set; }
        public string type { get; set; }
        //public Manager manager { get; set; }
        //public Deputy deputy { get; set; }
    }
}
