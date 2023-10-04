using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.EmployeePolicy
{
    /// <summary>
    /// Interaction logic for DeleteEmployeePolicyGroup.xaml
    /// </summary>
    public partial class DeleteEmployeePolicyGroup : UserControl
    {
        string token;
        string id;

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public DeleteEmployeePolicyGroup(string token, string name, string id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            tbName.Text = name;
            this.id = id;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void DeleteRegulation(object sender, MouseButtonEventArgs e)
        {
            DeleteRegulation();
        }

        private async void DeleteRegulation()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Delete;
                string api = APIs.API.delete_employee_policy;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Xóa thành công!");
                }
                else
                {
                    MessageBox.Show("Xóa thất bại, vui lòng thử lại!");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}
