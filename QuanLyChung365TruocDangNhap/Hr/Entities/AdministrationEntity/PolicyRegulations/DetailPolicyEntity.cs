﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations
{
    public class DetailPolicyEntity
    {
        public bool result { get; set; }
        public int code { get; set; }
        public Success5 success { get; set; }
    }

    public class Success5
    {
        public Data5 data { get; set; }
        public string  message { get; set; }
    }

    public class Data5
    {
        public DataEntity5 data { get; set; }
    }

    public class DataEntity5
    {
        public string id { get; set; }
        public string provision_id { get; set; }
        public string employe_policy_id { get; set; }
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
        public string time_start
        {
            get
            {
                return ConvertDate(Time_start, "dd/MM/yyyy");
            }
            set
            {
                Time_start = value;
            }
        }
        public string supervisor_name { get; set; }
        public string apply_for { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public string created_by { get; set; }
        public string is_delete { get; set; }
        public string name { get; set; }
        public string file { get; set; }
        public string deleted_at { get; set; }
        public string qd_name { get; set; }
        public string updated_at { get; set; }

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
