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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.RecruitmentProcessPopups
{
    /// <summary>
    /// Interaction logic for ProcessEditingPopup.xaml
    /// </summary>
    public partial class ProcessEditingPopup : UserControl
    {
        string token;
        string job;
        string name;
        string target;
        string time;
        string description;
        string stage_id;

        Entities.ManageRecuitmentEntities.RecuitmentProcessEntities.Data2Entity dataEntity;
        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public ProcessEditingPopup(string token, string job, string name, string target, string time, string description, string stage_id)
        {
            InitializeComponent();
            this.token = token;
            this.name = name;
            this.job = job;
            this.target = target;
            this.time = time;
            this.description = description;
            this.stage_id = stage_id;

            BindingData();
        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        private void BindingData()
        {
            tbName.Text = name;
            tbJob.Text = job;
            tbTarget.Text = target;
            tbTime.Text = time;
            if(!string.IsNullOrEmpty(description))
                rtbDescription.Selection.Text = HtmlToPlainText(description);

        }
        private string HtmlToPlainText(string html)
        {
            var text = html;
            try
            {
                const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
                const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
                const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
                var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
                var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
                var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

                //Decode html specific characters
                text = System.Net.WebUtility.HtmlDecode(text);
                //Remove tag whitespace/line breaks
                text = tagWhiteSpaceRegex.Replace(text, "><");
                //Replace <br /> with line breaks
                text = lineBreakRegex.Replace(text, Environment.NewLine);
                //Strip formatting
                text = stripFormattingRegex.Replace(text, string.Empty);
            }
            catch { }

            return text;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void UpdateStage(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
            {
                UpdateStage();
            }
        }

      
        private async void UpdateStage()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.edit_stage;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("posAssum", tbJob.Text));
                parameters.Add(new KeyValuePair<string, string>("nameStage", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("target", tbTarget.Text));
                parameters.Add(new KeyValuePair<string, string>("time", tbTime.Text));
                var desciption = StringFromRichTextBox(rtbDescription);
                parameters.Add(new KeyValuePair<string, string>("des", desciption));
                parameters.Add(new KeyValuePair<string, string>("stageRecruitmentId", stage_id));

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
                    MessageBox.Show("Cập nhật thành công !");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xả ra, vui lòng thử lại!");
            }
        }

        private bool ValidateForm()
        {
            if(tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên giai đoạn không được để trống!");
                return false;
            }

            if (tbJob.Text.Trim() == "")
            {
                MessageBox.Show("Vị trí đảm nhận không được để trống!");
                return false;
            }

            if (tbTarget.Text.Trim() == "")
            {
                MessageBox.Show("Mục tiêu không được để trống!");
                return false;
            }

            return true;
        }
    }
}
