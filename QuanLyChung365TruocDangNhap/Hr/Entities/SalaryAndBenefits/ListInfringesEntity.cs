using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits
{
    public class ListInfringesEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success1 success { get; set; }
        public object error { get; set; }
    }
    public class Success1
    {
        public Data1 data { get; set; }
    }
    public class Data1
    {
        public int total { get; set; }
        public List<DataEntity1> data { get; set; }
    }
    public class DataEntity1
    {
        public string id { get; set; }
        public string name_binding { get; set; }
        public string infringe_name { get; set; }
        public string regulatory_basis { get; set; }
        public string number_violation { get; set; }
        public string list_user { get; set; }
        public string list_user_name { get; set; }
        public string created_by { get; set; }
        public string infringe_at { get; set; }
        public string infringe_type { get; set; }
        public string type { get; set; }
        public string com_id { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
