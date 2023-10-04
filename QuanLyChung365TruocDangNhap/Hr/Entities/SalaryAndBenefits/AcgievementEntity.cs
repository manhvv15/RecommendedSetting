using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits
{
    public class AcgievementEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success3 success { get; set; }
        public object error { get; set; }
    }
    public class Success3
    {
        public Data3 data { get; set; }
        public string message { get; set; }
    }
    public class Data3
    {
        public string data { get; set; }
    }
}
