using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WifiData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Wifi> items { get; set; }
    }

    public class Wifi
    {
        public string wifi_id { get; set; }
        public string com_id { get; set; }
        public string name_wifi { get; set; }
        public string mac_address { get; set; }
        public string ip_address { get; set; }
        public string create_time { get; set; }
        public string display_create_time
        {
            get
            {
                try
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
                    date = date.AddSeconds(long.Parse(create_time));
                    return date.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return create_time;
                }
            }
        }
        public string is_default { get; set; }
        public string status { get; set; }
    }

    public class API_Wifi_List
    {
        public WifiData data { get; set; }
        public Error error { get; set; }
    }
}
