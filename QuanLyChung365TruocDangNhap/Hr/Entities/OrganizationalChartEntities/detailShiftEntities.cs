using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class detailShiftEntities
    {
        public ListDetailShift data { get; set; }
        public object error { get; set; }
    }
    public class ListDetailShift
    {
        public bool result { get; set; }
        public string message { get; set; }
        public DetailShift detailShift { get; set; }
    }
    public class DetailShift
    {
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
    }
}
