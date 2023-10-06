using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities
{
    public class ProcessEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success4 success { get; set; }
        public Success4 data { get; set; }
        public Error error { get; set; }
    }

    public class Success4
    {
        public object data { get; set; }
        public string message { get; set; }
        public bool result { get; set; }
    }

    public class Error
    {
        public int code { get; set; }

        public string message { get; set; }
    }
}
