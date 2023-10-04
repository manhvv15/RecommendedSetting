using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.HRReportEntity
{
    public class ListReportCandidateRcm
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success3 success { get; set; }
    }

    public class Success3
    {
        public Data3 data { get; set; }
        public string message { get; set; }
    }

    public class Data3
    {
        public int total { get; set; }
        public List<DataEntity3> data { get; set; }
    }

    public class DataEntity3
    {
        public string id_full
        {
            get
            {
                return "NV" + user_recommend;
            }
        }
        public string name { get; set; }
        public string user_recommend { get; set; }
        public string soungvien { get; set; }
    }
}
