using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages;
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
    /// Interaction logic for MoreCandidatePopup.xaml
    /// </summary>
    public partial class MoreCandidatePopup : UserControl
    {
        string token;
        List<Items> listItem = new List<Items>();

        // deligate event hide popups
        public delegate void HidePopup(int mode, int id_process);
        public static event HidePopup hidePopup;

        public MoreCandidatePopup(string token, List<Items> listItem)
        {
            InitializeComponent();
            this.token = token;
            this.listItem = listItem;
            cbx.ItemsSource = this.listItem;
        }

        private void ShowCombobox(object sender, MouseButtonEventArgs e)
        {

        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0, 1);
        }

        private void AddProcess(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
            {
                AddProcess();
            }
        }

        private async void AddProcess()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.add_process_interview;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("name", tbNameProcess.Text));
                string id_process_before = ((Items)cbx.SelectedItem).ID;
                parameters.Add(new KeyValuePair<string, string>("processBefore", id_process_before));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (result.data != null && result.data.result)
                {
                    hidePopup(1, 1);
                    MessageBox.Show(result.success.message);
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private bool ValidateForm()
        {
            if (tbNameProcess.Text.Trim() == "")
            {
                MessageBox.Show("Tên giai đoạn không được để trống.");
                return false;
            }

            if (cbx.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giai đoạn đứng trước");
                return false;
            }

            return true;
        }
    }
       
}
