using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class listDepartment
    {
        public Data6 data { get; set; }
        public object error { get; set; }
    }
    public class Data6
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Items6> items { get; set; }
    }
   
    public class Items6
    {
        public string dep_id { get; set; }
        public string com_id { get; set; }
        public string dep_name { get; set; }
        public string dep_create_time { get; set; }
        //public List<Manager> manager { get; set; }
        //public List<Deputy> deputy { get; set; }
        public List<NestEntity> ListNestEntity { get; set; }
        public string total_emp { get; set; }
        public string total_emp_noTimeKeep { get; set; }
        public int total_emp_TimeKeep { get; set; }
        public string type { get; set; }
        public string typeTail { get; set; }
        public string typeBottom { get; set; }


    }

    public class Manager
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }
    public class Deputy
    {
        public string ep_id { get; set; }
        public string ep_name { get; set; }
    }
}
