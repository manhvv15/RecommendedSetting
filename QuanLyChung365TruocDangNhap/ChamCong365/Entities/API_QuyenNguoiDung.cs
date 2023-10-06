using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    class API_QuyenNguoiDung
    {
        public string permission_id { get; set; }
        public List<string> quyen
        {
            get
            {
                var t = new List<string>();
                if (!string.IsNullOrEmpty(permission_id))
                {
                    var list = new List<char>() { '\"', '[', ',', ']', '\\' };
                    for (int i = 0; i < permission_id.Length; i++)
                    {
                        if (!list.Contains(permission_id[i])) t.Add(permission_id[i].ToString());
                    }
                }
                return t;
            }
        }
    }
}
