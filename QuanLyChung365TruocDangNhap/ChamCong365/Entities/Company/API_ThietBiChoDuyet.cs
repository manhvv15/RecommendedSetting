using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataThietBiChoDuyet
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ThietBiChoDuyet> items { get; set; }
    }

    public class ThietBiChoDuyet: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string ed_id { get; set; }
        public string current_device { get; set; }
        public string current_device_name { get; set; }
        public string new_device { get; set; }
        public string new_device_name { get; set; }
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string Display_ep_name
        {
            get
            {
                return "(" + ep_id + ")" + ep_name;
            }
        }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string display_ep_image
        {
            get
            {
                string x = "https://chamcong.24hpay.vn/upload/employee/" + ep_image;
                if(string.IsNullOrEmpty(ep_image))
                    x = "https://chamcong.timviec365.vn/images/ep_logo.png";
                return x;
            }
        }
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        private bool _isCheck;
        public bool isCheck
        {
            get { return _isCheck; }
            set { _isCheck = value; OnPropertyChanged(); }
        }
    }

    public class API_ThietBiChoDuyet
    {
        public DataThietBiChoDuyet data { get; set; }
        public object error { get; set; }
    }
}
