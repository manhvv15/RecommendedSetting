using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.RecentDeletedData
{
    public class RecentDeletedDataEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success success { get; set; }
    }

    public class Success
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public DataList list_delete_recuitment { get; set; }
        public DataList list_delete_recuitment_new { get; set; }
        public DataList list_delete_job_description { get; set; }
        public DataList list_delete_training_process { get; set; }
        public DataList list_delete_provision { get; set; }
        public DataList list_delete_employe_policy { get; set; }
    }

    public class DataList
    {
        public int total { get; set; }
        public List<DataEntity> data { get; set; }
    }
    
    public class DataEntity
    {
        public string full_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }

        private string Delete_at;
        public string deleted_at 
        {
            get
            {
                return ConvertDate(Delete_at);
            }
            set
            {
                Delete_at = value;
            }
        }
        public string time { get; set; }
        public bool isCheck { get; set; }

        private string ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }

        }
    }
}
