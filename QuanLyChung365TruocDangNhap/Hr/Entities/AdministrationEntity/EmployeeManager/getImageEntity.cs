using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager
{
    public class getImageEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public ImageEntity success { get; set; }
        public string error { get; set; }
    }
    public class ImageEntity
    {
        public ImageEntityData data { get; set; }
        public string message { get; set; }
    }
    public class ImageEntityData
    {
        public ImageEntityData2 data { get; set; }
    }
    public class ImageEntityData2
    {
        public string ep_id { get; set; }
        public string image_name { get; set; }
        public string created_at { get; set; }
        public string is_delete { get; set; }
        public string deleted_at { get; set; }
        public string id { get; set; }
}
}
