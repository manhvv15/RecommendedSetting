using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EPNotInCycleData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<EPNotInCycle> items { get; set; }
    }

    public class EPNotInCycle
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string ep_gender { get; set; }
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
    }

    public class API_EPNotInCycle_List
    {
        public EPNotInCycleData data { get; set; }
        public object error { get; set; }
    }
}
