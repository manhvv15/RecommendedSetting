using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity
{
    public class UpDownSalaryEntity
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Data3> data { get; set; }
        public int totalRecord { get; set; }
    }
    public class Data3
    {
        public string sb_id_user { get; set; }
        public string sb_ep_name { get; set; }
        public string sb_dep_id { get; set; }
        public string sb_dep_name { get; set; }
        public string sb_salary_basic { get; set; }
        public string display_sb_salary_basic
        {
            get
            {
                string a = "0";
                if (Convert.ToDouble(sb_salary_basic) >= 0)
                {
                    double m;
                    if(double.TryParse(sb_salary_basic, out m)) a = m.ToString("C0").Replace(@"$", "");
                }
                else
                {
                    double n;
                    if (double.TryParse(sb_salary_basic.ToString(), out n))
                        a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                }
                return a;
            }
        }
        public string sb_time_up { get; set; }
        public string display_sb_time_up
        {
            get
            {
                try
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
                    date = date.AddSeconds(long.Parse(sb_time_up));
                    return date.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return sb_time_up;
                }
            }
        }
        public string sb_lydo { get; set; }
        public string sb_quyetdinh { get; set; }
        public int sb_tanggiam { get; set; }
        public double salary_up { get; set; }
        public string display_salary_up
        {
            get
            {
                if (sb_tanggiam > 0)
                {
                    string a = "";
                    if (Convert.ToDouble(sb_tanggiam) >= 0)
                    {
                        double m = sb_tanggiam;
                        a = m.ToString("C0").Replace(@"$", "");
                    }
                    else
                    {
                        double n;
                        if (double.TryParse(sb_tanggiam.ToString(), out n))
                            a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                    }

                    return a;
                }
                else return "0";
            }
        }
        public double salary_down { get; set; }
        public string display_salary_down
        {
            get
            {
                if (sb_tanggiam < 0)
                {
                    string a = "";
                    if (Convert.ToDouble(sb_tanggiam) >= 0)
                    {
                        double m = sb_tanggiam;
                        a = m.ToString("C0").Replace(@"$", "");
                    }
                    else
                    {
                        double n;
                        if (double.TryParse(sb_tanggiam.ToString(), out n))
                            a = "-" + n.ToString("C0").Replace(@"$", "").Replace(@"(", "").Replace(@")", "");
                    }

                    return a;
                }
                else return "0";
            }
        }
        public string position_name { get; set; }
    }
}
