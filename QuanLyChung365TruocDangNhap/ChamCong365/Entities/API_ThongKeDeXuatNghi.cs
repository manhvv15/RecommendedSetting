using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ND
    {
        private string _ca_nghi;
        public string ca_nghi
        {
            get
            {
                var x = _ca_nghi;
                if (string.IsNullOrEmpty(_ca_nghi)) x = "Nghỉ cả ngày";
                return x;
            }
            set
            {
                _ca_nghi = value;
            }
        }
        

        public string ngaybatdau_nghi { get; set; }
        public string ngayketthuc_nghi { get; set; }

        public string thoigiannghi
        {
            get
            {
                return string.Format("Ngày bắt đầu:{0}\nNgày kết thúc:{1}", ngaybatdau_nghi, ngayketthuc_nghi);
            }
        }
    }
    public class InfoThongKeDeXuatNghi
    {
        public string loai_nghi_phep { get; set; }
        public string ly_do { get; set; }
        public string display_ly_do
        {
            get
            {
                string result = ly_do;
                while (result.Contains("</br>"))
                {
                    int x = result.IndexOf("</br>");
                    result = result.Remove(x, 5);
                    result = result.Insert(x, "\n");
                }
                return result;
            }
        }
        public List<ND> nd { get; set; }
    }
    public class Data_ThongKeDeXuatNghi
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_ThongKeDeXuatNghi> items { get; set; }
    }

    public class Item_ThongKeDeXuatNghi
    {
        public int STT { get; set; }
        public string id_de_xuat { get; set; }
        public string name_dx { get; set; }
        public string type_dx { get; set; }

        public string noi_dung
        {
            get;
            set;
        }
        public InfoThongKeDeXuatNghi info { get; set; }
        /*{
            get
            {
                var x = JsonConvert.DeserializeObject<InfoThongKeDeXuatNghi>(noi_dung);
                return x;
            }
        }*/
        public string name_user { get; set; }
        public string id_user { get; set; }
        public string IDnTen
        {
            get
            {
                return string.Format("({0}) {1}", id_user, name_user);
            }
        }
        public string com_id { get; set; }
        public string kieu_duyet { get; set; }
        public string id_user_duyet { get; set; }
        public string id_user_theo_doi { get; set; }
        public string file_kem { get; set; }
        public string type_duyet { get; set; }
        public string type_time { get; set; }
        public string time_start_out { get; set; }
        public string time_create { get; set; }
        public string time_tiep_nhan { get; set; }
        public string time_duyet { get; set; }
        public string active { get; set; }
        public string del_type { get; set; }
        public string dep_name { get; set; }
        public string ng_duyet { get; set; }
        private string _tenCa;
        public string tenCa
        {
            get
            {
                return _tenCa;
            }
            set { _tenCa = value; }
        }
    }

    public class API_ThongKeDeXuatNghi
    {
        public Data_ThongKeDeXuatNghi data { get; set; }
        public object error { get; set; }
    }
}
