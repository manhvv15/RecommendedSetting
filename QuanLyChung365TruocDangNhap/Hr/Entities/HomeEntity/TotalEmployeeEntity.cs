using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity
{
    public class TotalEmployeeEntity
    {
        public Data2 data { get; set; }
        public object error { get; set; }
    }

    public class Data2
    {
        public int itemsPerPage { get; set; }
        public int totalItems { get; set; }
        public TotalEmploy items { get; set; }
        public string total_active { get; set; }
        public string total_not_active { get; set; }
    }
    public class TotalEmploy
    {
        public string tong_nv { get; set; }
        public int tong_nv_da_diem_danh { get; set; }
        public string tong_nv_chua_diem_danh { get; set; }
    }
}
