using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.RecruitmentProcessPopups
{
    /// <summary>
    /// Interaction logic for AddProcessPopup.xaml
    /// </summary>
    public partial class AddProcessPopup : UserControl
    {
        List<Stage> listStage = new List<Stage>();
        int stt_stage = 0;
        string token;
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        public AddProcessPopup(string token)
        {
            InitializeComponent();
            this.token = token;
        }

        private void AddStage(object sender, MouseButtonEventArgs e)
        {
            listStage.Add(new Stage() { stt = stt_stage });
            stt_stage++;
            icListStage.Items.Refresh();
            icListStage.ItemsSource = listStage;
        }

        private void DeleteStage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Stage stage = (Stage)grid.DataContext;
            int stt = stage.stt;
            foreach (var item in listStage)
            {
                if (item.stt == stt)
                {
                    listStage.Remove(item);
                    icListStage.ItemsSource = listStage;
                    icListStage.Items.Refresh();
                    break;
                }
            }
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void ClickAddProcess(object sender, MouseButtonEventArgs e)
        {
            tbForcus.Focus();
            if (ValidateForm())
            {
                ClickAddProcess();
            }

        }
        public class ListStage
        {
            public string nameStage { get; set; }
            public string posAssum { get; set; }
            public string target { get; set; }
            public string time { get; set; }
            public string des { get; set; }
        }

        public class DataCreateStage
        {
            public string nameProcess { get; set; }
            public string applyFor { get; set; }
            public List<ListStage> listStage { get; set; } = new List<ListStage>();
        }
        private async void ClickAddProcess()
        {
            try
            {
                DataCreateStage data = new DataCreateStage();
                data.nameProcess = tbName.Text;
                data.applyFor = tbObject.Text;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                //var multiForm = new MultipartFormDataContent();
                //multiForm.Add(new StringContent(token), "token");
                //multiForm.Add(new StringContent(tbName.Text), "name");
                //multiForm.Add(new StringContent(tbObject.Text), "object");
                ListStage stage = new ListStage()
                {
                    des = tbDescription.Text,
                    posAssum = tbJobStage.Text,
                    nameStage = tbNameStage.Text,
                    target = tbTarget.Text,
                    time = tbTime.Text,
                };
                data.listStage.Add(stage);
                if(listStage.Count > 0)
                {
                    foreach(var item in listStage)
                    {
                        ListStage stage1 = new ListStage()
                        {
                            des = item.desciption,
                            posAssum = item.jobs,
                            nameStage = item.names,
                            target = item.target,
                            time = item.time,
                        };
                        data.listStage.Add(stage1);
                    }
                }
                string listStagePost;
                listStagePost = JsonConvert.SerializeObject(data);
                //var json = JsonConvert.SerializeObject(listStagePost);
                //multiForm.Add(new StringContent(JsonConvert.SerializeObject(listStagePost)), "list_stage");
                var content = new StringContent(listStagePost, null, "application/json");
                var response = await client.PostAsync("http://210.245.108.202:3006/api/hr/recruitment/createRecruitment", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm quy trình tuyển dụng thành công!");
                }
                else
                {
                    MessageBox.Show("Tên quy trình tuyển dụng đã tồn tại");
                }

            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }


        private bool ValidateForm()
        {
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên quy trình không được để trống!");
                return false;
            }

            if (tbObject.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng áp dụng không được để trống!");
                return false;
            }

            if (tbNameStage.Text.Trim() == "")
            {
                MessageBox.Show("Tên giai đoạn không được để trống!");
                return false;
            }

            if (tbJobStage.Text.Trim() == "")
            {
                MessageBox.Show("Bộ phận đảm nhận không được để trống!");
                return false;
            }

            if (tbTarget.Text.Trim() == "")
            {
                MessageBox.Show("Mục tiêu không được để trống!");
                return false;
            }

            if (listStage.Count > 0)
            {
                foreach (var item in listStage)
                {
                    if (item.names == null || item.names.Trim() == "")
                    {
                        MessageBox.Show("Tên giai đoạn không được để trống!");
                        return false;
                    }
                    if (item.jobs == null || item.jobs.Trim() == "")
                    {
                        MessageBox.Show("Bộ phận đảm nhận không được để trống!");
                        return false;
                    }
                    if (item.target == null || item.target.Trim() == "")
                    {
                        MessageBox.Show("Mục tiêu không được để trống!");
                        return false;
                    }
                }
            }

            return true;
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }
    }

    public class Stage
    {
        public string desciption { get; set; }
        public string jobs { get; set; }
        public string names { get; set; }
        public int stt { get; set; }
        public string target { get; set; }
        public string time { get; set; }
    }
}
