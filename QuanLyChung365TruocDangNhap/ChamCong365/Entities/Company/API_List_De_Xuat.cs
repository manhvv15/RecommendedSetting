using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataAPI_List_De_Xuat
    {
        public bool result { get; set; }
        public string message { get; set; }
        public List<DeXuat> data { get; set; }
        public int totalPages { get; set; }
        
    }
    public class DeXuat
    {
        public int _id { get; set; }
        public string name_dx { get; set; }
        public string name_user { get; set; }
        public int type_duyet { get; set; }
        public int time_create { get; set; }
    }
    public class API_List_De_Xuat
    {
        public DataAPI_List_De_Xuat data { get; set; }
        public object error { get; set; }
    }
}
