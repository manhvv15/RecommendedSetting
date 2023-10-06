using QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class getListEmployee
    {
        public static int STT;
        public Data5 data { get; set; }
        public getListEmployee()
        {
            STT = 1;
        }
    }
    public class Data5
    {
        public int itemsPerPage { get; set; }
        public int totalItems { get; set; }
        public List<Items3> items { get; set; }
       
    }
   
    public class Items3
    {
        public List<ProposeEntity> proposeEntities { get; set; }
        

        public string ep_id { get; set; }
        public string ep_email { get; set; }
        public string ep_name { get; set; }
        public string ep_phone { get; set; }

        public string noi_dung { get; set; }

        public string xem_chi_tiet { get; set; }

        private string Ep_image;
        public string ep_image
        {
            get
            {
                return Ep_image;
            }
            set
            {
                Ep_image = value;
                SetId(Ep_image);
            }
        }
        public string Url_image { get; set; }
        private void SetId(string url_image)
        {
            Url_image = "https://chamcong.24hpay.vn/upload/employee/" + url_image;
        }

        public string ep_address { get; set; }
        private string Ep_education;
        public string ep_education
        {
            get
            {
                return GetEducation(Ep_education);
            }
            set
            {
                Ep_education = value;
            }
        }

        


        public string ep_exp { get; set; }
        public string Ep_exp
        {
            get
            {
                return GetCanExp(ep_exp);
            }
        }


        public string ep_birth_day { get; set; }
        private string Ep_married;
        public string ep_married {
            get
            {
                return GetCanMarried(Ep_married);
            }
            set
            {
                Ep_married = value;
            }
        }
        public string ep_gender { get; set; }
        public string Ep_gender
        {
            get
            {
                return GetGender(ep_gender);
            }
        }
        public string role_id { get; set; }
        public string position_name { get; set; }
        /*{
            get
            {
                return GetNamePositionApply(position_id);
            }
        }*/
        public string position_id { get; set; }
        public string ep_status { get; set; }
        public string update_time { get; set; }
        public string allow_update_face { get; set; }
        public string com_id { get; set; }
        public string com_name { get; set; }
        public string com_logo { get; set; }
        public string dep_id { get; set; }
        public string dep_name { get; set; }
        public string create_time { get; set; }
        public string link_dx { get; set; }
        public bool isTime { get; set; }
        //private string Create_time;
        //public string create_time
        //{
        //    get
        //    {
        //        return ConvertDate(Create_time, "dd-MM-yyyy HH:mm:ss");
        //    }
        //    set
        //    {
        //        Create_time = value;
        //    }
        //}

        public string group_id { get; set; }
        public string old_position_id { get; set; }
        public string old_position_name
        {
            get
            {
                return GetNamePositionApply(old_position_id);
            }
        }
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

        private string GetCanMarried(string can_is_married)
        {
            switch (can_is_married)
            {
                case "1":
                    return "Độc thân";
                case "2":
                    return "Đã lập gia đình";
                default:
                    return "Khác";
            }
        }

        private string GetGender(string gender)
        {
            switch (gender)
            {
                case "1":
                    return "Nam";
                case "2":
                    return "Nữ";
                case "3":
                    return "Khác";
                default:
                    return "";
            }
        }

        private string GetCanExp(string exp)
        {
            switch (exp)
            {
                case "0":
                    return "Chưa có kinh nghiệm";
                case "1":
                    return "0 - 1 năm kinh nghiệm";
                case "2":
                    return "1 - 2 năm kinh nghiệm";
                case "3":
                    return "2 - 5 năm kinh nghiệm";
                case "4":
                    return "5 - 10 năm kinh nghiệm";
                case "5":
                    return "Hơn 10 năm kinh nghiệm";
                default:
                    return "Chưa cập nhật";
            }
        }

        private string GetEducation(string can_education)
        {
            switch (can_education)
            {
                case "1":
                    return "THPT trở lên";
                case "2":
                    return "Đại Học";
                case "3":
                    return "Cao đẳng";
                case "4":
                    return "Trung cấp trở lên";
                case "5":
                    return "Cao đẳng trở lên";
                case "6":
                    return "Cử nhân trở lên";
                case "7":
                    return "Đại học trở lên";
                case "8":
                    return "Thạc sĩ trở lên";
                case "9":
                    return "Thạc sĩ Nghệ thuật";
                case "10":
                    return "Thạc sĩ Thương mại";
                case "11":
                    return "Thạc sĩ Khoa học";
                case "12":
                    return "Thạc sĩ Kiến trúc";
                case "13":
                    return "Thạc sĩ QTKD";
                case "14":
                    return "Thạc sĩ Kỹ thuật ứng dụng";
                case "15":
                    return "Thạc sĩ Luật";
                case "16":
                    return "Thạc sĩ Y học";
                case "17":
                    return "Thạc sĩ Dược phẩm";
                case "18":
                    return "Tiến sĩ";
                case "19":
                    return "Khác";
                default:
                    return "";

            }
        }

        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd-MM-yyyyTHH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "dd-MM-yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "dd-MM-yyyy",
                                                      System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString(format);
                    }
                    catch
                    {
                        try
                        {
                            myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                          System.Globalization.CultureInfo.InvariantCulture);
                            return myDate.ToString(format);
                        }
                        catch
                        {
                            try
                            {
                                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                              System.Globalization.CultureInfo.InvariantCulture);
                                return myDate.ToString(format);
                            }
                            catch
                            {
                                try
                                {
                                    myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                                  System.Globalization.CultureInfo.InvariantCulture);
                                    return myDate.ToString(format);
                                }
                                catch
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }
            }

        }

        private string stt;
        public string Stt
        {
            get { return stt; }
            set
            {
                stt = value;
            }
        }
        public Items3()
        {
            Stt = getListEmployee.STT.ToString();
            getListEmployee.STT++;
        }
    }
}
