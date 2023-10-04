using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddProcedure.xaml
    /// </summary>
    public partial class AddProcedure : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        string token;
        public AddProcedure(string token)
        {
            this.token = token;
            InitializeComponent();
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void AddProcedures(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                AddNewDataProcedure();
        }
        private async void AddNewDataProcedure()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.addTrainingProcess;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("name", tbDoiTuongDT.Text));
                parameters.Add(new KeyValuePair<string, string>("description", StringFromRichTextBox(tbMoTaNgan)));


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
                    MessageBox.Show("Thêm mới quy trình đào tạo thành công!");
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("Hihi");
            }
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );
            string replacement = Regex.Replace(textRange.Text, @"\t|\n|\r", " ");
            return replacement;
        }

        private bool ValidateForm()
        {
            if (tbTenQuyTrinh.Text.Trim() == "")
            {
                MessageBox.Show("Tên quy trình không được để trống.");
                return false;
            }

            if (tbDoiTuongDT.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng đào tạo không được để trống.");
                return false;
            }

            if (StringFromRichTextBox(tbMoTaNgan).Trim() == "")
            {
                MessageBox.Show("Mô tả không được để trống.");
                return false;
            }

            return true;
        }
    }
}
