using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Regulation.Policy
{
    /// <summary>
    /// Interaction logic for DetailPolicy.xaml
    /// </summary>
    public partial class DetailPolicy : UserControl
    {
        string token;
        string id;
        string file_path;
        int TypeUser;
        DataEntity5 dataEntity;
        LoginCompanyEntity loginCompanyEntity;

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public DetailPolicy(string token, string id, int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            GetData();

            if (TypeUser == 2)
            {
                txtdownloadCom.Visibility = Visibility.Visible;
            }
            else
            {
                txtdownloadCom.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.detail_policy;


                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id));
                api = api + "?id=" + id;
                httpRequestMessage.RequestUri = new Uri(api);

                /*var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;*/

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                DetailPolicyEntity result = JsonConvert.DeserializeObject<DetailPolicyEntity>(responseContent);

                if (result.result && result.success != null)
                {
                    dataEntity = result.success.data.data;
                    BindingData();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("error");
            }
        }

        private void BindingData()
        {
            tbName.Text = dataEntity.name;
            tbName2.Text = dataEntity.qd_name;
            tbSupervisor.Text = dataEntity.supervisor_name;
            tbTimeStart.Text = dataEntity.time_start;
            tbDescription.Text = HtmlToPlainText(dataEntity.content);
            tbApllyFor.Text = dataEntity.apply_for;
            tbCreatedBy.Text = dataEntity.created_by;
            if (dataEntity.file != null && dataEntity.file != "")
            {
                file_path = dataEntity.file;
                tbFile.Visibility = Visibility.Visible;
            }
            else
            {
                tbFile.Visibility = Visibility.Collapsed;
            }
        }

        private string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void PreviewFile(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo(APIs.API.preview_file_policy + file_path));
            e.Handled = true;
        }

        private void downFileDoc(object sender, MouseButtonEventArgs e)
        {

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            dlg.FileName = file_path;
            if (dlg.ShowDialog() == true)
                File.WriteAllText(dlg.FileName, file_path);

        }
    }
}
