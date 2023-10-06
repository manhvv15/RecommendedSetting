using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Regulation
{
    /// <summary>
    /// Interaction logic for DetailRegulation.xaml
    /// </summary>
    public partial class DetailRegulation : UserControl
    {
        string token;
        string id;
        string file_path;
        DataEntity dataEntity;

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public DetailRegulation(string token, string id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            GetData();
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.detail_provison;


                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id));
                api = api + "?id=" + id;
                httpRequestMessage.RequestUri = new Uri(api);

                /*var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;*/

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                DetailProvisionEntity result = JsonConvert.DeserializeObject<DetailProvisionEntity>(responseContent);

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
            //tbName.Text = dataEntity.name;
            tbName2.Text = dataEntity.name;
            tbSupervisor.Text = dataEntity.supervisor_name;
            tbTimeStart.Text = dataEntity.time_start;
            tbDescription.Text = HtmlToPlainText(dataEntity.description);
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
    }
}
