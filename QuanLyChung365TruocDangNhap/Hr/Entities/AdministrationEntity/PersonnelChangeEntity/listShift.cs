using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class listShift
    {
        public listShiftData data { get; set; }
    }
    public class listShiftData
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<ListItems> items { get; set; }
    }
    public class ListItems
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
        //public string num_to_calculate { get; set; }
        public string num_to_money { get; set; }
        public string is_overtime { get; set; }
        public string status { get; set; }
    }
}
