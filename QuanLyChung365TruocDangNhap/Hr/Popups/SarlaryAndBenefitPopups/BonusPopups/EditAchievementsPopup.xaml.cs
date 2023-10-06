
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
using QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
using static QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups.AddListAchievementsPopup;

namespace HRApp.Popups.SarlaryAndBenefitPopups.BonusPopups
{
    /// <summary>
    /// Interaction logic for EditAchievementsPopup.xaml
    /// </summary>
    /// 
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
    public class getLisstEmployee1
    {
        public getLisstEmployee1(string ep_name, string ep_id)
        {
            Ep_name = ep_name;
            Ep_id = ep_id;
        }

        public string Ep_name { get; set; }
        public string Ep_id { get; set; }
    }
    public partial class EditAchievementsPopup : UserControl, INotifyPropertyChanged
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        string token;
        string achievement_id;
        string content;
        string created_by;
        string achievement_at;
        string appellation;
        string achievement_level;
        public string list_user;
        public string list_user_name;
        string id;
        List<getListEmployee1> listEmployees = new List<getListEmployee1>();

        int stt = 0;
        private string _searchText;
        bool isFocus = false;
        List<Employee> listAllEmployee = new List<Employee>();
        List<Employee> listEmployeeSelected = new List<Employee>();
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;

                OnPropertyChanged("SearchText");
                OnPropertyChanged("MyFilteredItems");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public List<Employee> MyItems { get; set; }

        public List<Employee> MyFilteredItems
        {
            get
            {
                if (!isFocus) return new List<Employee>();

                if (SearchText == null) return MyItems;

                List<Employee> listResult = new List<Employee>();

                foreach (var item in listAllEmployee)
                {
                    if (item.name.Contains(SearchText))
                    {
                        listResult.Add(new Employee() { name = item.name, id = item.id });
                    }
                }

                return listResult;
            }
        }

        public EditAchievementsPopup(string token, string id, string achievement_id, string content, string appellation, string achievement_level, string achievement_at, string created_by, string achievement_type, string list_user, string list_user_name)
        {
            InitializeComponent();
            this.token = token;
            GetDataListEmployee();
            List<cboxValue1> list = new List<cboxValue1>();
            list.Add(new cboxValue1("Huân chương", "1"));
            list.Add(new cboxValue1("Huy chương", "2"));
            list.Add(new cboxValue1("Giấy khen", "3"));
            list.Add(new cboxValue1("Thăng chức", "4"));
            list.Add(new cboxValue1("Kỉ niệm chương", "5"));
            list.Add(new cboxValue1("Tiền mặt", "6"));

            cbx1.ItemsSource = list;
            txtSoQuyetDinh.Text = achievement_id;
            txtNDkhenthuong.Text = content;
            txtNguoiKiQD.Text = created_by;
            txtDanhHieu.Text = appellation;
            txtCapKhen.Text = achievement_level;
            DTdate.Text = ConvertDate(achievement_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(achievement_at));
            cbx1.SelectedItem = list.FirstOrDefault(t => t.Value == achievement_type);

            this.list_user = list_user;
            this.list_user_name = list_user_name;
            this.created_by = created_by;
            this.id = id;

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }


        private async void GetAllEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = QuanLyChung365TruocDangNhap.Hr.APIs.API.list_all_employee;

                httpRequestMessage.RequestUri = new Uri(api);
                var form = new Dictionary<string, string>();
                form.Add("com_id", id);
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                foreach (var item in result.data.data)
                {
                    listAllEmployee.Add(new Employee() { id = item.ep_id, name = item.ep_name });
                }

                MyItems = listAllEmployee;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetDataListEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = QuanLyChung365TruocDangNhap.Hr.APIs.API.getlistEmplouyee;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);


                listEmployees = result.data.items;
                if (result.data == null) return;
                foreach (var item in result.data.items)
                {
                    listAllEmployee.Add(new Employee() { id = item.ep_id, name = item.ep_name });
                }

                MyItems = listAllEmployee;
                try
                {
                    listEmployees = result.data.items;
                    cbxTenDoiTuong.ItemsSource = listEmployees;
                    cbxTenDoiTuong.SelectedItem = listEmployees.FirstOrDefault(t => t.ep_id == list_user);
                    //foreach (var item in listEmployees)
                    //{
                    //    string dep_name = " (Chưa cập nhật)";
                    //    if (item.dep_name != null && item.dep_name != "")
                    //    {
                    //        dep_name = " (" + item.dep_name + ")";
                    //    }
                    //    item.ep_name = item.ep_name + dep_name;
                    //}
                    //cbxTenDoiTuong.ItemsSource = listEmployees;
                }
                catch
                {

                }

            }
            catch (Exception)
            {
                //MessageBox.Show("Error");
            }
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
         {
            hidePopup(0);
        }
        private void UpdateAchievement(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                UpdateListAchievements();
        }
        private async void UpdateListAchievements()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Put;
                string api = QuanLyChung365TruocDangNhap.Hr.APIs.API.UpdateAchievementsPopup;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                string list_user = "";
                string list_user_name = "";
                InputAchievement input = new InputAchievement();
                if(cbxTenDoiTuong.SelectedValue != null)
                {
                    input.list_user.Add(int.Parse(cbxTenDoiTuong.SelectedValue.ToString()));
                    input.list_user_name.Add(((getListEmployee1)cbxTenDoiTuong.SelectedItem).ep_name);
                }
                input.achievement_level = txtCapKhen.Text;
                input.achievement_id = txtSoQuyetDinh.Text;
                input.content = txtNDkhenthuong.Text;
                input.achievement_at = DTdate.SelectedDate.Value.ToString("yyyy-MM-dd");
                input.created_by = txtNguoiKiQD.Text;
                input.appellation = txtDanhHieu.Text;
                input.achievement_type = int.Parse(cbx1.SelectedValue.ToString());
                input.id = int.Parse(id);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhật thành công.");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại.");
            }

        }

        private bool ValidateForm()
        {
            if (txtSoQuyetDinh.Text == "")
            {
                MessageBox.Show("Số quy định không được để trống.");
                return false;
            }

            if (txtNDkhenthuong.Text == "")
            {
                MessageBox.Show("Nội dung khen thưởng không được để trống.");
                return false;
            }

            if (cbxTenDoiTuong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tên phòng ban.");
                return false;
            }

            if (txtNguoiKiQD.Text == "")
            {
                MessageBox.Show("Người ký quyết định không được để trống.");
                return false;
            }

            if (DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời điểm.");
                return false;
            }

            if (cbx1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn hình thức khen thưởng.");
                return false;
            }

            if (txtDanhHieu.Text == "")
            {
                MessageBox.Show("Danh hiệu không được để trống.");
                return false;
            }

            if (txtCapKhen.Text == "")
            {
                MessageBox.Show("Cấp khen không được để trống.");
                return false;
            }

            return true;
        }

        private string ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return "";
                }
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            isFocus = false;
            SearchText = SearchText;
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            isFocus = true;
            SearchText = SearchText;
        }

        //private void lbSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    //tbSearch.Text = "";
        //    ListBoxItem item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
        //    if (item != null)
        //    {
        //        Employee employee = item.DataContext as Employee;
        //        if (!IsSelectedEp(employee.id))
        //        {
        //            employee.stt = stt;
        //            stt++;
        //            listEmployeeSelected.Add(employee);
        //            icListEmployee.Items.Refresh();
        //            icListEmployee.ItemsSource = listEmployeeSelected;
        //        }
        //    }
        //}
        private bool IsSelectedEp(string id)
        {
            if (listEmployeeSelected.Count == 0) return false;

            foreach (var item in listEmployeeSelected)
            {
                if (item.id == id) return true;
            }

            return false;
        }

        //private void DeleteItem(object sender, MouseButtonEventArgs e)
        //{
        //    Border grid = sender as Border;
        //    Employee employee = grid.DataContext as Employee;
        //    string id = employee.id;
        //    int index = 0;
        //    for (int i = 0; i < listEmployeeSelected.Count; i++)
        //    {
        //        if (listEmployeeSelected[i].id == id)
        //        {
        //            index = i;
        //            break;

        //        }
        //    }
        //    listEmployeeSelected.RemoveAt(index);
        //    if (listEmployeeSelected.Count == 0)
        //    {
        //        tbSearch.Text = "";
        //    }
        //    icListEmployee.Items.Refresh();
        //    icListEmployee.ItemsSource = listEmployeeSelected;
        //}
        public class Employee
        {
            public int stt { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }
        private void cbxTenDoiTuong_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenDoiTuong.SelectedIndex = -1;
            string textSearch = cbxTenDoiTuong.Text + e.Text;
            cbxTenDoiTuong.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenDoiTuong.Text = "";
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = listEmployees;
                cbxTenDoiTuong.SelectedIndex = -1;
            }
            else
            {
                cbxTenDoiTuong.ItemsSource = "";
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = listEmployees.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenDoiDuong_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenDoiTuong.SelectedIndex = -1;
                string textSearch = cbxTenDoiTuong.Text;
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = listEmployees.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
