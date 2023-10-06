using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.HRReportEntity
{
    public class ListRecruitmentStaffEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success2 success { get; set; }
    }
    public class Success2
    {
        public Data2 data { get; set; }
        public string message { get; set; }
    }

    public class Data2
    {
        public int total { get; set; }
        public List<DataEntity> data { get; set; }
    }

    public class DataEntity
    {
        public int STT { get; set; }
        public string Hr_name { get; set; } // name hr
        public string hr_name { get; set; } // id hr
        public string sotintheodoi { get; set; }
    }
}
