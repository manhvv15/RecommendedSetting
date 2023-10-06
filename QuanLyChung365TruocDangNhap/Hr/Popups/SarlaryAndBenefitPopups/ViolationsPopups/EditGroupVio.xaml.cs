using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
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
    /// Interaction logic for EditGroupVio.xaml
    /// </summary>
    public partial class EditGroupVio : UserControl, INotifyPropertyChanged
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        string token;
        string comid;
        string id;
        public string list_user;
        //public string list_user_name;
        public string infringe_name;
        public string regulatory_basis;
        public string created_by;
        public string infringe_at;
        public string infringe_type;
        public string number_violation;
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
        public EditGroupVio(string token,string comid,string id,string list_user,string infringe_name,string regulatory_basis,string created_by,string infringe_at,string infringe_type,string number_violation)
        {
            InitializeComponent();
            this.token = token;
            this.comid = comid;
            this.id = id;
            GetAllEmployee(list_user);

            this.DataContext = this;

            tbTenLoiVP.Text = infringe_name;
            tbCanCuQD.Text = regulatory_basis;
            DTdate.Text = ConvertDate(infringe_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(infringe_at));
            tbHinhThucXLVP.Text = infringe_type;
            tbSoQDXuLyVP.Text = number_violation;
            tbNguoiKyQD.Text = created_by;


            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private async void GetAllEmployee(string list_user)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_employee;

                httpRequestMessage.RequestUri = new Uri(api);
                var form = new Dictionary<string, string>();
                form.Add("com_id", comid);
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                string[] stringSeparators = new string[] { ", ", "," };
                string[] listWord = list_user.Split(stringSeparators, StringSplitOptions.None);
                foreach (var item in result.data.items)
                {
                    if (listWord.Length > 0 && listWord.Contains(item.ep_id))
                    {
                        listEmployeeSelected.Add(new Employee() { id = item.ep_id, name = item.ep_name });
                    }

                    string dep_name = " (Chưa cập nhật)";
                    if (item.dep_name != null && item.dep_name != "")
                    {
                        dep_name = " (" + item.dep_name + ")";
                    }
                    item.ep_name = item.ep_name + dep_name;
                    listAllEmployee.Add(new Employee() { id = item.ep_id, name = item.ep_name });
                }

                MyItems = listAllEmployee;

                icListEmployee.Items.Refresh();
                icListEmployee.ItemsSource = listEmployeeSelected;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private bool ValidateFrom()
        {
            return true;
        }
        private void EditGroupViolations(object sender, MouseButtonEventArgs e)
        {
            if(ValidateFrom())
            EditGroupViGetData();
        }
        private async void EditGroupViGetData()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Put;
            string api = APIs.API.updateInfinges;

            httpRequestMessage.RequestUri = new Uri(api);
            var parameters = new List<KeyValuePair<string, string>>();

            string list_user = "";
            string list_user_name = "";
            var content = new MultipartFormDataContent();
            for (int i = 0; i < listEmployeeSelected.Count; i++)
            {
                /*if (i != 0)
                {
                    list_user += ",";
                    list_user_name += ",";
                }
                list_user += listEmployeeSelected[i].id;
                list_user_name += listEmployeeSelected[i].name;*/
                content.Add(new StringContent(listEmployeeSelected[i].name), "list_user_name[]");
                content.Add(new StringContent(listEmployeeSelected[i].id), "list_user[]");
            }


            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            parameters.Add(new KeyValuePair<string, string>("id", id));
            parameters.Add(new KeyValuePair<string, string>("infringe_name", tbTenLoiVP.Text));
            parameters.Add(new KeyValuePair<string, string>("regulatory_basis", tbCanCuQD.Text));
            parameters.Add(new KeyValuePair<string, string>("number_violation", tbSoQDXuLyVP.Text));
            parameters.Add(new KeyValuePair<string, string>("infringe_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
            parameters.Add(new KeyValuePair<string, string>("infringe_type", tbHinhThucXLVP.Text));
            parameters.Add(new KeyValuePair<string, string>("created_by", tbNguoiKyQD.Text));
            content.Add(new StringContent(tbTenLoiVP.Text), "infringe_name");
            content.Add(new StringContent(tbCanCuQD.Text), "regulatory_basis");
            content.Add(new StringContent(tbNguoiKyQD.Text), "created_by");
            content.Add(new StringContent(DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")), "infringe_at");
            content.Add(new StringContent(tbHinhThucXLVP.Text), "infringe_type");
            content.Add(new StringContent(tbSoQDXuLyVP.Text), "number_violation");
            content.Add(new StringContent(id), "id");
            // Thiết lập Content
            //var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

            if (result.data.result)
            {
                hidePopup(1);
                MessageBox.Show(result.success.message);
            }
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

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
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

    }
}
