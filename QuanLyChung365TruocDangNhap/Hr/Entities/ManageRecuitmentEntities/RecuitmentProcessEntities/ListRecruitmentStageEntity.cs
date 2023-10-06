using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.RecuitmentProcessEntities
{
    public class ListRecruitmentStageEntity
    {
        public static int STT;
        public bool result { get; set; }
        public int code { get; set; }
        public Success2 success { get; set; }
        public object error { get; set; }

        public ListRecruitmentStageEntity()
        {
            STT = 1;
        }

    }

    public class Success2
    {
        public Data2 data { get; set; }
    }

    public class Data2
    {
        public int total { get; set; }
        public List<Data2Entity> data { get; set; }
    }

    public class Data2Entity
    {
        private string stt;
        public string Stt
        {
            get { return stt; }
            set
            {
                stt = value;
            }
        }
        public string id { get; set; }
        public string recuitment_id { get; set; }
        public string name { get; set; }
        public string position_assumed { get; set; }
        public string target { get; set; }
        public string complete_time { get; set; }
        public string description { get; set; }

        public Data2Entity()
        {
            Stt = ListRecruitmentStageEntity.STT.ToString();
            ListRecruitmentStageEntity.STT ++;
        }
    }
}
