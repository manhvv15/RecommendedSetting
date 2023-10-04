using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity
{
    public class AccountEmployeeEntity
    {
        public bool result { get; set; }

        public int code { get; set; }

        public Success success { get; set; }

        public object error { get; set; }
    }

    public class Success
    {
        public Data data { get; set; }

        public string message { get; set; }
    }

    public class Data
    {
        public int total { get; set; }
    }
}
