using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.ViolationsPopups
{
    /// <summary>
    /// Interaction logic for PersonalAddViolationPopup.xaml
    /// </summary>
    /// insert into Messages(id,conversationId,senderId,messageType,message,quoteMessage,createAt,messageQuote,isEdited)
    //values('637951266190000000_139',170,139,'text','ĐC Mạnh- PHÒNG 9 - BÁO CÁO CÔNG VIỆC sáng 3.8.2022:-Đổ chức năng tìm kiếm trang chủ rao nhanh tìm kiếm theo key','','2022-08-03 12:30:40.050','',0)

    public partial class PersonalAddViolationPopup : UserControl, INotifyPropertyChanged
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

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
        string token;
        string id;
        int TypeUser;
        public PersonalAddViolationPopup(string token,string id,int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            GetDataListEmployee();
            this.DataContext = this;



            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        private async void GetDataListEmployee()
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
                foreach (var item in result.data.items)
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
        private void AddNewViolation(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                AddNewViolationGetData();
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
        public class InputKyLuatCaNhan
        {
            public string regulatory_basis { get; set; }
            public string infringe_name { get; set; }
            public string created_by { get; set; }
            public string infringe_at { get; set; }
            public int infringe_type { get; set; }
            public string number_violation { get; set; }
            public List<int> list_user { get; set; } = new List<int>();
            public List<string> list_user_name { get; set; } = new List<string>();
            public int price { get; set; }
        }
        private async void AddNewViolationGetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.addInfinges;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                string list_user = "";
                string list_user_name = "";
                InputKyLuatCaNhan inputKyLuatCaNhan = new InputKyLuatCaNhan();
                for (int i = 0; i < listEmployeeSelected.Count; i++)
                {
                    inputKyLuatCaNhan.list_user.Add(int.Parse(listEmployeeSelected[i].id));
                    inputKyLuatCaNhan.list_user_name.Add(listEmployeeSelected[i].name);
                }
                inputKyLuatCaNhan.number_violation = tbSoQDXuLyVP.Text;
                inputKyLuatCaNhan.regulatory_basis = tbCanCuQD.Text;
                inputKyLuatCaNhan.infringe_name = tbTenLoiVP.Text;
                inputKyLuatCaNhan.created_by = tbNguoiKyQD.Text;
                inputKyLuatCaNhan.price = int.Parse(tbNumber.Text);
                inputKyLuatCaNhan.infringe_type = int.Parse(tbHinhThucXLVP.Text);
                inputKyLuatCaNhan.infringe_at = DTdate.SelectedDate.Value.ToString("yyyy-MM-dd");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                /*parameters.Add(new KeyValuePair<string, string>("infringe_name", tbTenLoiVP.Text));
                parameters.Add(new KeyValuePair<string, string>("regulatory_basis", tbCanCuQD.Text));
                parameters.Add(new KeyValuePair<string, string>("number_violation", tbSoQDXuLyVP.Text));
                parameters.Add(new KeyValuePair<string, string>("infringe_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                parameters.Add(new KeyValuePair<string, string>("created_by", tbNguoiKyQD.Text));
                parameters.Add(new KeyValuePair<string, string>("infringe_type", tbHinhThucXLVP.Text));
                parameters.Add(new KeyValuePair<string, string>("list_user", list_user));
                parameters.Add(new KeyValuePair<string, string>("list_user_name", list_user_name));
                string api1 = APIs.API.add_punish_emp;
                parameters.Add(new KeyValuePair<string, string>("amount", tbNumber.Text));
                parameters.Add(new KeyValuePair<string, string>("type", "2"));
                parameters.Add(new KeyValuePair<string, string>("des", tbLydo.Text));*/

                //parameters.Add(new KeyValuePair<string, string>("list_user_name", cbxTenDoiTuong.Text));
                //parameters.Add(new KeyValuePair<string, string>("list_user", cbxTenDoiTuong.SelectedValue.ToString()));

                // Thiết lập Content
                var content = new StringContent(JsonConvert.SerializeObject(inputKyLuatCaNhan), null, "application/json");
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
                    MessageBox.Show("Thêm mới thất bại, vui lòng thử lại.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại.");
            }

        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private bool ValidateForm()
        {
            if(tbTenLoiVP.Text.Trim() == "")
            {
                MessageBox.Show("Tên lỗi vi phạm không được để trống.");
                return false;
            }

            if(tbCanCuQD.Text.Trim() == "")
            {
                MessageBox.Show("Căn cứ quy định không được để trống.");
                return false;
            }

            if(tbSoQDXuLyVP.Text.Trim() == "")
            {
                MessageBox.Show("Số quy định xử lý vi phạm không được để trống.");
                return false;
            }

            if(DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền thời gian vi phạm");
                return false;
            }

            if(tbHinhThucXLVP.Text.Trim() == "")
            {
                MessageBox.Show("Hình thức xử lý vi phạm không được để trống");
                return false;
            }

            //if(cbxTenDoiTuong.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Vui lòng chọn tên đối tượng");
            //    return false;
            //}

            if(tbNguoiKyQD.Text.Trim() == "")
            {
                MessageBox.Show("Người ký quyết định không được để trống.");
                return false;
            }

            //if (tbNumber.Text.Trim() == "")
            //{
            //    MessageBox.Show("Số tiền không được để trống");
            //    return false;
            //}

            if (tbLydo.Text.Trim() == "")
            {
                MessageBox.Show("Lý do không được bỏ trống");
                return false;
            }

            if (Convert.ToInt32(tbNumber.Text) <= 0)
            {
                MessageBox.Show("Số tiền phải lớn hơn 0");
                return false;
            }

            return true;
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
            parameters.Add(new KeyValuePair<string, string>("Message", tbLydo.Text));


            parameters.Add(new KeyValuePair<string, string>("ListEmployee", "[" + list_user + "]"));
            parameters.Add(new KeyValuePair<string, string>("CreateAt", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));

            string type = "Discipline";
            parameters.Add(new KeyValuePair<string, string>("Type", type));

            string Status_com = TypeUser.ToString();
            parameters.Add(new KeyValuePair<string, string>("Status", Status_com));
            parameters.Add(new KeyValuePair<string, string>("ListReceive", "[" + list_user + "]"));
            parameters.Add(new KeyValuePair<string, string>("SenderId", id));

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
