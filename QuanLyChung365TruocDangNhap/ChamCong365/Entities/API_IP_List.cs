using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class IPData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<IP> items { get; set; }
    }

    public class IP
    {
        public string ip_id { get; set; }
        public string name_ip { get; set; }
        public string com_id { get; set; }
        public string ip_address { get; set; }
        public string type { get; set; }
        public string create_time { get; set; }
        /*{
            get
            {
                try
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
                    date.AddSeconds(long.Parse(create_time));
                    return date.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return create_time;
                }
            }
            set { create_time = value.ToString(); }
        }*/
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

    public class API_IP_List
    {
        public IPData data { get; set; }
        public Error error { get; set; }
    }
}
