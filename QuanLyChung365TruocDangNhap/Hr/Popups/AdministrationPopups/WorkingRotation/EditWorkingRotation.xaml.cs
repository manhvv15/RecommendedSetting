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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.WorkingRotation
{
    /// <summary>
    /// Interaction logic for EditWorkingRotation.xaml
    /// </summary>
    public partial class EditWorkingRotation : UserControl
    {
        string token;
        Items dataEntity;
        const int COUNT_RECORD_PER_PAGE = 1000;

        Dictionary<string, string> listEmployee = new Dictionary<string, string>();
        Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        Dictionary<string, string> listCom = new Dictionary<string, string>();

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public EditWorkingRotation(string token, Items dataEntity)
        {
            InitializeComponent();
            this.token = token;
            //this.id = id;
            this.dataEntity = dataEntity;
            GetDatalistCom();
            GetDatalistEmployee();
            SetUpCombobox();
            GetDatalistDepartment(1, 1);
            GetDatalistDepartment(2, 1);
            BindingData();

        }

        private void SetUpCombobox()
        {
            cbxChonQD.ItemsSource = ListItemCombobox.ListRegulations;
            cbxChucvuHT.ItemsSource = ListItemCombobox.ListPositionApply;
            cbxQHBN.ItemsSource = ListItemCombobox.ListPositionApply;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        //Thêm mới

        private void EditWorking(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                EditData();
        }
        private async void EditData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.add_working;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("ep_id", cbxTenNV.SelectedValue.ToString()));
                if(cbxDonViCongTacHT.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("com_id", cbxDonViCongTacHT.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("com_id", ""));
                }
                
                parameters.Add(new KeyValuePair<string, string>("new_com_id", cbxDonViCongTacMoi.SelectedValue.ToString()));
                if(cbxChucvuHT.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("current_position", cbxChucvuHT.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("current_position", ""));
                }

                if(cbxPhongBanHT.SelectedIndex > 0)
                {
                    parameters.Add(new KeyValuePair<string, string>("current_dep_id", cbxPhongBanHT.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("current_dep_id", ""));
                }
                
                parameters.Add(new KeyValuePair<string, string>("update_position", cbxQHBN.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("update_dep_id", cbxPhongBanMoi.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("created_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                parameters.Add(new KeyValuePair<string, string>("decision_id", cbxChonQD.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("mission", StringFromRichTextBox(tbNVmoi)));
                parameters.Add(new KeyValuePair<string, string>("note", StringFromRichTextBox(tbGhiChu)));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                Entities.AdministrationEntity.PersonnelChangeEntity.EntitySuccessMessage result = JsonConvert.DeserializeObject<Entities.AdministrationEntity.PersonnelChangeEntity.EntitySuccessMessage>(responseContent);
                if (result.data != null && result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhật thành công");
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
        //lấy toàn bộ danh sách nhân viên 
        private async void GetDatalistEmployee()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";

            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("com_id", dataEntity.com_id);
            form.Add("pageNumber", "1");
            form.Add("pageSize", "10000");

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            httpRequestMessage.Content = new FormUrlEncodedContent(form);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
            try
            {
                listEmployee = new Dictionary<string, string>();
                foreach (var item in result.data.items)
                {
                    listEmployee.Add(item.ep_id, item.ep_name);
                }
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee;
                cbxTenNV.SelectedItem = listEmployee.FirstOrDefault(t => t.Key == dataEntity.ep_id);
            }
            catch
            {

            }

        }
        //Lấy toàn bộ danh sách phòng ban
        // mode: 1:binding, 2: ko binding
        private async void GetDatalistDepartment(int index, int mode)
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = "";
            var form = new Dictionary<string, string>();
            if(mode == 1)
            {
                if (index == 1)
                {
                    api = APIs.API.listDepartment;
                    form.Add("com_id", dataEntity.old_com_id);
                }
                else
                {
                    api = APIs.API.listDepartment;
                    form.Add("com_id", dataEntity.com_id);
                }
            }
            else
            {
                if (index == 1)
                {
                    api = APIs.API.listDepartment;
                    form.Add("com_id", cbxDonViCongTacHT.SelectedValue.ToString());
                }
                else
                {
                    api = APIs.API.listDepartment;
                    form.Add("com_id", cbxDonViCongTacMoi.SelectedValue.ToString());
                }
            }
            

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            httpRequestMessage.Content = new FormUrlEncodedContent(form);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);
            try
            {
                listDepartment = new Dictionary<string, string>();
                listDepartment.Add("", "Phòng ban");
                foreach (var item in result.data.items)
                {
                    listDepartment.Add(item.dep_id, item.dep_name);
                }
                if (index == 1)
                {
                    cbxPhongBanHT.Items.Refresh();
                    cbxPhongBanHT.ItemsSource = listDepartment;
                    if(mode == 1) cbxPhongBanHT.SelectedItem = listDepartment.FirstOrDefault(t => t.Key == dataEntity.old_dep_id);
                }
                else
                {
                    cbxPhongBanMoi.Items.Refresh();
                    cbxPhongBanMoi.ItemsSource = listDepartment;
                    if(mode == 1) cbxPhongBanMoi.SelectedItem = listDepartment.FirstOrDefault(t => t.Key == dataEntity.dep_id);
                }
            }
            catch
            {

            }

        }
        //lấy toàn bộ danh sách công ti
        private async void GetDatalistCom()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.listCompany;

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            listCompany result = JsonConvert.DeserializeObject<listCompany>(responseContent);
            try
            {
                listCom = new Dictionary<string, string>();
                listCom.Add("", "Chọn chi nhánh");
                foreach (var item in result.data.items)
                {
                    listCom.Add(item.com_id, item.com_name);
                }
                cbxDonViCongTacHT.ItemsSource = listCom;
                cbxDonViCongTacMoi.ItemsSource = listCom;
                cbxDonViCongTacHT.SelectedItem = listCom.FirstOrDefault(t => t.Key == dataEntity.old_com_id);
                cbxDonViCongTacMoi.SelectedItem = listCom.FirstOrDefault(t => t.Key == dataEntity.com_id);
            }
            catch
            {

            }
        }

        private void BindingData()
        {
            cbxChucvuHT.SelectedItem = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == dataEntity.old_position_id);
            cbxQHBN.SelectedItem = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == dataEntity.position_id);
            cbxChonQD.SelectedItem = ListItemCombobox.ListRegulations.FirstOrDefault(t => t.ID == dataEntity.decision_id);
            tbNVmoi.Selection.Text = HtmlToPlainText(dataEntity.mission);
            tbGhiChu.Selection.Text = HtmlToPlainText(dataEntity.note);
            DTdate.Text = ConvertDate(dataEntity.created_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(dataEntity.created_at));
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );
            return textRange.Text;
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

        private void SelectionCom1Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDonViCongTacHT.SelectedIndex > 0)
            {
                GetDatalistDepartment(1, 2);
                GetDatalistEmployee();
            }
        }

        private void SelectionCom2Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDonViCongTacMoi.SelectedIndex > 0)
            {
                GetDatalistDepartment(2, 2);
            }
        }

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
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

        private bool ValidateForm()
        {
            if (cbxDonViCongTacMoi.SelectedIndex < 1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị công tác mới.");
                return false;
            }

            if (cbxPhongBanMoi.SelectedIndex < 1)
            {
                MessageBox.Show("Vui lòng chọn phòng ban mới.");
                return false;
            }

            if (cbxQHBN.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn chức vụ mới.");
                return false;
            }

            if (DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời gian luân chuyển");
                return false;
            }

            if (StringFromRichTextBox(tbNVmoi).Trim() == "")
            {
                MessageBox.Show("Nhiệm vụ công việc mới không được để trống.");
                return false;
            }
            if(cbxChonQD.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn quy định!");
                return false;
            }
            return true;
        }
        //search cbc đơn vị công tác hiện tại
        private void cbxDonViCongTacMoi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxDonViCongTacMoi.SelectedIndex = -1;
                string textSearch = cbxTenNV.Text;
                cbxDonViCongTacMoi.Items.Refresh();
                cbxDonViCongTacMoi.ItemsSource = listCom.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
        private void cbxDonViCongTacMoi_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxDonViCongTacMoi.SelectedIndex = -1;
            string textSearch = cbxDonViCongTacMoi.Text + e.Text;
            cbxDonViCongTacMoi.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxDonViCongTacMoi.Text = "";
                cbxDonViCongTacMoi.Items.Refresh();
                cbxDonViCongTacMoi.ItemsSource = listCom;
                cbxDonViCongTacMoi.SelectedIndex = -1;
            }
            else
            {
                cbxDonViCongTacMoi.ItemsSource = "";
                cbxDonViCongTacMoi.Items.Refresh();
                cbxDonViCongTacMoi.ItemsSource = listCom.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxPhongBanMoi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxPhongBanMoi.SelectedIndex = -1;
                string textSearch = cbxPhongBanMoi.Text;
                cbxPhongBanMoi.Items.Refresh();
                cbxPhongBanMoi.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxPhongBanMoi_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxPhongBanMoi.SelectedIndex = -1;
            string textSearch = cbxPhongBanMoi.Text + e.Text;
            cbxPhongBanMoi.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxPhongBanMoi.Text = "";
                cbxPhongBanMoi.Items.Refresh();
                cbxPhongBanMoi.ItemsSource = listDepartment;
                cbxPhongBanMoi.SelectedIndex = -1;
            }
            else
            {
                cbxPhongBanMoi.ItemsSource = "";
                cbxPhongBanMoi.Items.Refresh();
                cbxPhongBanMoi.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
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
    }
}
