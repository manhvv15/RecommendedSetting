using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class ProposeEntities
    {
        public ListPropose data { get; set; }
        public object error { get; set; }
    }
    public class ListPropose
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ProposeEntity> items { get; set; }
    }
    public class ProposeEntity
    {
        public string noi_dung { get; set; }
        public string name_user { get; set; }
        public string id_user { get; set; }
        public string com_id { get; set; }
        public string id_user_duyet { get; set; }
        public string type_duyet { get; set; }
        public string time_create { get; set; }
        public string id_de_xuat { get; set; }
        public string link { get; set; }
    }

    public class NDDeXuat
    {
        public string ca_nghi { get; set; }
        public string ngaybatdau_nghi { get; set; }
        public string ngayketthuc_nghi { get; set; }
    }

    public class ThongTinDeXuat
    {
        public int loai_nghi_phep { get; set; }
        public string ly_do { get; set; }
        public List<NDDeXuat> nd { get; set; }
    }

}
