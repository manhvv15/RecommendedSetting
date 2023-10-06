using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity
{
    public class verifyOtp
    {
        public VerifyOtp data { get; set; }

        public string error { get; set; }
    }

    public class VerifyOtp
    {
        public bool result { get; set; }
        public string message { get; set; }
    }
}
