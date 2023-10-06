using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;
using QuanLyChung365TruocDangNhap.Hr.View;
using System.Net.Http;
using Newtonsoft.Json;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.LoginPages
{
    /// <summary>
    /// Interaction logic for ForgotPasswordPage.xaml
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        LoginView loginView;
        int type_user;
        public ForgotPasswordPage(LoginView loginView,int type_user)
        {
            InitializeComponent();
            this.loginView = loginView;
            this.type_user = type_user;
        }
        private async Task RetoreEmail()
        {
            btnRestore.Opacity = 0.5;
            var httpClient = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.send_otp_password_api;

            httpRequestMessage.RequestUri = new Uri(api);

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("email", txtEmail.Text));
            parameters.Add(new KeyValuePair<string, string>("type_user", type_user.ToString()));
            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            ForgotPassCompanyEntity result = JsonConvert.DeserializeObject<ForgotPassCompanyEntity>(responseContent);
            if (result.data != null && result.data.result)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(LoginView))
                    {
                        (window as LoginView).MainContent.Navigate(new OtpAuthentication(loginView,txtEmail.Text,type_user));
                    }
                }
                MessageBox.Show("Mã xác thực đã được gửi đến email của bạn.");
            }
            else if (txtEmail.Text == "") {
                MessageBox.Show("Email không được để trống");
            }
            else
            {
                messageError.Visibility = Visibility.Visible;
                btnRestore.Opacity = 1;
            }
        }
        private async void RetoreEmail(object sender, MouseButtonEventArgs e)
        {
            await RetoreEmail();
        }
        class Forgot
        {
            public string email { get; set; }

            public int type_user { get; set; }

            public Forgot(string email, int type_user)
            {
                this.email = email;
                this.type_user = type_user;
            }
        }
        //Trở lại view login
        private void Back(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        //ẩn hiển validate khôi phục email
        private void RetoreEmail(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length == 0)
            {
                error.Visibility = Visibility.Visible;

            }
            else if (!Regex.IsMatch(txtEmail.Text, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                messageError.Visibility = Visibility.Visible;
            }
            else
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(LoginView))
                    {
                        (window as LoginView).MainContent.Navigate(new OtpAuthentication(loginView,txtEmail.Text,type_user));
                    }
                }
            }
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            error.Visibility = Visibility.Collapsed;
            messageError.Visibility = Visibility.Collapsed;
            btnRestore.Opacity = 2;
        }

        private void KU_Email(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                 RetoreEmail();
            }
        }
    }
}
