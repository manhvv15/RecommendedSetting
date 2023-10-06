using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
    /// Interaction logic for AddNewDown.xaml
    /// </summary>
    public partial class AddNewDown : UserControl
    {
        public string token;
        public string id;
        int TypeUser;


        const int COUNT_RECORD_PER_PAGE = 1000;
        int page_now = 1;

        public static Dictionary<string, string> listShift = new Dictionary<string, string>();
        List<Items3> listEmployee = new List<Items3>();

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public AddNewDown(string token, string id, int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            SetUpCombobox();
            GetDatalistEmployee();
            GetDataListShift();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        private void SetUpCombobox()
        {
            cbxChonQuyDinh.ItemsSource = ListItemCombobox.ListRegulations;
            cbxHinhThuc.ItemsSource = ListItemCombobox.ListHinhThuc;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        //lấy toàn bộ danh sách nhân viên 
        private async void GetDatalistEmployee()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";

            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("com_id", id.ToString());
            form.Add("pageNumber", page_now.ToString());
            form.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            httpRequestMessage.Content = new FormUrlEncodedContent(form);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
            try
            {
                listEmployee = result.data.items;
                foreach (var item in listEmployee)
                {
                    string dep_name = " (Chưa cập nhật)";
                    if (item.dep_name != null && item.dep_name != "")
                    {
                        dep_name = " (" + item.dep_name + ")";
                    }
                    item.ep_name = item.ep_name + dep_name;
                }
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee;
            }
            catch
            {

            }

        }
        //lấy toàn bộ danh sách ca 
        private async void GetDataListShift()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.list_shift + id;
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                listShift result = JsonConvert.DeserializeObject<listShift>(responseContent);
                if (result.data == null)
                    return;
                string value = result.data.items[0].start_time;
                string name = result.data.items[0].shift_name;
                foreach (var item in result.data.items)
                {
                    if (!listShift.ContainsKey(item.shift_id)) listShift.Add(item.shift_id, item.shift_name);
                    if (DateTime.Parse(item.start_time) < DateTime.Parse(value))
                    {
                        value = item.start_time;
                        name = item.shift_name;
                    }
                }
                cbxChonCaNghi.Items.Refresh();
                cbxChonCaNghi.ItemsSource = listShift;
                cbxChonCaNghi.SelectedValue = name;
                /*var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.list_shift + id;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", token);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();*/

            }
            catch (Exception e)
            {
                
                MessageBox.Show(e.ToString());
            }
        }
        private void AddNewDataDown(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                GetDatAddNew();

        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }
        private async void GetDatAddNew()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.addDownzing;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();

                //int indexSelection = cbxTenNV.SelectedIndex;
                //List<Items3> listEmployee = (List<Items3>)cbxTenNV.ItemsSource;
                //Items3 items3 = listEmployee[indexSelection];
                Items3 items3 = (Items3)cbxTenNV.SelectedItem;

                parameters.Add(new KeyValuePair<string, string>("ep_id", cbxTenNV.SelectedValue.ToString()));
                if (cbxChonCaNghi.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("shift_id", cbxChonCaNghi.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("shift_id", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("com_id", items3.com_id));
                parameters.Add(new KeyValuePair<string, string>("current_position", items3.position_id));
                parameters.Add(new KeyValuePair<string, string>("current_dep_id", items3.dep_id));
                string date = DTdate.SelectedDate.Value.ToString("yyyy-MM-dd");
                parameters.Add(new KeyValuePair<string, string>("created_at", date));
                parameters.Add(new KeyValuePair<string, string>("note", StringFromRichTextBox(rtbLyDo)));
                if(cbxChonQuyDinh.SelectedValue != null)
                {
                    parameters.Add(new KeyValuePair<string, string>("decision_id", cbxChonQuyDinh.SelectedValue.ToString()));
                }
                parameters.Add(new KeyValuePair<string, string>("type", cbxHinhThuc.SelectedValue.ToString()));
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                EntitySuccessMessage result = JsonConvert.DeserializeObject<EntitySuccessMessage>(responseContent);
                if (result.data != null && result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm thành công");
                    NotificationPersonnelChangeAppChat();
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
        public T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
        int flag =0;
        private void cbxTenNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int indexSelection = cbxTenNV.SelectedIndex;
            //List<Items3> listEmployee = (List<Items3>)cbxTenNV.ItemsSource;
            //Items3 items3 = listEmployee[indexSelection];
            Items3 items3 = (Items3)cbxTenNV.SelectedItem;
            if (cbxTenNV.SelectedIndex == -1)
            {
                txtPhongBan.Text = "";
                txtChucVuHT.Text = "";
                txtDonViCTHT.Text = "";
            }
            else if (items3.dep_name == null)
            {
                items3.dep_name = "Chưa cập nhật";
                txtPhongBan.Text = items3.dep_name;
                if(flag == 0)
                    cbxChonCaNghi.SelectedIndex = 2;
                flag = 1;
            }
            else
            {
                txtPhongBan.Text = items3.dep_name;
                txtChucVuHT.Text = items3.position_name;
                txtDonViCTHT.Text = items3.com_name;
                if(flag ==0)
                    cbxChonCaNghi.SelectedIndex = 2;
                flag = 1;
            }
            ComboBox cb = sender as ComboBox;
            cb.IsEditable = false;
            cbxChonCaNghi.ForceCursor = true;
            cb.IsEditable = true;
            TextBox txt = GetFirstChildOfType<TextBox>(cbxTenNV);
            if (txt != null)
            {
                txt.SelectionStart = 0;
                txt.SelectionLength = 0;
            }
        }
        private bool ValidateForm()
        {

            if(DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu nghỉ!");
                return false;
            }

            if (cbxTenNV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tên nhân viên");
                return false;
            }

            if (cbxHinhThuc.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn hình thức");
                return false;
            }
            return true;
        }
        private void cbxTenNV_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenNV.SelectedIndex = -1;
            string textSearch = cbxTenNV.Text + e.Text;
            cbxTenNV.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenNV.Text = "";
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee;
                cbxTenNV.SelectedIndex = -1;
            }
            else
            {
                cbxTenNV.ItemsSource = "";
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenNV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenNV.SelectedIndex = -1;
                string textSearch = cbxTenNV.Text;
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
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


        private async void NotificationPersonnelChangeAppChat()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.NotificationPersonnelChange;

                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("SenderId", id));
                parameters.Add(new KeyValuePair<string, string>("EmployeeId", cbxTenNV.SelectedValue.ToString()));

                //string Status_com = "1";
                //string Status_emp = "2";
                string Status_com = TypeUser.ToString();


                if (Status_com == "1")
                {
                    parameters.Add(new KeyValuePair<string, string>("Status", Status_com));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("Status", Status_com));
                }

                parameters.Add(new KeyValuePair<string, string>("ListReceive", "[" + cbxTenNV.SelectedValue.ToString() + "]"));
                parameters.Add(new KeyValuePair<string, string>("CompanyId", id));

                //int indexSelection = cbxTenNV.SelectedIndex;
                //List<Items3> listEmployee = (List<Items3>)cbxTenNV.ItemsSource;
                //Items3 items3 = listEmployee[indexSelection];

                Items3 items3 = (Items3)cbxTenNV.SelectedItem;


                parameters.Add(new KeyValuePair<string, string>("Position", items3.position_name));
                parameters.Add(new KeyValuePair<string, string>("Department", items3.dep_name));
                parameters.Add(new KeyValuePair<string, string>("CreateAt", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));

                string type = "QuitJob";
                parameters.Add(new KeyValuePair<string, string>("Type", type));
                parameters.Add(new KeyValuePair<string, string>("NewCompanyId", id));
                parameters.Add(new KeyValuePair<string, string>("NewCompanyName", items3.com_name));
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                MessNofiChat result = JsonConvert.DeserializeObject<MessNofiChat>(responseContent);
                //if (result.data.result != null)
                //{
                //    MessageBox.Show(result.data.message);
                //}
            }
            catch { }

        }
    }
}
