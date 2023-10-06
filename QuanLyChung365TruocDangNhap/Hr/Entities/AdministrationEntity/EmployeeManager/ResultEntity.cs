using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager
{
    public class ResultEntity
    {
        public Data2 data { get; set; }
        public Error2 error { get; set; }
    }

    public class Data2
    {
        public bool result { get; set; }
        public string message { get; set; }
    }

    public class Error2
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
