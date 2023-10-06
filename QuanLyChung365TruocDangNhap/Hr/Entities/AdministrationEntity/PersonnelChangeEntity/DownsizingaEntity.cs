using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class DownsizingaEntity
    {
        public Data1 data { get; set; }
    }
    public class Data1
    {
        public int itemsPerPage { get; set; }
        public int totalItems { get; set; }
        public int totalCount { get; set; }
        public List<Items1> items { get; set; }
        public List<Items1> data { get; set; }
    }
    public class Items1
    {
        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }
        public string ep_image { get; set; }
        public string ep_address { get; set; }
        public string ep_gender { get; set; }
        public string ep_married { get; set; }
        public string ep_birth_day { get; set; }
        public string ep_description { get; set; }
        public string create_time { get; set; }
        public string role_id { get; set; }
        public string position_id { get; set; }
        public string position_name { get; set; }
        /*{
            get
            {
                return GetNamePositionApply(position_id);
            }
        }*/
        private string GetNamePositionApply(string position_apply)
        {
            switch (position_apply)
            {
                case "1":
                    return "SINH VIÊN THỰC TẬP";
                case "2":
                    return "NHÂN VIÊN THỬ VIỆC";
                case "3":
                    return "NHÂN VIÊN CHÍNH THỨC";
                case "4":
                    return "TRƯỞNG NHÓM";
                case "5":
                    return "PHÓ TRƯỞNG PHÒNG";
                case "6":
                    return "TRƯỞNG PHÒNG";
                case "7":
                    return "PHÓ GIÁM ĐỐC";
                case "8":
                    return "GIÁM ĐỐC";
                case "9":
                    return "NHÂN VIÊN PART TIME";
                case "10":
                    return "PHÓ BAN DỰ ÁN";
                case "11":
                    return "TRƯỞNG BAN DỰ ÁN";
                case "12":
                    return "PHÓ TỔ TRƯỞNG";
                case "13":
                    return "TỔ TRƯỞNG";
                case "14":
                    return "PHÓ TỔNG GIÁM ĐỐC";
                case "15":
                    return "";
                case "16":
                    return "TỔNG GIÁM ĐỐC";
                case "17":
                    return "THÀNH VIÊN HỘI ĐỒNG QUẢN TRỊ";
                case "18":
                    return "PHÓ CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ";
                case "19":
                    return "CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ";
                case "20":
                    return "NHÓM PHÓ";
                case "21":
                    return "TỔNG GIÁM ĐỐC TẬP ĐOÀN";
                case "22":
                    return "PHÓ TỔNG GIÁM ĐỐC TẬP ĐOÀN";
                default:
                    return "";
            }
        }
        public string type { get; set; }
        public string name_type
        {
            get
            {
                return GetNameType(type);
            }
        }
        private string GetNameType(string type_apply)
        {
            switch (type_apply)
            {
                case "1":
                    return "Giảm biên chế";
                case "2":
                    return "Nghỉ việc";
                default:
                    return "";
            }
        }
        public string ep_status { get; set; }
        public string gr_name { get; set; }
        public string parent_gr { get; set; }

        public string decision_id { get; set; }
        public string note { get; set; }
        private string _created_at {get; set;}
        public string created_at
        {
            get
            {
                if (string.IsNullOrEmpty(_created_at)) return time;
                return _created_at;
            }
            set
            {
                _created_at = value;
            }
        }
        public string time { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string shift_id { get; set; }
        public string shift_name { get; set; }

    }
}