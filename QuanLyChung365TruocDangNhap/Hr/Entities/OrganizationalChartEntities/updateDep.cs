using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class updateDep
    {
        public bool result { get; set; }
        public int code { get; set; }
        public SuccessUpdateDep success { get; set; }
        public object error { get; set; }
    }

    public class SuccessUpdateDep
    {
        public DataUpdateDep data { get; set; }
        public string message { get; set; }
    }
    public class DataUpdateDep
    {
        public string dep_id { get; set; }
    }

}
