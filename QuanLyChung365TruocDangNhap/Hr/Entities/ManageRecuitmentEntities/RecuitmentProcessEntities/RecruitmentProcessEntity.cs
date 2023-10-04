using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.RecuitmentProcessEntities
{
    public class RecruitmentProcessEntity
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
        public List<DataEntity> data { get; set; }
    }

    public class DataEntity
    {
        public string id { get; set; }
        public string name { get; set; }
        public string created_by { get; set; }
        public string created_at { get; set; }
        public string is_delete { get; set; }
        public string apply_for { get; set; }
        public string slug { get; set; }
        public string com_id { get; set; }
        public string is_com { get; set; }
    }
}
