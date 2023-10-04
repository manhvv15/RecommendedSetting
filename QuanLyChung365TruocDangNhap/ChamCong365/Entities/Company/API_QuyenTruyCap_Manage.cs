﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data_QuyenTruyCap_Manage
    {
        public int itemsPerPage { get; set; }
        public string totalItems { get; set; }
        public List<Item_QuyenTruyCap_Manage> items { get; set; }
    }

    public class Item_QuyenTruyCap_Manage
    {
        public string role_id { get; set; }
        public string group_id { get; set; }
        public object org_id { get; set; }
        public string role_name { get; set; }
        public object description { get; set; }
        public string rank { get; set; }
        public string rights { get; set; }
        public string status { get; set; }
    }

    public class API_QuyenTruyCap_Manage
    {
        public Data_QuyenTruyCap_Manage data { get; set; }
        public object error { get; set; }
    }
}
