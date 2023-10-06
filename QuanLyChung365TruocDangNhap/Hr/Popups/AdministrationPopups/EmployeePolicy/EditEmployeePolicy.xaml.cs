using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PolicyRegulations;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.EmployeePolicy
{
    /// <summary>
    /// Interaction logic for EditEmployeePolicy.xaml
    /// </summary>
    public partial class EditEmployeePolicy : UserControl
    {
        string token;
        string file_path = "";
        string id;
        Dictionary<string, string> listAllRegulation;
        DataEntity5 dataEntity;
        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public EditEmployeePolicy(string token, string id, Dictionary<string, string> listAllRegulation)
        {
            InitializeComponent();
            this.token = token;
            this.listAllRegulation = listAllRegulation;
            cbxpolicy.ItemsSource = this.listAllRegulation;
            this.id = id;
            GetData();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.detail_employee_policy;

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
            tbSupervisor.Text = dataEntity.supervisor_name;
            dpTimeStart.Text = ConvertDate(dataEntity.time_start, "dd/MM/yyyy");
            dpTimeStart.SelectedDate = DateTime.Parse(ConvertDate(dataEntity.time_start, "dd/MM/yyyy"));
            rtbDescription.Selection.Text = HtmlToPlainText(dataEntity.content);
            tbApllyFor.Text = dataEntity.apply_for;

            cbxpolicy.SelectedItem = listAllRegulation.FirstOrDefault(t => t.Key == dataEntity.employe_policy_id);
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

        private void ChooseCV(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.Filter = "PDF Files (*.pdf)|*.pdf|XLSX Files (*.xlsx)|*.xlsx|XLS Files (*.xls)|*.xls|DOC Files (*.doc)|*.doc|DOCX Files (*.docx)|*.docx|PPT Files (*.ppt)|*.ppt|PPTX Files (*.pptx)|*.pptx|TXT Files (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.SafeFileName;
                file_path = dlg.FileName;
                tbFile.Text = filename;
                btnDeleteCV.Visibility = Visibility.Visible;
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

        private void DeleteFileCV(object sender, MouseButtonEventArgs e)
        {
            tbFile.Text = "";
            file_path = "";
            btnDeleteCV.Visibility = Visibility.Collapsed;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void EditPolicyHandler(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                EditPolicyHandler();
        }

        private async void EditPolicyHandler()
        {
            try
            {
                var client = new HttpClient();
                var multiForm = new MultipartFormDataContent();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                multiForm.Add(new StringContent(id), "id");
                multiForm.Add(new StringContent(tbName.Text), "name");
                multiForm.Add(new StringContent(cbxpolicy.SelectedValue.ToString()), "employe_policy_id");
                multiForm.Add(new StringContent(dpTimeStart.SelectedDate.Value.ToString("yyyy-MM-dd")), "time_start");
                multiForm.Add(new StringContent(tbSupervisor.Text), "supervisor_name");
                multiForm.Add(new StringContent(tbApllyFor.Text), "apply_for");
                multiForm.Add(new StringContent(StringFromRichTextBox(rtbDescription)), "content");

                //add file and directly upload it
                if (file_path != "")
                {
                    FileStream fs = File.OpenRead(file_path);
                    multiForm.Add(new StreamContent(fs), "files", Path.GetFileName(file_path));
                }

                // send request to API
                var url = APIs.API.edit_employee_policy;
                var response = await client.PutAsync(url, multiForm);
                var responseContent = await response.Content.ReadAsStringAsync();
                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show(result.error.message);
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                return "";
            }

        }

        private string ConvertDateToSend(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "d/MM/yyyy",
                                             System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    return "";
                }
            }

        }

        private bool ValidateForm()
        {
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên chính sách không được để trống!");
                return false;
            }

            if (cbxpolicy.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhóm chính sách.");
                return false;
            }

            if (dpTimeStart.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền thời gian hiệu lực.");
                return false;
            }

            if (tbSupervisor.Text.Trim() == "")
            {
                MessageBox.Show("Người giám sát không được để trống.");
                return false;
            }

            if (tbApllyFor.Text.Trim() == "")
            {
                MessageBox.Show("Đối tượng thi hành không được để trống.");
                return false;
            }

            if (StringFromRichTextBox(rtbDescription).Trim() == "")
            {
                MessageBox.Show("Nội dung chính sách không được để trống.");
                return false;
            }

            return true;
        }
        private void cbxpolicy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxpolicy.SelectedIndex = -1;
                string textSearch = cbxpolicy.Text;
                cbxpolicy.Items.Refresh();
                cbxpolicy.ItemsSource = listAllRegulation.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxpolicy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxpolicy.SelectedIndex = -1;
            string textSearch = cbxpolicy.Text + e.Text;
            cbxpolicy.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxpolicy.Text = "";
                cbxpolicy.Items.Refresh();
                cbxpolicy.ItemsSource = listAllRegulation;
                cbxpolicy.SelectedIndex = -1;
            }
            else
            {
                cbxpolicy.ItemsSource = "";
                cbxpolicy.Items.Refresh();
                cbxpolicy.ItemsSource = listAllRegulation.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
