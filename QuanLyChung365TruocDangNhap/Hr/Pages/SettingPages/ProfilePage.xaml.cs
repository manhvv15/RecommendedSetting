using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using QuanLyChung365TruocDangNhap.Hr.View;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page, INotifyPropertyChanged
    {
        private bool isEditPersonalInfo = false;
        string token;
        int typeUser; // 1:nhân viên, 2:công ty
        string comId;
        string epId;
        private bool show;

        Dictionary<string, string> listItemGender = new Dictionary<string, string>();
        Dictionary<string, string> listItemMarried = new Dictionary<string, string>();

        public bool Show
        {
            get { return show; }
            set
            {
                show = value;
                OnPropertyChanged("Show");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        public ProfilePage(string token, int typeUser, string comId, string epId)
        {
            InitializeComponent();
            this.token = token;
            this.typeUser = typeUser;
            this.comId = comId;
            this.epId = epId;
            if (this.typeUser == 1)
            {
                Show = false;
            }
            else
            {
                Show = true;
            }
            DataContext = this;
            SetupCombobox();
            GetData();
        }

        private void SetupCombobox()
        {
            listItemGender.Add("1", "Nam");
            listItemGender.Add("2", "Nữ");
            listItemGender.Add("3", "Khác");
            listItemMarried.Add("1", "Độc thân");
            listItemMarried.Add("2", "Đã kết hôn");
            cbxGender.ItemsSource = listItemGender;
            cbxMarried.ItemsSource = listItemMarried;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", comId.ToString());
                form.Add("ep_id", epId.ToString());
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
                if (result.data != null && result.data.items.Count > 0)
                {
                    getListEmployee1 dataEntity = result.data.items[0];
                    tbName.Text = dataEntity.ep_name;
                    tbDepName.Text = dataEntity.dep_name;
                    tbId.Text = dataEntity.ep_id;
                    txtAddress.Text = dataEntity.ep_address;
                    txtSex.Text = dataEntity.gender;
                    cbxGender.SelectedItem = listItemGender.FirstOrDefault(t => t.Key == dataEntity.ep_gender);
                    cbxMarried.SelectedItem = listItemMarried.FirstOrDefault(t => t.Key == dataEntity.ep_married);
                    txtEmail.Text = dataEntity.ep_email;
                    txtPhoneNumber.Text = dataEntity.ep_phone;
                    if(dataEntity.ep_married == "1")
                    {
                        txtMaritalStatus.Text = "Độc thân";
                    } 

                    if(dataEntity.ep_married == "2")
                    {
                        txtMaritalStatus.Text = "Đã kết hôn";
                    }
                    string position = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == dataEntity.position_id).value;
                    tbPosition.Text = position;
                    if (dataEntity.start_working_time != null)
                    {
                        txtDateStartWork.Text = dataEntity.start_working_time;
                    }

                    if(dataEntity.ep_image != null && dataEntity.ep_image != "")
                    {
                        imageAvatar.ImageSource = new BitmapImage(new Uri(APIs.API.avatar_uri + dataEntity.ep_image));
                    }

                }
            }
            catch
            {

            }
        }

        private void editButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isEditPersonalInfo)
            {
                hideAllTbPersonalInfo();
            }
            else
            {
                showAllTbPersonalInfo();
            }

        }

        // ẩn form chỉnh sửa thông tin cá nhân
        private void hideAllTbPersonalInfo()
        {
            gridTbAddress.Visibility = Visibility.Collapsed;
            gridTbMaritalStatus.Visibility = Visibility.Collapsed;
            gridTbPhoneNumber.Visibility = Visibility.Collapsed;
            gridTbSex.Visibility = Visibility.Collapsed;
            gridBtnPersonalInfo.Visibility = Visibility.Collapsed;
            gridTbEmail.Visibility = Visibility.Collapsed;
            gridTbDateStartWork.Visibility = Visibility.Collapsed;

            isEditPersonalInfo = !isEditPersonalInfo;
        }


        // hiện form chỉnh sửa thông tin cá nhân
        private void showAllTbPersonalInfo()
        {
            gridTbAddress.Visibility = Visibility.Visible;
            gridTbMaritalStatus.Visibility = Visibility.Visible;
            gridTbPhoneNumber.Visibility = Visibility.Visible;
            gridTbSex.Visibility = Visibility.Visible;
            gridBtnPersonalInfo.Visibility = Visibility.Visible;
            gridTbEmail.Visibility = Visibility.Visible;
            gridTbDateStartWork.Visibility = Visibility.Visible;

            isEditPersonalInfo = !isEditPersonalInfo;
        }

        private void btnAddPersonal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (isAddFamilyMember)
            //{
            //    gridFormFamilyMember.Visibility = Visibility.Collapsed;
            //    gridBtnAddFamilyMember.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    gridFormFamilyMember.Visibility = Visibility.Visible;
            //    gridBtnAddFamilyMember.Visibility = Visibility.Visible;
            //}

            //isAddFamilyMember = !isAddFamilyMember;
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
                            (window as HomeView).MainContent.Navigate(new SettingPage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "Profile":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ProfilePage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "SecurityInformation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new SecurityInformationPage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "ActivityLog":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ActivityLogPage(token, typeUser, comId, epId));
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

        private void ClickBack(object sender, MouseButtonEventArgs e)
        {
            hideAllTbPersonalInfo();
        }

        private bool ValidateForm()
        {
            if(tbPhone.Text.Length < 10)
            {
                MessageBox.Show("SĐT tối thiểu là 10 ký tự");
                return false;
            }
            if (tbPhone.Text.Length > 15)
            {
                MessageBox.Show("SĐT không hợp lệ!");
                return false;
            }
            return true;
        }

        private void ClickUpdate(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
            {
                UpdateInfoEmployee();
            }
        }

        private async void UpdateInfoEmployee()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.update_employee_info;
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("userName", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("phoneTK", tbPhone.Text));
                parameters.Add(new KeyValuePair<string, string>("address", tbAddress.Text));
                parameters.Add(new KeyValuePair<string, string>("gender", cbxGender.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("married", cbxMarried.SelectedValue.ToString()));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ResultEntity result = JsonConvert.DeserializeObject<ResultEntity>(responseContent);
                if (result.data != null && result.data.result)
                {
                    hideAllTbPersonalInfo();
                    GetData();
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại");
            }
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
