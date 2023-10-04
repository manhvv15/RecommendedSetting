using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChung365TruocDangNhap.Hr.Entities.TrainingAndDeveloping
{
    public class StageList
    {
        public static int STT;
        public bool result { get; set; }
        public int code { get; set; }
        public Success1 success { get; set; }
        public object error { get; set; }
        public StageList()
        {
            STT = 1;
        }
    }
    public class Success1
    {
        public Data1 data { get; set; }
        public string message { get; set; }
    }
    public class Data1
    {
        public int total { get; set; }
        public List<ListStageTraining> data { get; set; }

    }
    public class ListStageTraining
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
        public string name { get; set; }
        public string object_training { get; set; }
        public string content { get; set; }
        public string training_process_id { get; set; }
        public string is_delete { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public ListStageTraining()
        {
            Stt = StageList.STT.ToString();
            StageList.STT++;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataListStageTraining1
    {
        public bool result { get; set; }
        public string message { get; set; }
        public ProcessTrain processTrain { get; set; }
        public List<ListStage> listStage { get; set; }
    }

    public class ListStage
    {
        public string _id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string objectTraining { get; set; }
        public string content { get; set; }
        public int trainingProcessId { get; set; }
        public int isDelete { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedAt { get; set; }
        public string deleteddAt { get; set; }
    }

    public class ProcessTrain
    {
        public string _id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int comId { get; set; }
        public int isDelete { get; set; }
        public DateTime createdAt { get; set; }
        public object updatedAt { get; set; }
        public object deletedAt { get; set; }
    }

    public class ListStageTraining1
    {
        public DataListStageTraining1 data { get; set; }
        public object error { get; set; }
    }
}
