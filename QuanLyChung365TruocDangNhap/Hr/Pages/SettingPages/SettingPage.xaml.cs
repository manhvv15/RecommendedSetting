using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.SettingEntities;
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
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page, INotifyPropertyChanged
    {
        string token;
        int typeUser; // 1:nhân viên, 2:công ty
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
        public SettingPage(string token, int typeUser, string comId, string epId)
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
                GetInfoCompany();
            }
            DataContext = this;
        }

        private async void GetInfoCompany()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.company_info;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", comId);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                CompanyInfoEntity result = JsonConvert.DeserializeObject<CompanyInfoEntity>(responseContent);
                if (result.data.result)
                {
                    txtCompanyInfo.Text = result.data.data.com_name;
                    txtPhoneNumber.Text = result.data.data.com_phone;
                    txtDescriptipn.Text = result.data.data.com_description;
                    txtComSize.Text = result.data.data.com_size;
                    txtAddress.Text = result.data.data.com_address;
                    txtWebsite1.Text = result.data.data.com_email;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void explandmore_company_info_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (company_info_expland.Visibility == Visibility.Collapsed)
            {
                RotateTransform rotateTransform = new RotateTransform(0);
                company_info.RenderTransform = rotateTransform;
                company_info_expland.Visibility = Visibility.Visible;
                line_company_info.Visibility = Visibility.Hidden;
            }
            else
            {
                RotateTransform rotateTransform = new RotateTransform(-90);
                company_info.RenderTransform = rotateTransform;
                company_info_expland.Visibility = Visibility.Collapsed;
                line_company_info.Visibility = Visibility.Visible;
            }
        }

        private void notify_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (notify_expland.Visibility == Visibility.Collapsed)
            {
                RotateTransform rotateTransform = new RotateTransform(0);
                notify.RenderTransform = rotateTransform;
                notify_expland.Visibility = Visibility.Visible;
                line_notify.Visibility = Visibility.Hidden;
            }
            else
            {
                RotateTransform rotateTransform = new RotateTransform(-90);
                notify.RenderTransform = rotateTransform;
                notify_expland.Visibility = Visibility.Collapsed;
                line_notify.Visibility = Visibility.Visible;
            }
        }

        private void remind_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (remind_expland.Visibility == Visibility.Collapsed)
            {
                RotateTransform rotateTransform = new RotateTransform(0);
                remind.RenderTransform = rotateTransform;
                remind_expland.Visibility = Visibility.Visible;
                line_remind.Visibility = Visibility.Hidden;
            }
            else
            {
                RotateTransform rotateTransform = new RotateTransform(-90);
                remind.RenderTransform = rotateTransform;
                remind_expland.Visibility = Visibility.Collapsed;
                line_remind.Visibility = Visibility.Visible;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void editButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowHideEditInfo();
        }

        private void ShowHideEditInfo()
        {
            if (txtCompanyInfo.Visibility == Visibility.Visible)
            {
                txtCompanyInfo.Visibility = Visibility.Collapsed;
                txtPhoneNumber.Visibility = Visibility.Collapsed;
                txtDescriptipn.Visibility = Visibility.Collapsed;
                txtComSize.Visibility = Visibility.Collapsed;
                txtAddress.Visibility = Visibility.Collapsed;

                gridTbCompanyInfo1.Visibility = Visibility.Visible;
                gridTbCompanyInfo2.Visibility = Visibility.Visible;
                gridTbCompanyInfo3.Visibility = Visibility.Visible;
                gridBtn.Visibility = Visibility.Visible;
            }
            else
            {
                txtCompanyInfo.Visibility = Visibility.Visible;
                txtPhoneNumber.Visibility = Visibility.Visible;
                txtDescriptipn.Visibility = Visibility.Visible;
                txtComSize.Visibility = Visibility.Visible;
                txtAddress.Visibility = Visibility.Visible;

                gridTbCompanyInfo1.Visibility = Visibility.Collapsed;
                gridTbCompanyInfo2.Visibility = Visibility.Collapsed;
                gridTbCompanyInfo3.Visibility = Visibility.Collapsed;
                gridBtn.Visibility = Visibility.Collapsed;

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

        private void NavigateToUserRightsPage(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new UserRightsPage(token, comId, typeUser,epId));
                }
            }
        }

        private void ClickBtnCancel(object sender, MouseButtonEventArgs e)
        {
            ShowHideEditInfo();
        }

        private void ClickUpdateInfo(object sender, MouseButtonEventArgs e)
        {
            UpdateInfo();
        }

        private async void UpdateInfo()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.update_company_info;
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();
                //parameters.Add(new KeyValuePair<string, string>("id_com", comId));
                parameters.Add(new KeyValuePair<string, string>("userName", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("phone", tbPhone.Text));
                parameters.Add(new KeyValuePair<string, string>("address", tbAddress.Text));
                parameters.Add(new KeyValuePair<string, string>("com_size", tbComSize.Text));
                parameters.Add(new KeyValuePair<string, string>("description", tbDescription.Text));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ResultEntity result = JsonConvert.DeserializeObject<ResultEntity>(responseContent);
                if (result.data != null && result.data.result)
                {
                    ShowHideEditInfo();
                    GetInfoCompany();
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
