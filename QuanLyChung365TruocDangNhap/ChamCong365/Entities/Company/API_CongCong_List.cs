using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ND
    {
        public string ly_do { get; set; }
        private string _time_xnc;
        public string time_xnc
        {
            get
            {
                DateTime d;
                if (!string.IsNullOrEmpty(_time_xnc) && DateTime.TryParse(_time_xnc, out d)) return d.ToString("dd-MM-yyyy");
                return _time_xnc;
            }
            set
            {
                _time_xnc = value;
            }
        }
        public string time_vao_ca { get; set; }
        public string time_het_ca { get; set; }
        public string ca_xnc { get; set; }
        public string id_ca_xnc { get; set; }
        public string ghi_nhan_cong
        {
            get
            {
                return string.Format("Giờ vào ca: {0} Giờ hết ca: {1}", time_vao_ca, time_het_ca);
            }
        }
    }
    public class CongCongData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<CongCongItem> items { get; set; }
    }

    public class CongCongItem
    {
        public int STT { get; set; }
        public string id_de_xuat { get; set; }
        public string name_dx { get; set; }
        public string IDnTen
        {
            get
            {
                return string.Format("({0}) {1}", id_user, name_user);
            }
        }
        public string type_dx { get; set; }
        public string noi_dung { get; set; }
        public ND NoiDung { get; set; }
        /*{
            get
            {
                return JsonConvert.DeserializeObject<ND>(noi_dung);
            }
        }*/
        public string name_user { get; set; }
        public string id_user { get; set; }
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
        public string chuc_vu { get; set; }
        public string ng_duyet { get; set; }
    }

    public class API_CongCong_List
    {
        public CongCongData data { get; set; }
        public Error error { get; set; }
    }
}
