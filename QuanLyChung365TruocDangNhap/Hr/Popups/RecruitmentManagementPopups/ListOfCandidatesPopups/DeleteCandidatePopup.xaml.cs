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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups
{
    /// <summary>
    /// Interaction logic for DeleteCandidatePopup.xaml
    /// </summary>
    public partial class DeleteCandidatePopup : UserControl
    {
        string token;
        string name;
        string id_candidate;
        string id_process;
        // deligate event hide popups
        public delegate void HidePopup(int mode, int id_process);
        public static event HidePopup hidePopup;

        public DeleteCandidatePopup(string token, string id_candidate, string name, string id_process)
        {
            InitializeComponent();
            this.token = token;
            this.name = name;
            this.id_candidate = id_candidate;
            this.id_process = id_process;
            tbNameProcess.Text = this.name;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0, 1);
        }

        private void DeleteProcess(object sender, MouseButtonEventArgs e)
        {
            DeleteProcess();
        }

        private async void DeleteProcess()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.delete_candidate;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("candidateId", id_candidate));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);


                if (result.data.result)
                {
                    hidePopup(1, Convert.ToInt32(id_process));
                    MessageBox.Show(result.success.message);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
                }

            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
