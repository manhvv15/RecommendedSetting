using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ListGroup
    {
        
        public int gr_id { get; set; }
        public string gr_name { get; set; }
        public int team_id { get; set; }
        public string team_name { get; set; }
        public int dep_id { get; set; }
        public string dep_name { get; set; }
        public int total_emp { get; set; }
        public string manager { get; set; }
        public string deputy { get; set; }
    }

    public class DataListGroup
    {
        public bool result { get; set; }
        public string message { get; set; }
        public int totalItems { get; set; }
        public List<ListGroup> data { get; set; }
    }

    public class GroupEntity
    {
        public DataListGroup data { get; set; }
        public object error { get; set; }
    }


}
