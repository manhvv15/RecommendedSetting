using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations
{
    class DetailProvisionEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success4 success { get; set; }
        public object error { get; set; }
    }
    public class Success4
    {
        public Data4 data { get; set; }
        public string message { get; set; }
    }
    public class Data4
    {
        public DataEntity data { get; set; }
    }
}
