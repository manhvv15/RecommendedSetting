using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.SettingEntities
{
    public class CompanyInfoEntity
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public bool result { get; set; }
        public string message { get; set; }
        public DataEntity data { get; set; }
    }

    public class DataEntity
    {
        public string com_id { get; set; }
        public string com_parent_id { get; set; }
        public string com_name { get; set; }
        public string com_email { get; set; }
        public string com_phone { get; set; }
        public string com_logo { get; set; }
        public string com_address { get; set; }
        public string com_size { get; set; }
        public string com_description { get; set; }
        public string com_create_time { get; set; }
    }
}
