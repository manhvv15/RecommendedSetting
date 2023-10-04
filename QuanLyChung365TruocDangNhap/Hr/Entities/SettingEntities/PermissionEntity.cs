using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.SettingEntities
{
    public class PermissionEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success success { get; set; }

        public object error { get; set; }
    }

    public class Success
    {
        public DataPermission data { get; set; }
    }

    public class DataPermission
    {
        public List<Role> list_permision { get; set; }
    }

    public class Role
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string per_id { get; set; }
        public string bar_id { get; set; }
    }
}
