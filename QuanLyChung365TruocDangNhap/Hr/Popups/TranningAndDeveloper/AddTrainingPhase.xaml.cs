using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.TrainingAndDeveloping;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.TranningAndDeveloper
{
    /// <summary>
    /// Interaction logic for AddTrainingPhase.xaml
    /// </summary>
    public partial class AddTrainingPhase : UserControl
    {
        string token;
        string id;
        //string training_process_id;
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        //List<ListStageTraining> data = new List<ListStageTraining>();

        public AddTrainingPhase(string token, string id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
        }
        private void CanCelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void AddNewTrainPhase(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                AddNewDataTrainPhase();
        }
        private async void AddNewDataTrainPhase()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.addStageTrainingProcess;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("name", tbTenGD.Text));
                parameters.Add(new KeyValuePair<string, string>("objectTraining", tbDoiTuongDT.Text));
                parameters.Add(new KeyValuePair<string, string>("content", StringFromRichTextBox(tbMoTaNgan)));
                parameters.Add(new KeyValuePair<string, string>("trainingProcessId", id));



                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show(result.success.message);
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("Hihi");
            }
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        private bool ValidateForm()
        {
            if (tbTenGD.Text.Trim() == "")
            {
                MessageBox.Show("Tên giai đoạn không được để trống.");
                return false;
            }

            if (tbDoiTuongDT.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng đào tạo không được để trống.");
                return false;
            }

            if (StringFromRichTextBox(tbMoTaNgan).Trim() == "")
            {
                MessageBox.Show("Mô tả không được để trống.");
                return false;
            }

            return true;
        }
    }
}
