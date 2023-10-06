using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.TranningAndDeveloper
{
    /// <summary>
    /// Interaction logic for DeleteProcess.xaml
    /// </summary>
    public partial class DeleteProcess : UserControl
    {
        string token;
        string name;
        string id;
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public DeleteProcess(string token, string id, string name)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.name = name;
            txtName.Text = name + "?";
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void RemoveProcess(object sender, MouseButtonEventArgs e)
        {
            DeleteDataProcess();
        }
        private async void DeleteDataProcess()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.deleteTrainingProcess;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("processTrainId", id));

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
                    MessageBox.Show("Xóa thành công!");
                }
                //else
                //{
                //    MessageBox.Show("Xóa thất bại, vui lòng thử lại!");
                //}

            }
            catch
            {
                //MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
