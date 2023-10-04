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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.PerformRecruitmentPopup
{
    /// <summary>
    /// Interaction logic for SetupTemplatePopup.xaml
    /// </summary>
    public partial class SetupTemplatePopup : UserControl
    {
        string token;
        string id;

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public SetupTemplatePopup(string token, string id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void ClickSetSampleNew(object sender, MouseButtonEventArgs e)
        {
            SetSampleNew();
        }

        private async void SetSampleNew()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.setSampleNew;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("newsId", id));
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    hidePopup(0);
                    MessageBox.Show("Thiết lập thành công!");
                }
                else
                {
                    MessageBox.Show("Thiết lập thất bại, vui lòng thử lại.");
                }

            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
