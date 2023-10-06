using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.PerformRecuitmentEntities
{
    public class PerformDetail
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success4 success { get; set; }
        public object error { get; set; }
    }

    public class Success4
    {
        public Data4 data { get; set; }
        public string message { get; set; }
    }

    public class Data4
    {
        public DataEntity4 data { get; set; }
        public List<CandidateInfo> listCandidate { get; set; }
        public List<CandidateInfo> listOfferJob { get; set; }
        public List<CandidateInfo> listInterview { get; set; }
        public List<CandidateInfo> listInterviewPass { get; set; }
        public List<CandidateInfo> listInterviewFail { get; set; }
    }

    public class DataEntity4
    {
        public string id { get; set; }
        public string title { get; set; }
        public string title_full
        {
            get
            {
                return "(TD" + id + ") " + title;
            }
        }
        public string position_apply { get; set; }
        public string cit_id { get; set; }
        public string address { get; set; }
        public string cate_id { get; set; }
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
        public string number { get; set; }
        public string recruitment_time { get; set; }
        public string recruitment_time_to { get; set; }
        public string job_detail { get; set; }
        public string woking_form { get; set; }
        public string workingform
        {
            get
            {
                return SetWorkingForm(woking_form);
            }
        }
        public string probationary_time { get; set; }
        public string money_tip { get; set; }
        public string job_description { get; set; }
        public string interest { get; set; }
        public string recruitmen_id { get; set; }
        public string job_exp { get; set; }
        public string degree { get; set; }
        public string gender { get; set; }
        public string job_require { get; set; }
        public string member_follow { get; set; }
        public string hr_name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string deleted_at { get; set; }
        public string is_delete { get; set; }
        public string com_id { get; set; }
        public string is_com { get; set; }
        public string created_by { get; set; }
        public string is_sample { get; set; }

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

        private string SetWorkingForm(string working_form)
        {
            if (working_form == null || working_form == "") return "";
            List<string> listWorkingForm = new List<string>() { "Vui lòng chọn", "Toàn thời gian cố định", "Toàn thời gian tạm thời",
                "Bán thời gian", "Bán thời gian tạm thời", "Hợp đồng", "Khác"};
            int indexChoose = Convert.ToInt32(working_form);
            return listWorkingForm[indexChoose];
        }
    }

    public class CandidateInfo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
