using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Shift_Manage_Data
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Shift_Manage_Item> items { get; set; }
    }

    public class Shift_Manage_Item
    {
        public string shift_id { get; set; }
        public string com_id { get; set; }
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string start_timex
        {
            get
            {
                string kq = "";
                if (!string.IsNullOrEmpty(start_time) && start_time.Contains(":"))
                {
                    var arr = start_time.Split(':');
                    int so;
                    if (int.TryParse(arr[0], out so))
                    {
                        if (so <= 12) kq = string.Format("{0}:{1} a.m", arr[0], arr[1]);
                        if (so > 12) kq = string.Format("{0}:{1} p.m", arr[0], arr[1]);
                    }
                }
                return kq;
            }
        }
        public string start_time_latest { get; set; }
        public string end_time { get; set; }
        public string end_timex
        {
            get
            {
                string kq = "";
                if (!string.IsNullOrEmpty(end_time) && end_time.Contains(":"))
                {
                    var arr = end_time.Split(':');
                    int so;
                    if (int.TryParse(arr[0], out so))
                    {
                        if (so <= 12) kq = string.Format("{0}:{1} a.m", arr[0], arr[1]);
                        if (so > 12) kq = string.Format("{0}:{1} p.m", arr[0], arr[1]);
                    }
                }
                return kq;
            }
        }
        public string end_time_earliest { get; set; }
        public string create_time { get; set; }
        public string over_night { get; set; }
        public string shift_type { get; set; }
        public string num_to_calculate { get; set; }
        public string num_to_money { get; set; }
        public string is_overtime { get; set; }
        public string status { get; set; }
        public bool IsAddItem { get; set; }
    }

    public class API_Shift_Manage
    {
        public Shift_Manage_Data data { get; set; }
        public Error error { get; set; }
    }
}
