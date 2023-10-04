using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits
{
    public class ListAchievementEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success success { get; set; }
        public object error { get; set; }
    }
    public class Success
    {
        public Data data { get; set; }
    }
    public class Data
    {
        public int total { get; set; }
        public List<DataEntity2> data { get; set; }
    }
    public class DataEntity2
    {
        public string id { get; set; }
        public string achievement_id { get; set; }

        public string name_binding { get; set; }
        public string list_user { get; set; }
        public string list_user_name { get; set; }
        public string content { get; set; }
        public string created_by { get; set; }
        public string achievement_at { get; set; }
        public string achievement_type { get; set; }
        public string achievement_name { get; set; }
        public string appellation { get; set; }
        public string achievement_level { get; set; }
        public string type { get; set; }
        public string com_id { get; set; }
        public string created_at { get; set; }
        public string dep_name { get; set; }
        public string dep_id { get; set; }
    }
}
