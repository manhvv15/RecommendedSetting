using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    /// Interaction logic for AddNewWorkingRotation.xaml
    /// </summary>
    public partial class AddNewWorkingRotation : UserControl
    {
        string token;
        string id;
        int TypeUser;
        const int COUNT_RECORD_PER_PAGE = 1000;
        Dictionary<string, string> listEmployee = new Dictionary<string, string>();
        Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        Dictionary<string, string> listDepartmentNow = new Dictionary<string, string>();
        Dictionary<string, string> listCom = new Dictionary<string, string>();
        Dictionary<string, string> listEpDep = new Dictionary<string, string>();
        Dictionary<string, string> listEpPos = new Dictionary<string, string>();




        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;
        public AddNewWorkingRotation(string token, string id, int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            listEmployee.Add("", "Chọn nhân viên");
            SetUpCombobox();
            GetDatalistCom();


            //GetDatalistEmployee();
            //GetDatalistDepartment();

        }

        private void SetUpCombobox()
        {
            cbxTenNV.ItemsSource = listEmployee;
            cbxChonQD.ItemsSource = ListItemCombobox.ListRegulations;
            cbxChucvuHT.ItemsSource = ListItemCombobox.ListPositionApply;
            cbxQHBN.ItemsSource = ListItemCombobox.ListPositionApply;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        //Thêm mới

        private void AddNewWorking(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                AddNewData();
        }

        private async void AddNewData()
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
                parameters.Add(new KeyValuePair<string, string>("com_id", cbxDonViCongTacHT.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("new_com_id", cbxDonViCongTacMoi.SelectedValue.ToString()));
                if (cbxChucvuHT.SelectedValue != null)
                    parameters.Add(new KeyValuePair<string, string>("current_position", cbxChucvuHT.SelectedValue.ToString()));
                if (cbxPhongBanHT.SelectedValue != null)
                    parameters.Add(new KeyValuePair<string, string>("current_dep_id", cbxPhongBanHT.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("update_position", cbxQHBN.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("update_dep_id", cbxPhongBanMoi.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("created_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                if (cbxChonQD.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("decision_id", cbxChonQD.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("decision_id", ""));
                }
                parameters.Add(new KeyValuePair<string, string>("mission", StringFromRichTextBox(tbNVmoi)));
                parameters.Add(new KeyValuePair<string, string>("note", StringFromRichTextBox(tbGhiChu)));

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

        //lấy toàn bộ danh sách nhân viên 
        private async void GetDatalistEmployee()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;


            string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";

            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("com_id", cbxDonViCongTacHT.SelectedValue.ToString());
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
                listEmployee.Add("", "Chọn nhân viên");
                listEpDep = new Dictionary<string, string>();
                listEpPos = new Dictionary<string, string>();
                foreach (var item in result.data.items)
                {
                    string dep_name = " (Chưa cập nhật)";
                    if (item.dep_name != null && item.dep_name != "")
                    {
                        dep_name = " (" + item.dep_name + ")";
                    }
                    string name = item.ep_name + dep_name;
                    listEmployee.Add(item.ep_id, name);
                    listEpDep.Add(item.ep_id, item.dep_id);
                    listEpPos.Add(item.ep_id, item.position_id);
                }
                cbxChonQD.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee;

            }
            catch
            {

            }

        }

        //Lấy toàn bộ danh sách phòng ban
        private async void GetDatalistDepartment(int index)
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = "";
            var form = new Dictionary<string, string>();
            if (index == 1)
            {
                api = APIs.API.listDepartment;
                form.Add("com_id", cbxDonViCongTacHT.SelectedValue.ToString());
            }
            else
            {
                form.Add("com_id", cbxDonViCongTacMoi.SelectedValue.ToString());
                api = APIs.API.listDepartment;
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
                    listDepartmentNow = listDepartment;
                }
                else
                {
                    cbxPhongBanMoi.Items.Refresh();
                    cbxPhongBanMoi.ItemsSource = listDepartment;
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
            string api = "http://210.245.108.202:3000/api/qlc/company/child/list";

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
            }
            catch
            {

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

        private void SelectionCom1Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDonViCongTacHT.SelectedIndex > 0)
            {
                GetDatalistDepartment(1);
                GetDatalistEmployee();
            }
        }

        private void SelectionCom2Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDonViCongTacMoi.SelectedIndex > 0)
            {
                GetDatalistDepartment(2);
            }
        }

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
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

        private void cbxTenNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTenNV.SelectedIndex > 0)
            {
                string ep_id = cbxTenNV.SelectedValue.ToString();
                string dep_id = "";
                string posion_id = "";
                if (listEpDep.ContainsKey(ep_id))
                {
                    if (listEpDep[ep_id] == null)
                    {
                        cbxPhongBanHT.SelectedIndex = 0;
                    }
                    else
                    {
                        dep_id = listEpDep[ep_id];
                    }
                }
                else
                {
                    cbxPhongBanHT.SelectedIndex = 0;
                }
                if (listEpPos.ContainsKey(ep_id))
                {
                    posion_id = listEpPos[ep_id];
                }

                cbxPhongBanHT.SelectedItem = listDepartmentNow.FirstOrDefault(t => t.Key == dep_id);
                cbxChucvuHT.SelectedItem = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == posion_id);
                ComboBox cb = sender as ComboBox;
                cb.IsEditable = false;
                cbxDonViCongTacHT.ForceCursor = true;
                cb.IsEditable = true;
                TextBox txt = GetFirstChildOfType<TextBox>(cbxTenNV);
                if (txt != null)
                {
                    txt.SelectionStart = 0;
                    txt.SelectionLength = 0;
                }
            }
        }

        private bool ValidateForm()
        {
            if (cbxTenNV.SelectedIndex < 0 || cbxTenNV.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên");
                return false;
            }

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
            return true;
        }
        //search cbc đơn vị công tác hiện tại
        private void cbxDonViCongTacHT_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxDonViCongTacHT.SelectedIndex = -1;
            string textSearch = cbxDonViCongTacHT.Text + e.Text;
            cbxDonViCongTacHT.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxDonViCongTacHT.Text = "";
                cbxDonViCongTacHT.Items.Refresh();
                cbxDonViCongTacHT.ItemsSource = listCom;
                cbxDonViCongTacHT.SelectedIndex = -1;
            }
            else
            {
                cbxDonViCongTacHT.ItemsSource = "";
                cbxDonViCongTacHT.Items.Refresh();
                cbxDonViCongTacHT.ItemsSource = listCom.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxDonViCongTacHT_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxDonViCongTacHT.SelectedIndex = -1;
                string textSearch = cbxTenNV.Text;
                cbxDonViCongTacHT.Items.Refresh();
                cbxDonViCongTacHT.ItemsSource = listCom.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }


        //search cbc nhân viên

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
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenNV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenNV.SelectedIndex = -1;
                string textSearch = cbxTenNV.Text;
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
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

        //thông báo luân chuyển
        //public string statusName_id { get; set; }
        //public string statusName
        //{
        //    get
        //    {
        //        return GetNameStatus(statusName_id);
        //    }
        //}
        //private string GetNameStatus(string statusName_id)
        //{
        //    switch (statusName_id)
        //    {
        //        case "1":
        //            return "Công ty";
        //        case "2":
        //            return "Nhân viên";
        //        default:
        //            return "";
        //    }

        //}
        private async void NotificationPersonnelChangeAppChat()
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
            parameters.Add(new KeyValuePair<string, string>("Position", cbxQHBN.Text));
            parameters.Add(new KeyValuePair<string, string>("Department", cbxPhongBanHT.Text));
            parameters.Add(new KeyValuePair<string, string>("CreateAt", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));

            string type = "Transfer";
            if (type == "Transfer")
            {
                parameters.Add(new KeyValuePair<string, string>("Type", type));
            }
            parameters.Add(new KeyValuePair<string, string>("NewCompanyId", cbxDonViCongTacMoi.SelectedValue.ToString()));
            parameters.Add(new KeyValuePair<string, string>("NewCompanyName", cbxDonViCongTacMoi.Text));
            // Thiết lập Content
            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();
            //MessNofiChat result = JsonConvert.DeserializeObject<MessNofiChat>(responseContent);
            //if (result.data.result != null)
            //{
            //    MessageBox.Show(result.data.message);
            //}
        }
    }
}
