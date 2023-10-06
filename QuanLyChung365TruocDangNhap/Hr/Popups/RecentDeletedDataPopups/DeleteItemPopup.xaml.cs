using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecentDeletedDataPopups
{
    /// <summary>
    /// Interaction logic for DeleteItemPopup.xaml
    /// </summary>
    public partial class DeleteItemPopup : UserControl
    {
        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;

        string token;
        string list_recruitment = "";
        string list_recruitment_new = "";
        string list_job_desc = "";
        string list_training_process = "";
        string list_provision = "";
        string list_policy = "";
        public DeleteItemPopup(string token, string list_recruitment, string list_recruitment_new, string list_job_desc, string list_training_process, string list_provision, string list_policy)
        {
            InitializeComponent();
            this.token = token;
            this.list_recruitment = list_recruitment;
            this.list_recruitment_new = list_recruitment_new;
            this.list_job_desc = list_job_desc;
            this.list_training_process = list_training_process;
            this.list_provision = list_provision;
            this.list_policy = list_policy;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void ClickDelete(object sender, MouseButtonEventArgs e)
        {
            DeleteData();
        }

        private async void DeleteData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.deleteRecentData;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("list_recuitment", list_recruitment));
                parameters.Add(new KeyValuePair<string, string>("list_recuitment_new", list_recruitment_new));
                parameters.Add(new KeyValuePair<string, string>("list_job_desc", list_job_desc));
                parameters.Add(new KeyValuePair<string, string>("list_training_process", list_training_process));
                parameters.Add(new KeyValuePair<string, string>("list_provision", list_provision));
                parameters.Add(new KeyValuePair<string, string>("list_employe_policy", list_policy));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Xóa thành công !");
                }
                else
                {
                    MessageBox.Show("Xóa thất bại, vui lòng thử lại!");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
