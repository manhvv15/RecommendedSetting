using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
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
using static System.Windows.Forms.AxHost;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.BreakTheRules
{
    /// <summary>
    /// Interaction logic for UpdatePopup.xaml
    /// </summary>
    public partial class UpdatePopup : UserControl
    {
        string id_com;
        string token;
        string ep_id;
        string ep_name;
        string dep_name;
        string position_name;
        string created_at;
        string note;
        string dep_id;
        string position_id;

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        public UpdatePopup(string token, string id_com, string ep_id, string ep_name, string dep_name, string position_name, string created_at, string note, string dep_id, string position_id)
        {
            InitializeComponent();
            this.token = token;
            this.id_com = id_com;
            this.ep_id = ep_id;
            this.ep_name = ep_name;
            this.dep_name = dep_name;
            this.position_name = position_name;
            this.created_at = created_at;
            this.note = note;
            this.dep_id = dep_id;
            this.position_id = position_id;

            Binding();
        }

        private void Binding()
        {
            try
            {
                txtName.Text = ep_name;
                txtDep.Text = dep_name;
                txtPosition.Text = position_name;
                if(created_at != null)
                {
                    DTdate.SelectedDate = DateTime.Parse(created_at);
                    DTdate.Text = created_at;
                }
                rtbGhiChu.Selection.Text = HtmlToPlainText(note);
            }
            catch(Exception ex) { }
        }
        private static string HtmlToPlainText(string html)
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
            text = text.Replace("</br>", "\n");
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        private string ConvertDate(string date)
        {
            try
            {
                DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("MM/dd/yyyy");
            }
            catch
            {
                try
                {
                    DateTime myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString("MM/dd/yyyy");
                }
                catch
                {
                    return "";
                }
            }
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void ClickUpdate(object sender, MouseButtonEventArgs e)
        {
            UpdateData();
        }
        private async void UpdateData()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3006/api/hr/personalChange/updateQuitJobNew");
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            var parameters = new Dictionary<string, string>();
            parameters.Add("ep_id", ep_id);
            if(position_id != null)
                parameters.Add("current_position", position_id);
            if(dep_id != null)
                parameters.Add("current_dep_id", dep_id);
            parameters.Add("created_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
            string note = StringFromRichTextBox(rtbGhiChu);
            parameters.Add("note", note);

            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();
            EntitySuccessMessage result = JsonConvert.DeserializeObject<EntitySuccessMessage>(responseContent);
            if (result.data != null && result.data.result)
            {
                hidePopup(1);
                MessageBox.Show("Sửa thành công");
            }
            else
            {
                MessageBox.Show(result.error.message);
            }
        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }
        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}

