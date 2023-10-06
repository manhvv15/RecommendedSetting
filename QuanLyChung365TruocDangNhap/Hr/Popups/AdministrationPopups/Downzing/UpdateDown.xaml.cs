using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.Downzing
{
    /// <summary>
    /// Interaction logic for UpdateDown.xaml
    /// </summary>
    public partial class UpdateDown : UserControl
    {
        Items1 dataEn;

        public string token;
        public string id;
        public string ep_id;
        public string old_position_id;
        public string com_id;
        public string dep_id;

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public Dictionary<string, string> listShift = new Dictionary<string, string>();
        public UpdateDown(string token, string id, string ep_id, string ep_name, string old_position_id, string old_position_name, string created_at, string com_id, string com_name, string dep_id, string dep_name, string decision_id, string note, string type, string shift_id, Dictionary<string, string> listShift, Items1 dataEn)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.ep_id = ep_id;
            this.old_position_id = old_position_id;
            this.com_id = com_id;
            this.dep_id = dep_id;
            this.listShift = listShift;
            this.dataEn = dataEn;
            cbxChonCaNghi.ItemsSource = listShift;
            //GetDataListShift();

            txtTenNV.Text = ep_name;
            txtChucVuHT.Text = old_position_name;
            txtDonViCTHT.Text = com_name;
            txtPhongBan.Text = dep_name;
            rtbLyDo.Selection.Text = HtmlToPlainText(dataEn.note);
            //DTdate.Text = ConvertTicksToDate(created_at);
            DTdate.Text = ConvertDate(created_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(created_at));
            SetUpCombobox();
            cbxChonCaNghi.SelectedItem = listShift.FirstOrDefault(t => t.Key == shift_id);
            cbxChonQuyDinh.SelectedItem = ListItemCombobox.ListRegulations.FirstOrDefault(t => t.ID == decision_id);
            cbxHinhThuc.SelectedItem = ListItemCombobox.ListHinhThuc.FirstOrDefault(t => t.ID == type);

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;

        }
     
        private void SetUpCombobox()
        {
            cbxChonQuyDinh.ItemsSource = ListItemCombobox.ListRegulations;
            cbxHinhThuc.ItemsSource = ListItemCombobox.ListHinhThuc;
        }
        private string ConvertDate(string date)
        {
            try
            {
                DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                try
                {
                    DateTime myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString("dd/MM/yyyy");
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
        //update giảm biên chế,nghỉ việc
        private void UpdateDataDown(object sender, MouseButtonEventArgs e)
        {
            UpdateData();
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
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
        private async void UpdateData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.updateDownzing;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("ep_id", ep_id));
                parameters.Add(new KeyValuePair<string, string>("com_id", com_id));
                parameters.Add(new KeyValuePair<string, string>("current_position", old_position_id));
                parameters.Add(new KeyValuePair<string, string>("current_dep_id", dep_id));


                parameters.Add(new KeyValuePair<string, string>("created_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                if(cbxChonQuyDinh.SelectedValue != null)
                parameters.Add(new KeyValuePair<string, string>("decision_id", cbxChonQuyDinh.SelectedValue.ToString()));

                parameters.Add(new KeyValuePair<string, string>("note", StringFromRichTextBox(rtbLyDo)));
                parameters.Add(new KeyValuePair<string, string>("type", cbxHinhThuc.SelectedValue.ToString()));
                if (cbxChonCaNghi.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("shift_id", cbxChonCaNghi.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("shift_id", ""));
                }
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
                    MessageBox.Show("Cập nhật thành công");
                }
                else
                {
                    MessageBox.Show(result.error.message);
                }
            }
            catch (Exception)
            {

            }
        }
        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        private string ConvertTicksToDate(string ticks)
        {
            try
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(ticks)).ToLocalTime();
                return dtDateTime.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }
        }
        private void cbxChonCaNghi_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxChonCaNghi.SelectedIndex = -1;
            string textSearch = cbxChonCaNghi.Text + e.Text;
            cbxChonCaNghi.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxChonCaNghi.Text = "";
                cbxChonCaNghi.Items.Refresh();
                cbxChonCaNghi.ItemsSource = listShift;
                cbxChonCaNghi.SelectedIndex = -1;
            }
            else
            {
                cbxChonCaNghi.ItemsSource = "";
                cbxChonCaNghi.Items.Refresh();
                cbxChonCaNghi.ItemsSource = listShift.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxChonCaNghi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxChonCaNghi.SelectedIndex = -1;
                string textSearch = cbxChonCaNghi.Text;
                cbxChonCaNghi.Items.Refresh();
                cbxChonCaNghi.ItemsSource = listShift.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
