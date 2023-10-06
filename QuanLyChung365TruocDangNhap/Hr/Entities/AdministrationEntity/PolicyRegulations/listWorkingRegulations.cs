using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations
{
    public class listWorkingRegulations
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success2 success { get; set; }
        public object error { get; set; }
    }
    public class Success2
    {
        public Data data { get; set; }
        public string message { get; set; }
    }
    public class Data
    {
        public List<DataEntity> data { get; set; }
        public int total { get; set; }
    }
    public class DataEntity
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string supervisor_name { get; set; }
        private string Created_at;
        public string created_at
        {
            get
            {
                return ConvertDate(Created_at, "dd/MM/yyyy");
            }
            set
            {
                Created_at = value;
            }
        }
        private string Time_start;
        public string time_start {
            get
            {
                return ConvertDate(Time_start, "dd/MM/yyyy");
            }
            set
            {
                Time_start = value;
            }
        }
        public string file { get; set; }
        public List<DataEntity3> data_detail { get; set; }
        public bool show { get; set; }
        public string com_id { get; set; }
        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "yyyy/MM/dd",
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
                                myDate = DateTime.ParseExact(date, "MM/dd/yyyy",
                                                              System.Globalization.CultureInfo.InvariantCulture);
                                return myDate.ToString(format);
                            }
                            catch
                            {
                                try
                                {
                                    myDate = DateTime.ParseExact(date, "M/d/yyyy",
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
    }
}
