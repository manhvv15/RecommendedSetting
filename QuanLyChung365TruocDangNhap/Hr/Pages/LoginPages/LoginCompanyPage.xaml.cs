using QuanLyChung365TruocDangNhap.Hr.Entities;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.LoginPages
{
    /// <summary>
    /// Interaction logic for LoginCompanyPage.xaml
    /// </summary>
    public partial class LoginCompanyPage : Page, INotifyPropertyChanged
    {
        LoginView loginView;

        private bool showPassword = false;
        string password = "12345678";
        public string otp_code;
        public string token;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
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

        public LoginCompanyPage(LoginView loginView, string otp_code,string token)
        {
            InitializeComponent();
            this.loginView = loginView;
            DataContext = this;

            if (Properties.Settings.Default.comAccount != string.Empty)
            {
                txtEmail.Text = Properties.Settings.Default.comAccount;
                cbxRemember.IsChecked = true;
            }

            if (Properties.Settings.Default.comPassword != string.Empty)
            {
                txtPassword.Password = Properties.Settings.Default.comPassword;
                cbxRemember.IsChecked = true;
            }
            this.otp_code = otp_code;
            this.token = token;
        }
        public LoginCompanyPage()
        {
            InitializeComponent();
        }
        //login công ty trở về selectAccoutPage
        private void Back(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        //login công ty
        private async Task Login()
        {
            btnLogin.Opacity = 0.5;
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.login_company_api;

            httpRequestMessage.RequestUri = new Uri(api);

            var parameter = new List<KeyValuePair<string, string>>();
            parameter.Add(new KeyValuePair<string, string>("account", txtEmail.Text));
            string passLogin = "";
            if (showPassword)
            {
                passLogin = txtPassword2.Text;
            }
            else
            {
                passLogin = txtPassword.Password;
            }
            parameter.Add(new KeyValuePair<string, string>("password", passLogin));
            parameter.Add(new KeyValuePair<string, string>("type", "1"));


            var content = new FormUrlEncodedContent(parameter);
            httpRequestMessage.Content = content;

            var response = await httpClient.SendAsync(httpRequestMessage);
            var reponseContent = await response.Content.ReadAsStringAsync();

            LoginCompanyEntity result = JsonConvert.DeserializeObject<LoginCompanyEntity>(reponseContent);
            if (result.data != null && result.data.result)
            {
                if (cbxRemember.IsChecked == true)
                {
                    Properties.Settings.Default.comAccount = txtEmail.Text;
                    Properties.Settings.Default.comPassword = passLogin;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.comAccount = string.Empty;
                    Properties.Settings.Default.comPassword = string.Empty;
                    Properties.Settings.Default.Save();
                }

                HomeView homeView = new HomeView(result);
                homeView.Show();
                loginView.Close();
            }
            else
            {
                if (!Regex.IsMatch(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    MessageBox.Show("Email không đúng định dạng!");
                }
                else if (!Regex.IsMatch(txtPassword.ToString(), "^(?=.+[A-Za-z])(?=.+\\d)(?=.+[$@$!%*#?&])[A-Za-z\\d$@$!%*#?&]{8,}$"))
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!");
                }
                btnLogin.Opacity = 1;
            }
        }



        //bất đồng bộ
        private async void Login(object sender, MouseButtonEventArgs e)
        {
            await Login();
        }

        private async void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
                    (window as LoginView).MainContent.Navigate(new ForgotPasswordPage(loginView,2));
                }
            }
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

        private void ClickToRegister(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://quanlychung.timviec365.vn/dang-ky-cong-ty.html"));
            e.Handled = true;
        }

        private void txtEmail_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                txtPassword.SelectAll();
                txtPassword2.SelectAll();
            }
        }
    }

}
