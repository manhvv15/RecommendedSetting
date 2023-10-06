using QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup
{
    /// <summary>
    /// Interaction logic for UpdateDetailDep.xaml
    /// </summary>
    public partial class UpdateDetailDep : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        string token;
        string dep_id;
        string dep_name;
        string total_emp; 
        string pho_to_truong;

        public UpdateDetailDep(string token,string dep_id,string dep_name,string total_emp,string pho_to_truong)
        {
            InitializeComponent();
            this.token = token;
            this.dep_id = dep_id;
            this.dep_name = dep_name;
            this.total_emp = total_emp;
            this.pho_to_truong = pho_to_truong;
            this.DataContext = this;

            txtNameDep.Text = dep_name;

            if (txtLeader.Text == "")
            {
                txtLeader.Text = "Chưa cập nhật";
            }
            else
            {

            txtLeader.Text = pho_to_truong;

            }
            txtTotalEmp.Text = total_emp;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        private void Update(object sender, MouseButtonEventArgs e)
        {
            RunUpdate();
        }

        private async void RunUpdate()
        {
            try
                {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri("https://phanmemnhansu.timviec365.vn/updateDescDepartment");

                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("token", token));
                parameters.Add(new KeyValuePair<string, string>("dep_id", dep_id));
                string desc = StringFromRichTextBox(rtbDescription);
                parameters.Add(new KeyValuePair<string, string>("desc", desc));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                updateDep result = JsonConvert.DeserializeObject<updateDep>(responseContent);
                if (result.result == true)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhập thành công");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng thử lại!");
            }
        }
    }
}
