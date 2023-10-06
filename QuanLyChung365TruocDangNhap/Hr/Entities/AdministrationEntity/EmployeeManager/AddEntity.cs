using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager
{
    public class AddEntity
    {
        public AddDataEntity data { get; set; }
        public string error { get; set; }
    }
    public class AddDataEntity
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }
}
