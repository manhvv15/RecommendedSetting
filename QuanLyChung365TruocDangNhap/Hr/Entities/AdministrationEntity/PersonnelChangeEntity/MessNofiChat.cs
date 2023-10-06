using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class MessNofiChat
    {
        public NofiChat data { get; set; }
        public string error { get; set; }
    }
    public class NofiChat { 
        public bool result { get; set; }
        public string message { get; set; }
        public string listNotification { get; set; }
    }
}
