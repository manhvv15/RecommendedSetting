using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataDetailOrganization
    {
        public bool result { get; set; }
        public string message { get; set; }
        public InfoCompany infoCompany { get; set; }
    }

    public class Employee
    {
        public int? com_id { get; set; }
        public int? dep_id { get; set; }
        public int? position_id { get; set; }
        public int? team_id { get; set; }
        public int? group_id { get; set; }
    }

    public class InfoChildCompany
    {
        public int? com_id { get; set; }
        public string com_name { get; set; }
        public int? tong_nv { get; set; }
        public int? tong_nv_da_diem_danh { get; set; }
        public int? tong_nv_chua_diem_danh
        {
            get
            {
                return tong_nv - tong_nv_da_diem_danh;
            }
        }
        public string manager { get; set; }
        public string deputy { get; set; }
        public List<InfoDep> infoDep { get; set; }
    }

    public class InfoCompany
    {
        public int? parent_com_id { get; set; }
        public string companyName { get; set; }
        public List<ParentManager> parent_manager { get; set; }
        public List<object> parent_deputy { get; set; }
        public int tong_nv { get; set; }
        public int tong_nv_da_diem_danh { get; set; }
        public int tong_nv_chua_diem_danh
        {
            get
            {
                return tong_nv - tong_nv_da_diem_danh;
            }
        }
        public List<InfoDep> infoDep { get; set; }
        public List<InfoChildCompany> infoChildCompany { get; set; }
    }

    public class InfoDep
    {
        public string manager { get; set; }
        public int? dep_id { get; set; }
        public string dep_name { get; set; }
        public string deputy { get; set; }
        public string description { get; set; }
        public int total_emp { get; set; }
        public int tong_nv_da_diem_danh { get; set; }
        public int tong_nv_chua_diem_danh
        {
            get
            {
                return tong_nv - tong_nv_da_diem_danh;
            }
        }
        public List<InfoTeam> infoTeam { get; set; }
        public int tong_nv { get; set; }
    }

    public class InForPerson
    {
        public Employee employee { get; set; }
    }

    public class InfoTeam
    {
        public int? gr_id { get; set; }
        public string gr_name { get; set; }
        public string description { get; set; }
        public string to_truong { get; set; }
        public string pho_to_truong { get; set; }
        public int tong_nv { get; set; }
        public int tong_nv_da_diem_danh { get; set; }
        public int tong_nv_chua_diem_danh
        {
            get
            {
                return tong_nv - tong_nv_da_diem_danh;
            }
        }
        public List<InfoGroup> infoGroup { get; set; }
    }
    public class InfoGroup
    {
        public int? gr_id { get; set; }
        public string gr_name { get; set; }
        public string description { get; set; }
        public string truong_nhom { get; set; }
        public string pho_truong_nhom { get; set; }
        public int group_tong_nv { get; set; }
        public int tong_nv_da_diem_danh { get; set; }
    }

    public class ParentManager
    {
        public int? _id { get; set; }
        public string userName { get; set; }
        public int? idQLC { get; set; }
        public InForPerson inForPerson { get; set; }
    }

    public class DetailOrganization
    {
        public DataDetailOrganization data { get; set; }
        public object error { get; set; }
    }

}
