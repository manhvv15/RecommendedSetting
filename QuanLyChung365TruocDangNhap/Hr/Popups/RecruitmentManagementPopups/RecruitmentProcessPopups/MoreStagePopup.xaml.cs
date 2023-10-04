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
    /// Interaction logic for MoreStagePopup.xaml
    /// </summary>
    public partial class MoreStagePopup : UserControl
    {
        string token;
        string id;
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại

        public static event HidePopup hidePopup;
        public MoreStagePopup(string token, string id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void AddStage(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
            {
                AddStage();
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
        private async void AddStage()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.add_stage;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("posAssum", tbJob.Text));
                parameters.Add(new KeyValuePair<string, string>("nameStage", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("target", tbTarget.Text));
                parameters.Add(new KeyValuePair<string, string>("time", tbTime.Text));
                var desciption = StringFromRichTextBox(tbDescription);
                parameters.Add(new KeyValuePair<string, string>("des", desciption));
                parameters.Add(new KeyValuePair<string, string>("recruitmentId", id));

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
                    MessageBox.Show("Thêm mới giai đoạn thành công!");
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại, vui lòng thử lại.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private bool ValidateForm()
        {
            if(tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên giai đoạn không được để trống!");
                return false;
            }

            if (tbJob.Text.Trim() == "")
            {
                MessageBox.Show("Bộ phận đảm nhiệm không được để trống!");
                return false;
            }

            if (tbTarget.Text.Trim() == "")
            {
                MessageBox.Show("Mục tiêu không được để trống!");
                return false;
            }

            return true;
        }
    }
}
