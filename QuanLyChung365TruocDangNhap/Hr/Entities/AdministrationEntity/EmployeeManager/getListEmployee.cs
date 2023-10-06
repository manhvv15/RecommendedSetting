using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager
{
    public class getListEmployee
    {
        public Data data { get; set; }
        public object error { get; set; }
    }
    public class Data
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<getListEmployee1> items { get; set; }
    }
    public class getListEmployee1
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_education { get; set; }
        public string ep_exp { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string gender { get; set; }
        private string Ep_gender;
        public string ep_gender
        {
            get
            {
                return Ep_gender;
            }
            set
            {
                Ep_gender = value;
                SetGender(Ep_gender);
            }
        }
        private void SetGender(string ep_gender)
        {
            switch (ep_gender)
            {
                case "1":
                    gender = "Nam";
                    break;
                case "2":
                    gender = "Nữ";
                    break;
                default:
                    gender = "Khác";
                    break;
            }
        }
        private string Ep_married;
        public string married { get; set; }
        public string ep_married
        {
            get
            {
                return Ep_married;
            }
            set
            {
                Ep_married = value;
                SetName(Ep_married);
            }
        }
        private void SetName(string Ep_married)
        {
            switch (Ep_married)
            {
                case "1":
                    married = "Độc thân";
                    break;
                case "2":
                    married = "Đã kết hôn";
                    break;
            }
        }
        public string ep_birth_day { get; set; }
        public string ep_description { get; set; }
        public string create_time { get; set; }
        public string role_id { get; set; }
        public string group_id { get; set; }
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string update_time { get; set; }
        public string allow_update_face { get; set; }
        public string com_id { get; set; }
        public string gr_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string gr_name { get; set; }
        public string parent_gr { get; set; }
        public string start_working_time { get; set; }
    }
}
