using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities
{
    public class ListProcessInterviewEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success3 success { get; set; }
    }

    public class Success3
    {
        public Data3 data { get; set; }
    }

    public class Data3
    {
        public List<DataList> data { get; set; }
    }

    public class DataList
    {
        public string id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string total { get; set; }
        public List<DataEntity> data { get; set; }
    }
}
