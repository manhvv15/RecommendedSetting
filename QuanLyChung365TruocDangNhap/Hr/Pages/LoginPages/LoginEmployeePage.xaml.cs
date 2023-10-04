using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.LoginPages
{
    /// <summary>
    /// Interaction logic for LoginEmployeePage.xaml
    /// </summary>
    public partial class LoginEmployeePage : Page
    {
        LoginView loginView;
        private bool showPassword = false;
        public string otp_code;
        public string token;
        public LoginEmployeePage(LoginView loginView, string otp_code, string token)
        {
            InitializeComponent();
            this.loginView = loginView;
            if (Properties.Settings.Default.epAccount != string.Empty)
            {
                txtEmail.Text = Properties.Settings.Default.epAccount;
                cbxRemember.IsChecked = true;
            }

            if (Properties.Settings.Default.epPassword != string.Empty)
            {
                txtPassword.Password = Properties.Settings.Default.epPassword;
                cbxRemember.IsChecked = true;
            }
            this.otp_code = otp_code;
            this.token = token;
            this.DataContext = this;
        }

        public LoginEmployeePage()
        {
            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        //login công ty trở về selectAccoutPage
        private void Back(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        //login nhân viên
        private async Task Login()
        {
            btnLogin.Opacity = 0.5;
            var httpClient = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.login_employee_api;
            httpRequestMessage.RequestUri = new Uri(api);

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("account", txtEmail.Text));

            string passLogin = "";
            if (showPassword)
            {
                passLogin = txtPassword2.Text;
            }
            else
            {
                passLogin = txtPassword.Password;
            }
            parameters.Add(new KeyValuePair<string, string>("password", passLogin));
            parameters.Add(new KeyValuePair<string, string>("type", "2"));

            // Thiết lập Content
            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();

            LoginEmployeeEntity result = JsonConvert.DeserializeObject<LoginEmployeeEntity>(responseContent);


            if (result.data != null && result.data.result)
            {
                if (cbxRemember.IsChecked == true)
                {
                    Properties.Settings.Default.epAccount = txtEmail.Text;
                    Properties.Settings.Default.epPassword = passLogin;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.epAccount = string.Empty;
                    Properties.Settings.Default.epPassword = string.Empty;
                    Properties.Settings.Default.Save();
                }
                HomeView homeView = new HomeView(result);
                homeView.Show();
                loginView.Close();
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Tài khoản không được để trống!");
            }
            else if (txtPassword.Password.ToString() == "")
            {
                MessageBox.Show("Mật khẩu không được để trống!");
            }
            else if (!Regex.IsMatch(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                MessageBox.Show("Email không đúng định dạng!");
                btnLogin.Opacity = 1;
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng,vui lòng nhập lại!");
                btnLogin.Opacity = 1;
            }
        }
        //bất đồng bộ
        private async void Login(object sender, MouseButtonEventArgs e)
        {
            await Login();
        }

        //private void ShowHidePassWord(object sender, MouseButtonEventArgs e)
        //{
        //    if (!showPassword)
        //    {
        //        txtPassword2.Text = txtPassword.Password;
        //        iconPassword1.Visibility = Visibility.Collapsed;
        //        iconPassword2.Visibility = Visibility.Visible;
        //        txtPassword.Visibility = Visibility.Collapsed;
        //        txtPassword2.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        txtPassword.Password = txtPassword2.Text;
        //        iconPassword1.Visibility = Visibility.Visible;
        //        iconPassword2.Visibility = Visibility.Collapsed;
        //        txtPassword.Visibility = Visibility.Visible;
        //        txtPassword2.Visibility = Visibility.Collapsed;
        //    }

        //    showPassword = !showPassword;
        //}

        class Account
        {
            public string email { get; set; }

            public string pass { get; set; }

            public Account(string email, string pass)
            {
                this.email = email;
                this.pass = pass;
            }
        }

        private async void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await Login();
            }
        }

        //chuyển page quên mật khẩu
        private void ForgotPassword(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(LoginView))
                {
                    (window as LoginView).MainContent.Navigate(new ForgotPasswordPage(loginView, 1));
                }
            }
        }

        private void ClickToRegister(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://quanlychung.timviec365.vn/dang-ky-nhan-vien.html"));
            e.Handled = true;
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                txtPassword.SelectAll();
                txtPassword2.SelectAll();
            }
        }
    }
}
