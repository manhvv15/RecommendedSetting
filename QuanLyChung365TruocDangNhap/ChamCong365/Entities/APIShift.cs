using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    internal class APIShift
    {
        public DataShift data { get; set; }
        public Error error { get; set; }
    }
    public class DataShift
    {
        public bool result { get; set; }
        public string message { get; set; }
        public List<ItemShift> shift { get; set; }
    }
    public class ItemShift
    {
        public string shift_id { get; set; }
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }

    public class APICheckFace
    {
        public APICheckFace()
        {

        }

        public APICheckFace(string company_id, string image)
        {
            this.company_id = company_id;
            this.image = image;
        }

        public APICheckFace(string company_id, string user_id, string image )
        {
            this.company_id = company_id;
            this.user_id = user_id;
            this.image = image;
        }

        public string company_id { get; set; }
        public string user_id { get; set; }
        public string image { get; set; }
        public string status { get; set; }
        public string mess { get; set; }
        public bool message { get; set; }
        
    }
}

