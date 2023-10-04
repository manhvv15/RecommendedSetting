using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CyDetail
    {
        public string date { get; set; }
        public string shift_id { get; set; }
        public List<string> list_shift_id
        {
            get
            {
                if (!string.IsNullOrEmpty(shift_id)) return shift_id.Split(',').ToList();
                else return new List<string>();
            }
        }
    }

    public class LichLamViecData
    {
        public bool result { get; set; }
        public string message { get; set; }
        public LichLamViec item { get; set; }
    }

    public class LichLamViec
    {
        public string cy_id { get; set; }
        public string com_id { get; set; }
        public string cy_name { get; set; }
        public string apply_month { get; set; }
        public List<CyDetail> cy_detail { get; set; }
        public string status { get; set; }
        public string is_personal { get; set; }
    }

    public class API_LichLamViec_ChiTiet
    {
        public LichLamViecData data { get; set; }
        public Error error { get; set; }
    }
}
