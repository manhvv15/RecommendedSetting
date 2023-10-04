using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
using QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups
{
    /// <summary>
    /// Interaction logic for AddListAchievementsPopup.xaml
    /// </summary>
    /// 
    //0942862055 
    public class cboxValue
    {
        public cboxValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class getLisstEmployee
    {
        public getLisstEmployee(string ep_name, string ep_id)
        {
            Ep_name = ep_name;
            Ep_id = ep_id;
        }

        public string Ep_name { get; set; }
        public string Ep_id { get; set; }
    }
    public partial class AddListAchievementsPopup : UserControl, INotifyPropertyChanged
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        string token;
        public string list_user;
        public string list_user_name;
        string id;
        string comId;
        int page_now = 1;

        int TypeUser;
        const int COUNT_RECORD_PER_PAGE = 1000;
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

        public AddListAchievementsPopup(string token, string com_id, int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.id = com_id;
            this.TypeUser = TypeUser;
            GetAllEmployee();

            //GetDataListEmployee();
            List<cboxValue> list = new List<cboxValue>();
            list.Add(new cboxValue("Huân chương", "1"));
            list.Add(new cboxValue("Huy chương", "2"));
            list.Add(new cboxValue("Giấy khen", "3"));
            list.Add(new cboxValue("Thăng chức", "4"));
            list.Add(new cboxValue("Kỉ niệm chương", "5"));
            list.Add(new cboxValue("Tiền mặt", "6"));
            cbx1.ItemsSource = list;
            this.DataContext = this;
        }
        private async void GetDataListEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";

                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                form.Add("pageNumber", "1");
                form.Add("pageSize", "10000");

                httpRequestMessage.RequestUri = new Uri(api);
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
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void AddtAchievementsPopup(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                AddListAchievements();
        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );
            string replacement = Regex.Replace(textRange.Text, @"\t|\n|\r", " ");
            return replacement;
        }
        public class InputAchievement
        {
            public int id { get; set; }
            public string achievement_id { get; set; }
            public string content { get; set; }
            public List<int> list_user { get; set; } = new List<int>();
            public int depId { get; set; }
            public List<string> list_user_name { get; set; } = new List<string>();
            public string depName { get; set; }
            public string created_by { get; set; }
            public string achievement_at { get; set; }
            public int achievement_type { get; set; }
            public string appellation { get; set; }
            public string achievement_level { get; set; }
            public string created_at { get; set; }
            public string com_id { get; set; }
            public int price { get; set; }
        }
        private async void AddListAchievements()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.AddAchievementsPopup;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                string list_user = "";
                string list_user_name = "";
                InputAchievement input = new InputAchievement();
                for (int i = 0; i < listEmployeeSelected.Count; i++)
                {
                    input.list_user.Add(int.Parse(listEmployeeSelected[i].id));
                    input.list_user_name.Add(listEmployeeSelected[i].name);
                }
                input.achievement_level = txtCapKhen.Text;
                input.achievement_id = txtSoQuyetDinh.Text;
                input.content = txtNDkhenthuong.Text;
                input.achievement_at = DTdate.SelectedDate.Value.ToString("yyyy-MM-dd");
                input.created_by = txtNguoiKiQD.Text;
                input.appellation = txtDanhHieu.Text;
                input.achievement_type = int.Parse(cbx1.SelectedValue.ToString());
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                //parameters.Add(new KeyValuePair<string, string>("token", token));
                //parameters.Add(new KeyValuePair<string, string>("list_user", list_user));
                //parameters.Add(new KeyValuePair<string, string>("list_user_name", list_user_name));
                //parameters.Add(new KeyValuePair<string, string>("achievement_id", txtSoQuyetDinh.Text));
                //parameters.Add(new KeyValuePair<string, string>("content", txtNDkhenthuong.Text));
                //parameters.Add(new KeyValuePair<string, string>("achievement_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                //parameters.Add(new KeyValuePair<string, string>("created_by", txtNguoiKiQD.Text));
                //parameters.Add(new KeyValuePair<string, string>("appellation", txtDanhHieu.Text));
                //parameters.Add(new KeyValuePair<string, string>("achievement_level", txtCapKhen.Text));


                if(cbx1.SelectedValue.ToString() == "6")
                {
                    input.price = int.Parse(tbNumber.Text);
                    /*parameters.Add(new KeyValuePair<string, string>("amount", tbNumber.Text));
                    parameters.Add(new KeyValuePair<string, string>("des", tbLydo.Text));
                    parameters.Add(new KeyValuePair<string, string>("type", "1"));*/
                }

                //parameters.Add(new KeyValuePair<string, string>("achievement_type", cbx1.SelectedValue.ToString()));

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
                    MessageBox.Show("Thêm mới thành công.");
                    NotificationPersonnelChangeAppChat();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn tên đối tượng!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private bool ValidateForm()
        {
            if (txtSoQuyetDinh.Text.Trim() == "")
            {
                MessageBox.Show("Số quyết định không được để trống");
                return false;
            }

            if (txtNDkhenthuong.Text.Trim() == "")
            {
                MessageBox.Show("Nội dung khen thưởng không được để trống.");
                return false;
            }

            //if(cbx2.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Vui lòng chọn tên đối tượng");
            //    return false;
            //}

            if (txtNguoiKiQD.Text.Trim() == "")
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
                MessageBox.Show("Vui lòng chọn hình thức khen thưởng");
                return false;
            }

            if (txtDanhHieu.Text.Trim() == "")
            {
                MessageBox.Show("Danh hiệu không được để trống");
                return false;
            }

            if (txtCapKhen.Text.Trim() == "")
            {
                MessageBox.Show("Cấp khen không được để trống");
                return false;
            }

            //if (tbNumber.Text.Trim() == "")
            //{
            //    MessageBox.Show("Số tiền không được để trống");
            //    return false;
            //}

            if (tbLydo.Text.Trim() == "" && cbx1.SelectedIndex == 5)
            {
                MessageBox.Show("Lý do không được bỏ trống");
                return false;
            }

            if (!string.IsNullOrEmpty(tbNumber.Text) && Convert.ToInt32(tbNumber.Text) <= 0 && cbx1.SelectedIndex == 5)
            {
                MessageBox.Show("Số tiền phải lớn hơn 0");
                return false;
            }
            else if(cbx1.SelectedIndex == 5)
            {
                MessageBox.Show("Số tiền phải lớn hơn 0");
                return false;
            }    

            return true;
        }



        private async void GetAllEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_employee;

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
                    string dep_name = " (Chưa cập nhật)";
                    if (item.dep_name != null && item.dep_name != "")
                    {
                        dep_name = " (" + item.dep_name + ")";
                    }
                    item.ep_name = item.ep_name + dep_name;
                    listAllEmployee.Add(new Employee() { id = item.ep_id, name = item.ep_name });
                }

                MyItems = listAllEmployee;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        //private void cbxTenDoiTuong_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    cbxTenDoiTuong.SelectedIndex = -1;
        //    string textSearch = cbxTenDoiTuong.Text + e.Text;
        //    cbxTenDoiTuong.IsDropDownOpen = true;
        //    if (textSearch == "")
        //    {
        //        cbxTenDoiTuong.Text = "";
        //        cbxTenDoiTuong.Items.Refresh();
        //        cbxTenDoiTuong.ItemsSource = listEmployees;
        //        cbxTenDoiTuong.SelectedIndex = -1;
        //    }
        //    else
        //    {
        //        cbxTenDoiTuong.ItemsSource = "";
        //        cbxTenDoiTuong.Items.Refresh();
        //        cbxTenDoiTuong.ItemsSource = listEmployees.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
        //    }
        //}

        //private void cbxTenDoiTuong_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Back)
        //    {
        //        cbxTenDoiTuong.SelectedIndex = -1;
        //        string textSearch = cbxTenDoiTuong.Text;
        //        cbxTenDoiTuong.Items.Refresh();
        //        cbxTenDoiTuong.ItemsSource = listEmployees.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
        //    }
        //}



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

        private void lbSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //tbSearch.Text = "";
            ListBoxItem item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                Employee employee = item.DataContext as Employee;
                if (!IsSelectedEp(employee.id))
                {
                    employee.stt = stt;
                    stt++;
                    listEmployeeSelected.Add(employee);
                    icListEmployee.Items.Refresh();
                    icListEmployee.ItemsSource = listEmployeeSelected;
                }
            }
        }
        private bool IsSelectedEp(string id)
        {
            if (listEmployeeSelected.Count == 0) return false;

            foreach (var item in listEmployeeSelected)
            {
                if (item.id == id) return true;
            }

            return false;
        }

        private void DeleteItem(object sender, MouseButtonEventArgs e)
        {
            Border grid = sender as Border;
            Employee employee = grid.DataContext as Employee;
            string id = employee.id;
            int index = 0;
            for (int i = 0; i < listEmployeeSelected.Count; i++)
            {
                if (listEmployeeSelected[i].id == id)
                {
                    index = i;
                    break;

                }
            }
            listEmployeeSelected.RemoveAt(index);
            if (listEmployeeSelected.Count == 0)
            {
                tbSearch.Text = "";
            }
            icListEmployee.Items.Refresh();
            icListEmployee.ItemsSource = listEmployeeSelected;
        }
        public class Employee
        {
            public int stt { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }

        private void Cbx1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string combobox = cbx1.SelectedValue.ToString();

            if (combobox == "6")
            {
                inputNuberMoney.Visibility = Visibility.Visible;
                inputNote.Visibility = Visibility.Visible;
            }
            else
            {
                inputNuberMoney.Visibility = Visibility.Collapsed;
                inputNote.Visibility = Visibility.Collapsed;

            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private async void NotificationPersonnelChangeAppChat()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.NotificationRewardDiscipline;

            httpRequestMessage.RequestUri = new Uri(api);
            var parameters = new List<KeyValuePair<string, string>>();

            string list_user = "";
            for (int i = 0; i < listEmployeeSelected.Count; i++)
            {
                if (i != 0)
                {
                    list_user += ",";
                }
                list_user += listEmployeeSelected[i].id;
            }
            parameters.Add(new KeyValuePair<string, string>("CompanyId", id));
            parameters.Add(new KeyValuePair<string, string>("Message", txtNDkhenthuong.Text));

        
            parameters.Add(new KeyValuePair<string, string>("ListEmployee", "[" + list_user + "]"));
            parameters.Add(new KeyValuePair<string, string>("CreateAt", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));

            string type = "Reward";
            parameters.Add(new KeyValuePair<string, string>("Type", type));
            string Status_com = TypeUser.ToString();
            parameters.Add(new KeyValuePair<string, string>("Status", Status_com));
            parameters.Add(new KeyValuePair<string, string>("ListReceive", "[" + list_user + "]"));
            parameters.Add(new KeyValuePair<string, string>("SenderId", id));
            //if (cbx1.SelectedValue.ToString() == "6")
            //{
            //    string api1 = APIs.API.add_punish_emp;
            //    parameters.Add(new KeyValuePair<string, string>("amount", tbNumber.Text));
            //    parameters.Add(new KeyValuePair<string, string>("des", StringFromRichTextBox(rtblyDo)));
            //}
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
    }
}
