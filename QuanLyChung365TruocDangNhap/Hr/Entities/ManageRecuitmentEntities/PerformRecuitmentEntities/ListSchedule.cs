using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.PerformRecuitmentEntities
{
    public class ListSchedule
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success3 success { get; set; }
        public string message { get; set; }
        public object error { get; set; }

    }

    public class Success3
    {
        public Data3 data { get; set; }
    }

    public class Data3
    {
        public int total { get; set; }
        public List<DataEntity3> data { get; set; }
    }

    public class DataEntity3
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string cv_from { get; set; }
        public string user_recommend { get; set; }
        public string salary_id { get; set; }
        public string salary
        {
            get
            {
                return SetSalary(salary_id);
            }
            set
            {
                salary_id = value;
            }
        }
        public string recruitment_news_id { get; set; }
        public string time_send_cv { get; set; }
        public string interview_time { get; set; }
        public string interview_result { get; set; }
        public string interview_vote { get; set; }
        public string salary_agree { get; set; }
        public string status { get; set; }
        public string cv { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string is_delete { get; set; }
        public string com_id { get; set; }
        public string is_offer_job { get; set; }
        public string can_gender { get; set; }
        public string can_birthday { get; set; }
        public string can_education { get; set; }
        public string can_exp { get; set; }
        public string can_is_married { get; set; }
        public string can_address { get; set; }
        public string user_hiring { get; set; }
        public string star_vote { get; set; }
        public string school { get; set; }
        public string hometown { get; set; }
        public string is_switch { get; set; }
        public string thoigianphongvan { get; set; }
        public string title { get; set; }
        public string time
        {
            get
            {
                return DateTime.Parse(thoigianphongvan).ToString("H:mm");
            }
        }
        public string day_of_week 
        { 
            get {
                return DateTime.Parse(thoigianphongvan).ToString("dddd"); 
            } 
        }
        public string day_month
        {
            get
            {
                return DateTime.Parse(thoigianphongvan).ToString("dd/M");
            }
        }

        private string SetSalary(string salary_id)
        {
            if (salary_id == null || salary_id == "")
            {
                return "";
            }
            else
            {
                List<string> listSalary = new List<string>() { "Chưa chọn", "Thỏa thuận", "1 - 3 triệu", "3 - 5 triệu", "5 - 7 triệu",
                "7 - 10 triệu", "10 - 15 triệu", "15 - 20 triệu", "20 - 30 triệu", "Trên 30 triệu", "Trên 50 triệu", "Trên 100 triệu" };
                int indexChoose = Convert.ToInt32(salary_id);
                return listSalary[indexChoose];
            }
        }
    }
}