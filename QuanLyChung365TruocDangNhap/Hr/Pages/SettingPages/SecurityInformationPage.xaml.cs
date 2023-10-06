using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages
{
    /// <summary>
    /// Interaction logic for SecurityInformationPage.xaml
    /// </summary>
    public partial class SecurityInformationPage : Page, INotifyPropertyChanged
    {
        private bool isShowDevices = false;

        private bool isShowChangePassword = false;
        string token;
        int typeUser;
        string comId;
        string epId;
        private bool show;

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
        public SecurityInformationPage(string token, int typeUser, string comId, string epId)
        {
            InitializeComponent();
            this.token = token;
            this.typeUser = typeUser;
            this.comId = comId;
            this.epId = epId;
            if(this.typeUser == 1)
            {
                Show = false;
            }
            else
            {
                Show = true;
            }
            DataContext = this;
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

        private void showHideDevices(object sender, MouseButtonEventArgs e)
        {
            if (!isShowDevices)
            {
                grid_show_devices.Visibility = Visibility.Collapsed;
                grid_hidden_devices.Visibility = Visibility.Visible;
                grid_more_devices.Visibility = Visibility.Visible;
            }
            else
            {
                grid_show_devices.Visibility = Visibility.Visible;
                grid_hidden_devices.Visibility = Visibility.Collapsed;
                grid_more_devices.Visibility = Visibility.Collapsed;
            }

            isShowDevices = !isShowDevices;
        }

        private void showHideChangePassword(object sender, MouseButtonEventArgs e)
        {
            ShowHideChangePassword();
        }

        private void ShowHideChangePassword()
        {
            ResetGridPass();
            if (isShowChangePassword)
            {
                grid_change_password.Visibility = Visibility.Collapsed;
                change_password_arrow_down.Visibility = Visibility.Visible;
                change_password_arrow_up.Visibility = Visibility.Collapsed;

            }
            else
            {
                grid_change_password.Visibility = Visibility.Visible;
                change_password_arrow_down.Visibility = Visibility.Collapsed;
                change_password_arrow_up.Visibility = Visibility.Visible;
            }


            isShowChangePassword = !isShowChangePassword;
        }

        private void ResetGridPass()
        {
            pbOldPass.Password = "";
            pbNewPass1.Password = "";
            pbNewPass2.Password = "";
        }

        private void ClickUpdatePass(object sender, MouseButtonEventArgs e)
        {
            if(Validate())
                UpdatePass();
        }

        private async void UpdatePass()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "";
                if(typeUser == 1)
                {
                    api = APIs.API.change_pass_employee;
                }
                else
                {
                    api = APIs.API.change_pass_company;
                }
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("old_password", pbOldPass.Password));
                parameters.Add(new KeyValuePair<string, string>("password", pbNewPass1.Password.Trim()));
                parameters.Add(new KeyValuePair<string, string>("re_password", pbNewPass1.Password.Trim()));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ResultEntity result = JsonConvert.DeserializeObject<ResultEntity>(responseContent);
                if (result.data != null && result.data.result)
                {
                    ShowHideChangePassword();
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show(result.error.message);
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại");
            }
        }

        private bool Validate()
        {
            if(pbNewPass1.Password.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được để trống.");
                return false;
            }

            if(pbNewPass1.Password.Trim().Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 kí tự.");
                return false;
            }

            if(pbNewPass1.Password != pbNewPass2.Password)
            {
                MessageBox.Show("Nhập lại mật khẩu mới không khớp.");
                return false;
            }

            return true;
        }
    }
}
