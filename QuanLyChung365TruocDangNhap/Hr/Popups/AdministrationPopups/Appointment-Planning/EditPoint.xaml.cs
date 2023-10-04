using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Appointment_Planning
{
    public class cboxValue1
    {
        public cboxValue1(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for EditPoint.xaml
    /// </summary>
    public partial class EditPoint : UserControl
    {
        string token;
        string id;
        string ep_id;
        string old_position_id;
        string old_dep_id;
        //string update_position;

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        public EditPoint(string token, string ep_name, string ep_id, string old_position_id, string old_position_name, string old_dep_id, string old_dep_name, string position_id, string update_dep_id, string created_at, string decision_id, string note, string id, Dictionary<string, string> listDepartment)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.ep_id = ep_id;
            this.old_position_id = old_position_id;
            this.old_dep_id = old_dep_id;
            txtTenNV.Text = ep_name;
            txtChucVuHT.Text = old_position_name;
            this.listDepartment = listDepartment;
            cbxTenPhongBanMoi.ItemsSource = listDepartment;
            txtPhongBan.Text = old_dep_name;
            DTdate.SelectedDate = DateTime.Parse(created_at);
            DTdate.Text = created_at;
            if(!string.IsNullOrEmpty(note))
                rtbLyDo.Selection.Text = HtmlToPlainText(note);
            //cbxPhongBanMoi.SelectedItem = listDepartment.FirstOrDefault(t => t.Value == dep_id);
            SetUpCombobox();
            cbxTenPhongBanMoi.SelectedItem = listDepartment.FirstOrDefault(t => t.Key == update_dep_id);
            cbxQHBN.SelectedItem = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == old_position_id);
            cbxChonQuyDinh.SelectedItem = ListItemCombobox.ListRegulations.FirstOrDefault(t => t.ID == decision_id);

            //GetDatalistDepartment();
        }
        private void SetUpCombobox()
        {
            cbxQHBN.ItemsSource = ListItemCombobox.ListPositionApply;
            cbxChonQuyDinh.ItemsSource = ListItemCombobox.ListRegulations;

        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void UpdateNewDataPoint(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                UpdateData();
        }
        private async void UpdateData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.update_employe;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization","Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("ep_id", ep_id));

                parameters.Add(new KeyValuePair<string, string>("current_position", old_position_id));
                parameters.Add(new KeyValuePair<string, string>("current_dep_id", old_dep_id));
                parameters.Add(new KeyValuePair<string, string>("update_position", cbxQHBN.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("update_dep_id", cbxTenPhongBanMoi.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("created_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                if (cbxChonQuyDinh.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("decision_id", cbxChonQuyDinh.SelectedValue.ToString()));

                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("decision_id", ""));
                }
                string note = StringFromRichTextBox(rtbLyDo);
                parameters.Add(new KeyValuePair<string, string>("note", note));
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                EntitySuccessMessage result = JsonConvert.DeserializeObject<EntitySuccessMessage>(responseContent);
                if (result.data != null && result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhập thành công");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại");
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
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
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

        private bool ValidateForm()
        {
            if (cbxQHBN.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chức vụ.");
                return false;
            }

            if (cbxTenPhongBanMoi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng ban.");
                return false;
            }

            if (DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền thời gian luân chuyển công tác.");
                return false;
            }

            return true;
        }
        private void cbxQHBN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxQHBN.SelectedIndex = -1;
            string textSearch = cbxQHBN.Text + e.Text;
            cbxQHBN.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxQHBN.Text = "";
                cbxQHBN.Items.Refresh();
                cbxQHBN.ItemsSource = ListItemCombobox.ListPositionApply;
                cbxQHBN.SelectedIndex = -1;
            }
            else
            {
                cbxQHBN.ItemsSource = "";
                cbxQHBN.Items.Refresh();
                cbxQHBN.ItemsSource = ListItemCombobox.ListPositionApply.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxQHBN_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxQHBN.SelectedIndex = -1;
                string textSearch = cbxQHBN.Text;
                cbxQHBN.Items.Refresh();
                cbxQHBN.ItemsSource = ListItemCombobox.ListPositionApply.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

       

        private void cbxTenPhongBan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenPhongBanMoi.SelectedIndex = -1;
                string textSearch = cbxTenPhongBanMoi.Text;
                cbxTenPhongBanMoi.Items.Refresh();
                cbxTenPhongBanMoi.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenPhongBan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenPhongBanMoi.SelectedIndex = -1;
            string textSearch = cbxTenPhongBanMoi.Text + e.Text;
            cbxTenPhongBanMoi.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenPhongBanMoi.Text = "";
                cbxTenPhongBanMoi.Items.Refresh();
                cbxTenPhongBanMoi.ItemsSource = listDepartment;
                cbxTenPhongBanMoi.SelectedIndex = -1;
            }
            else
            {
                cbxTenPhongBanMoi.ItemsSource = "";
                cbxTenPhongBanMoi.Items.Refresh();
                cbxTenPhongBanMoi.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
