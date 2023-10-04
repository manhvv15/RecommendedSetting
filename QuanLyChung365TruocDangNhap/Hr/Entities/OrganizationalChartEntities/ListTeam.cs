using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class DataListTeam
    {
        public bool result { get; set; }
        public string message { get; set; }
        public List<Team> data { get; set; }
        
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

    public class ListTeam
    {
        public DataListTeam data { get; set; }
        public object error { get; set; }
    }
}
