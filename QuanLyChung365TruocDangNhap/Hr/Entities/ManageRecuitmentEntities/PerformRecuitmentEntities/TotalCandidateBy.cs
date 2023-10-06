using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.PerformRecuitmentEntities
{
    public class TotalCandidateBy
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success success { get; set; }
        public object error { get; set; }
    }

    public class Success
    {
        public Data2 data { get; set; }
        public string message { get; set; }
    }

    public class Data2
    {
        public DataContent data { get; set; }
    }

    public class DataContent
    {
        public int candidateToday { get; set; }
        public int candidateWeek { get; set; }
        public int candidateMonth { get; set; }
    }
}
