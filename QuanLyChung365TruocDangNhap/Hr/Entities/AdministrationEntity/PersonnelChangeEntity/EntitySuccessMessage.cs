using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class EntitySuccessMessage
    {
        public DataMesesage data { get; set; }
        public Error error { get; set; }
    }
    public class DataMesesage
    {
        public bool result { get; set; }
        public string message { get; set; }
        public int id { get; set; }
    }
    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
