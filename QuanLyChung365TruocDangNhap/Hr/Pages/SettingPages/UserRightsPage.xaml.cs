using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.SettingEntities;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages
{
    /// <summary>
    /// Interaction logic for UserRightsPage.xaml
    /// </summary>
    public partial class UserRightsPage : Page, INotifyPropertyChanged
    {
        string token;
        string comId;
        int typeUser;
        string EpId;
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

        public string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",
            };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public List<Employee> MyFilteredItems
        {
            get
            {
                if (!isFocus) return new List<Employee>();

                if (SearchText == null) return MyItems;

                List<Employee> listResult = new List<Employee>();

                foreach (var item in listAllEmployee)
                {
                    if (RemoveUnicode(item.name).ToLower().Contains(RemoveUnicode(SearchText).ToLower()))
                    {
                        listResult.Add(new Employee() { name = item.name, id = item.id });
                    }
                }

                return listResult;
            }
        }
        bool isSetupRole = false;
        bool[,] arrPermission = new bool[8, 5];



        public UserRightsPage(string token, string comId, int typeUser,string EpId)
        {
            InitializeComponent();
            this.token = token;
            this.comId = comId;
            this.typeUser = typeUser;
            this.EpId = EpId;
            GetAllEmployee();
            this.DataContext = this;
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
                form.Add("com_id", comId);
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

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            String uri = "SettingPages/" + name + "Page";

            switch (name)
            {
                case "Setting":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new SettingPage(token, typeUser, comId, EpId));
                        }
                    };
                    break;
                case "Profile":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ProfilePage(token, typeUser, comId, EpId));
                        }
                    };
                    break;
                case "SecurityInformation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new SecurityInformationPage(token, typeUser, comId,EpId));
                        }
                    };
                    break;
                case "ActivityLog":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ActivityLogPage(token, typeUser, comId, EpId));
                        }
                    };
                    break;
                case "ListMember":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ListMemberPage());
                        }
                    };
                    break;
            }
        }

        private void NavigateBack(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.NavigationService.GoBack();
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

        private void lbSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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
                GetUserPermission(employee.id);
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
            Grid grid = sender as Grid;
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
            icListEmployee.Items.Refresh();
            icListEmployee.ItemsSource = listEmployeeSelected;
        }


        private async void UpdateRole()
        {
            try
            {
                string user_id = "";
                foreach (var item in listEmployeeSelected)
                {
                    user_id += item.id + ",";
                }

                string role_td = "";
                if (cb11.IsChecked == true) role_td += "1,";
                if (cb12.IsChecked == true) role_td += "2,";
                if (cb13.IsChecked == true) role_td += "3,";
                if (cb14.IsChecked == true) role_td += "4,";

                string role_ttns = "";
                if (cb21.IsChecked == true) role_ttns += "1,";
                if (cb22.IsChecked == true) role_ttns += "2,";
                if (cb23.IsChecked == true) role_ttns += "3,";
                if (cb24.IsChecked == true) role_ttns += "4,";

                string role_ttvp = "";
                if (cb31.IsChecked == true) role_ttvp += "1,";
                if (cb32.IsChecked == true) role_ttvp += "2,";
                if (cb33.IsChecked == true) role_ttvp += "3,";
                if (cb34.IsChecked == true) role_ttvp += "4,";

                string role_hnnv = "";
                if (cb41.IsChecked == true) role_hnnv += "1,";
                if (cb42.IsChecked == true) role_hnnv += "2,";
                if (cb43.IsChecked == true) role_hnnv += "3,";
                if (cb44.IsChecked == true) role_hnnv += "4,";

                string role_bcns = "";
                if (cb51.IsChecked == true) role_bcns += "1,";
                if (cb52.IsChecked == true) role_bcns += "2,";
                if (cb53.IsChecked == true) role_bcns += "3,";
                if (cb54.IsChecked == true) role_bcns += "4,";

                string role_dldx = "";
                if (cb61.IsChecked == true) role_dldx += "1,";
                if (cb62.IsChecked == true) role_dldx += "2,";
                if (cb63.IsChecked == true) role_dldx += "3,";
                if (cb64.IsChecked == true) role_dldx += "4,";

                string role_tgl = "";
                if (cb71.IsChecked == true) role_tgl += "1,";
                if (cb72.IsChecked == true) role_tgl += "2,";
                if (cb73.IsChecked == true) role_tgl += "3,";
                if (cb74.IsChecked == true) role_tgl += "4,";


                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.updateRole;
                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("userId", user_id));
                parameters.Add(new KeyValuePair<string, string>("role_td", role_td));
                parameters.Add(new KeyValuePair<string, string>("role_ttns", role_ttns));
                parameters.Add(new KeyValuePair<string, string>("role_ttvp", role_ttvp));
                parameters.Add(new KeyValuePair<string, string>("role_hnnv", role_hnnv));
                parameters.Add(new KeyValuePair<string, string>("role_bcns", role_bcns));
                parameters.Add(new KeyValuePair<string, string>("role_dldx", role_dldx));
                parameters.Add(new KeyValuePair<string, string>("role_tgl", role_tgl));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    listEmployeeSelected.Clear();
                    ResetAllCheckBox();
                    MessageBox.Show("Cấp quyền thành công!");
                    tbSearch.Clear();
                    icListEmployee.Items.Refresh();

                }
                else
                {
                    MessageBox.Show("Cấp quyền thất bại, vui lòng thử lại.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại");
            }
        }

        private void ClickUpdateRole(object sender, MouseButtonEventArgs e)
        {
            if (listEmployeeSelected.Count == 0) return;
            UpdateRole();
        }

        private void ResetAllCheckBox()
        {
            cb11.IsChecked = false;
            cb12.IsChecked = false;
            cb13.IsChecked = false;
            cb14.IsChecked = false;

            cb21.IsChecked = false;
            cb22.IsChecked = false;
            cb23.IsChecked = false;
            cb24.IsChecked = false;

            cb31.IsChecked = false;
            cb32.IsChecked = false;
            cb33.IsChecked = false;
            cb34.IsChecked = false;

            cb41.IsChecked = false;
            cb42.IsChecked = false;
            cb43.IsChecked = false;
            cb44.IsChecked = false;

            cb51.IsChecked = false;
            cb52.IsChecked = false;
            cb53.IsChecked = false;
            cb54.IsChecked = false;

            cb61.IsChecked = false;
            cb62.IsChecked = false;
            cb63.IsChecked = false;
            cb64.IsChecked = false;

            cb71.IsChecked = false;
            cb72.IsChecked = false;
            cb73.IsChecked = false;
            cb74.IsChecked = false;
        }
        //Check phân quyền
        private async void GetUserPermission(string id)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.apiCheckRole;
                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                parameters.Add(new KeyValuePair<string, string>("userId", id));
                parameters.Add(new KeyValuePair<string, string>("winform", "1"));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                PermissionEntity result = JsonConvert.DeserializeObject<PermissionEntity>(responseContent);
                if (result.result)
                {
                    foreach (var item in result.success.data.list_permision)
                    {
                        int i = Convert.ToInt32(item.bar_id);
                        int j = Convert.ToInt32(item.per_id);
                        CheckBox checkBox = FindName("cb" + i + j) as CheckBox;
                        if(checkBox != null)
                        {
                            checkBox.IsChecked = true;
                            arrPermission[i, j] = true;
                        }
                    }
                    isSetupRole = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lai!");
            }
       
        }
    }

    public class Employee
    {
        public int stt { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }
}
