using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity
{
    public class ForgotPassCompanyEntity
    {
        public Data3 data { get; set; }

        public Error3 error { get; set; }
    }
    public class Data3
    {
        public bool result { get; set; }
        public Forgot forgot { get; set; }

    }
    public class Forgot
    {
        public string new_pass { get; set; }
        public string email { get; set; }
        public string otp_code { get; set; }
    }
    public class Error3
    {
        public int code { get; set; }

        public string message { get; set; }
    }
}
