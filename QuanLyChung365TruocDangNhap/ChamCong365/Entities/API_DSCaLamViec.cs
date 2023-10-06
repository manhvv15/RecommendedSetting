using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities
{
    public class DataDSCaLamViec
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<DSCaLamViec> items { get; set; }
    }

    public class DSCaLamViec : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string shift_id { get; set; }
        public string com_id { get; set; }
        public string shift_name { get; set; }
        public string start_time { get; set; }
        public string start_time_latest { get; set; }
        public string end_time { get; set; }
        public string end_time_earliest { get; set; }
        public string create_time { get; set; }
        public string over_night { get; set; }
        public string shift_type { get; set; }
        public string num_to_calculate { get; set; }
        public string num_to_money { get; set; }
        public string is_overtime { get; set; }
        public string status { get; set; }
        private bool _ischecked;
        public bool ischecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value; OnPropertyChanged();
            }
        }
    }

    public class API_DSCaLamViec
    {
        public DataDSCaLamViec data { get; set; }
        public object error { get; set; }
    }
}
