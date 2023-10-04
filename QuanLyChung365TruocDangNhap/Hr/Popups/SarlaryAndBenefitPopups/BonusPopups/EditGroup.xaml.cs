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
using static QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups.AddListAchievementsPopup;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups
{
    /// <summary>
    /// Interaction logic for EditGroup.xaml
    /// </summary>
    public partial class EditGroup : UserControl, INotifyPropertyChanged
    {

        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        string token;
        public string list_user;
        public string list_user_name;
        string id;
        string comId;
        string achievement_id;
        string content;
        string created_by;
        string achievement_at;
        string appellation;
        string achievement_level;

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

        public EditGroup(string token, string comId,string id, string achievement_id, string content, string list_user,string achievement_at,string achievement_type,string appellation,string achievement_level,string created_by,string achievement_name)
        {
            InitializeComponent();
            this.token = token;
            this.comId = comId;
            this.id = id;
            GetAllEmployee(list_user);

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
            
           

            cbx1.ItemsSource = list;
            txtSoQuyetDinh.Text = achievement_id;
            txtNDkhenthuong.Text = content;
            txtNguoiKiQD.Text = created_by;
            txtDanhHieu.Text = appellation;
            txtCapKhen.Text = achievement_level;
            DTdate.Text = ConvertDate(achievement_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(achievement_at));
            cbx1.SelectedItem = list.FirstOrDefault(t => t.Value == achievement_type);

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;

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

        private bool ValidateForm()
        {
            return true;
        }


        private void EditPopup(object sender, MouseButtonEventArgs e)
        {

                EditGroupGetData();
        }

        private async void EditGroupGetData()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.updateAchievement;

            httpRequestMessage.RequestUri = new Uri(api);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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
            input.id = int.Parse(id);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Thiết lập Content
            var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
            httpRequestMessage.Content = content;

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();

            ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);
            //result.success != null && result.success.data != null && 
            if (result.data.result)
            {
                hidePopup(1);
                MessageBox.Show("Cập nhật thành tích thành công!");
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
                form.Add("com_id", comId);
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                string[] stringSeparators = new string[] { ", " , ","};
                string[] listWord = list_user.Split(stringSeparators, StringSplitOptions.None);
                foreach (var item in result.data.data)
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
