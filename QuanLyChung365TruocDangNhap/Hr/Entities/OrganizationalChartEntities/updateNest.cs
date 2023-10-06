using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class updateNest
    {
        public bool result { get; set; }
        public int code { get; set; }
        public SuccessUpdateNest success { get; set; }
        public object error { get; set; }
    }

    public class SuccessUpdateNest
    {
        public DataUpdateNest data { get; set; }
        public string message { get; set; }
    }
    public class DataUpdateNest
    {
        public string nest_id { get; set; }
    }

}
