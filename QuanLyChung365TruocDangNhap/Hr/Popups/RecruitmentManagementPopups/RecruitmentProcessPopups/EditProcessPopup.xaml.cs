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
    /// Interaction logic for EditProcessPopup.xaml
    /// </summary>
    public partial class EditProcessPopup : UserControl
    {
        string token;
        string id;
        string name;
        string _object;

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public EditProcessPopup(string token,string id, string name, string _object)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.name = name;
            this._object = _object;
            tbName.Text = this.name;
            tbObject.Text = this._object;

        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void EditProcess(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
            {
                EditProcess();
            }
        }

        private async void EditProcess()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.edit_recruitment;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("recruitId", id));
                parameters.Add(new KeyValuePair<string, string>("nameProcess", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("applyFor", tbObject.Text));

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
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại.");
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
                MessageBox.Show("Tên quy trình không được để trống!");
                return false;
            }

            if (tbObject.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng áp dụng không được để trống!");
                return false;
            }

            return true;
        }
    }
}
