using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data_List_PhongBan
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_List_PhongBan> items { get; set; }
        public List<Item_List_PhongBan> data { get; set; }
    }

    public class Item_List_PhongBan
    {
        public string dep_id { get; set; }
        public string com_id { get; set; }
        public string dep_name { get; set; }
        public string dep_create_time { get; set; }
        /*public List<Manager_List_PhongBan> manager { get; set; }
        public List<object> deputy { get; set; }*/
        public string total_emp { get; set; }
    }

    public class Manager_List_PhongBan
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }

    public class API_List_PhongBan
    {
        public Data_List_PhongBan data { get; set; }
        public object error { get; set; }
    }


    
}
