using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.TrainingAndDeveloping
{
    public class ListTrainingProcessEntity
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
        private string Id;
        public string id
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
                SetId(Id);
            }
        }
        public string FullId { get; set; }
        private void SetId(string fullid)
        {
            FullId = "(QTTD" + fullid;
        }
        public string name { get; set; }
        public string description { get; set; }
        public string com_id { get; set; }
        public string is_delete { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string deleted_at { get; set; }
    }
}
