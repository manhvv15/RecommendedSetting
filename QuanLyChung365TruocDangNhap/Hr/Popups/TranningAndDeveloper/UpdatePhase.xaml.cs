using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
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
    /// Interaction logic for UpdatePhase.xaml
    /// </summary>
    public partial class UpdatePhase : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

        public string id;
        public string token;
        public string name;
        public string object_training;
        public string content;
        public UpdatePhase(string token, string id, string name, string object_training, string content)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            tbName.Text = name;
            tbObject.Text = object_training;
            tbContent.Selection.Text = content;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void UpdatePhaseData(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                GetDataPhase();
        }
        private async void GetDataPhase()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.updateStageTrainingProcess;

            httpRequestMessage.RequestUri = new Uri(api);
            var parameters = new List<KeyValuePair<string, string>>();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);;
            parameters.Add(new KeyValuePair<string, string>("stageProcessTrainingId", id));
            parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));
            parameters.Add(new KeyValuePair<string, string>("objectTraining", tbObject.Text));
            parameters.Add(new KeyValuePair<string, string>("content", StringFromRichTextBox(tbContent)));

            // Thiết lập Content
            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            AcgievementEntity result = JsonConvert.DeserializeObject<AcgievementEntity>(responseContent);

            if (responseContent.Contains("\"result\":true"))
            {
                hidePopup(1);
                MessageBox.Show("Chỉnh sửa thành công!");
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
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên giai đoạn không được để trống.");
                return false;
            }

            if (tbObject.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng đào tạo.");
                return false;
            }

            if (StringFromRichTextBox(tbContent).Trim() == "")
            {
                MessageBox.Show("Nội dung không được để trống.");
                return false;
            }

            return true;
        }
    }
}
