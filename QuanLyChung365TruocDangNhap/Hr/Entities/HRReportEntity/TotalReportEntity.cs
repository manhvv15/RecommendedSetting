using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.HRReportEntity
{
    public class TotalReportEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success success { get; set; }
    }

    public class Success
    {
        public Data data { get; set; }
        public string message { get; set; }
    }

    public class Data
    {
        public int total_new { get; set; }
        public int total_candidate { get; set; }
        public int total_candidate_number { get; set; }
        public int total_interview { get; set; }
        public int total_interview_pass { get; set; }
        public int total_cancel { get; set; }
        
    }
}
