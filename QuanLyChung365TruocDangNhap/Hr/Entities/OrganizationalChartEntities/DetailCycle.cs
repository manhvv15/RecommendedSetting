using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    public class DetailCycle
    {
        public ListDetailCycle data { get; set; }
        public object error { get; set; }
    }
    public class ListDetailCycle
    {
        public bool result { get; set; }
        public string message { get; set; }
        public DetailCycleEntity item { get; set; }
    }
    public class DetailCycleEntity
    {
        public string cy_id { get; set; }
        public string com_id { get; set; }
        public string cy_name { get; set; }
        public string apply_month { get; set; }
        public List<Cy_detail> cy_detail { get; set; }
    }
    public class Cy_detail
    {
        public string date { get; set; }
        public string shift_id { get; set; }
    }
}
