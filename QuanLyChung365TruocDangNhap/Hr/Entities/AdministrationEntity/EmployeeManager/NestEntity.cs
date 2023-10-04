using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager
{
    public class APINestEntity
    {
        public listNest data { get; set; }
        public object error { get; set; }
    }
    public class listNest
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<NestEntity> items { get; set; }
        public List<Team> data { get; set; }
    }
    public class NestEntity
    {
        public string gr_id { get; set; }
        public string dep_id { get; set; }
        public string gr_name { get; set; }
        public string parent_gr { get; set; }
        public string type { get; set; }
        public string typeGroup { get; set; }
        public string typeListNest { get; set; }

        public string total_empNes { get; set; }
        public string total_empNes_noTimeKeep { get; set; }
        public int total_empNes_TimeKeep { get; set; }
        public List<NestEntity> ListGroupEntity { get; set; }

    }
    public class Team
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
        public int dep_id { get; set; }
        public string dep_name { get; set; }
        public int total_emp { get; set; }
        public string manager { get; set; }
        public string deputy { get; set; }
    }

}
