using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations
{
    public class ListEmployeePolicy
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success1 success { get; set; }
        public string error { get; set; }
    }
    public class Success1
    {
        public Data1 data { get; set; }
        public string message { get; set; }
    }
    public class Data1
    {
        public List<DataEntity1> data { get; set; }
        public int total { get; set; }
    }
    public class DataEntity1
    {
        public string id { get; set; }
        public string name { get; set; }
        //public string time_start { get; set; }
        public string supervisor_name { get; set; }
        public string description { get; set; }
        public string is_delete { get; set; }
        public string created_at { get; set; }
        public string file { get; set; }
        public bool show { get; set; }
        public List<DataEntity3> data_detail { get; set; }
    }
}
