using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class getInfoByNestEntity
    {
        public DataInfoByNestEntity data { get; set; }
        public object error { get; set; }
    }
    public class DataInfoByNestEntity
    {
        public int itemsPerPage { get; set; }
        public int totalItems { get; set; }
        public InfoByNest items { get; set; }
 
    }
    public class InfoByNest
    {
        public List<ToTruong> to_truong { get; set; }
        public List<pho_to_truong> pho_to_truong { get; set; }
        public string tong_nv { get; set; }
        public int tong_nv_da_diem_danh { get; set; }
        public string tong_nv_chua_diem_danh { get; set; }
    }
    public class ToTruong
    {

    }
    public class pho_to_truong
    {

    }
}
