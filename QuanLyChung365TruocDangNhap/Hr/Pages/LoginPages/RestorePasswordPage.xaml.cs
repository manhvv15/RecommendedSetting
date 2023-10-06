using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RestorePasswordPage.xaml
    /// </summary>
    public partial class RestorePasswordPage : Page
    {
        LoginView loginView;
        string token;
        string email;
        string otp_code;
        private bool showPassword1 = false;
        private bool showPassword = false;
        int type_user;
        public RestorePasswordPage(LoginView loginView, string email, int type_user, string otp_code, string token)
        {
            InitializeComponent();
            this.email = email;
            this.loginView = loginView;
            this.type_user = type_user;
            this.otp_code = otp_code;
            this.token = token;
        }

        private async void VerifyOtp()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.verify_otp;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", token);
                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("email", email));
                parameters.Add(new KeyValuePair<string, string>("type_user", type_user.ToString()));
                parameters.Add(new KeyValuePair<string, string>("otp_code", otp_code));
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                verifyOtp result = JsonConvert.DeserializeObject<verifyOtp>(responseContent);
                if (result.data != null)
                {
                    MessageBox.Show("Mã xác thực công ty đúng!");
                }

            }
            catch (Exception)
            {
            }
        }


        private async void SubmitConfirm()
        {
            btnConfirm.Opacity = 0.5;
            var httpClient = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api;
            if (type_user == 1)
            {
                api = APIs.API.forgot_password_employee_api;

            }
            else
            {
                api = APIs.API.forgot_password_company_api;
            }
            httpRequestMessage.RequestUri = new Uri(api);

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("otp_code", otp_code));
            if (showPassword)
            {
                parameters.Add(new KeyValuePair<string, string>("new_pass", txtPassword.Text));
            }
            else
            {
                parameters.Add(new KeyValuePair<string, string>("new_pass", txtPass.Password));
            }
            parameters.Add(new KeyValuePair<string, string>("email", email));

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
                        (window as LoginView).MainContent.Navigate(new RestorePasswordSuccessPage(loginView));
                    }
                }
            }
            else
            {
                //messageError1.Visibility = Visibility.Collapsed;
                //MessageBox.Show("Mã OTP nhập vào không đúng, vui lòng nhập lại");
                btnConfirm.Opacity = 1;
            }
        }
        //Ẩn hiện validate xác thực mã và nhập lại mật khẩu
        private void SubmitConfirm(object sender, MouseButtonEventArgs e)
        {
            
            if(ValidateForm())
                SubmitConfirm();
            //error.Visibility = Visibility.Visible;
            //error1.Visibility = Visibility.Visible;
            //MessageBox.Show("Vui lòng nhập mã OTP.");
        }

        private bool ValidateForm()
        {
            if (txtPass.Password.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được để trống!");
                return false;
            }
            if (txtPass.Password.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được để khoảng trắng!");
                return false;
            }

            if (txtPass.Password.Length < 8 )
            {
                MessageBox.Show("Mật khẩu phải tối thiểu 8 ký tự!");
                return false;
            }



            if (txtConfigPass.Password.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được để trống!");
                return false;
            }
            if (txtConfigPass.Password.Trim() == "")
            {
                MessageBox.Show("Mật khẩu không được để khoảng trắng!");
                return false;
            }

            string str1 = txtPass.Password.TrimStart();
            string str2 = txtConfigPass.Password.TrimEnd();

            if (str1 != str2)
            {
                MessageBox.Show("Mật khẩu nhập lại không trùng khớp hoặc đang chứa khoảng trắng,vui lòng nhập lại!");
                return false;
            }
            if (txtPass.Password.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có tối thiểu 8 ký tự bao gồm tối thiểu 1 chữ, 1 số,1 ký tự đặc biệt và không chứa dấu cách");
                return false;
            }
            return true;
        }
        //cofigPass
        //private void TextBoxCofigPass_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    error1.Visibility = Visibility.Collapsed;
        //    messageError1.Visibility = Visibility.Collapsed;
        //}



        private void ShowHidePassWord1(object sender, MouseButtonEventArgs e)
        {
            if (!showPassword1)
            {
                txtPassword2.Text = txtConfigPass.Password;
                iconPassword1.Visibility = Visibility.Collapsed;
                iconPassword2.Visibility = Visibility.Visible;
                txtConfigPass.Visibility = Visibility.Collapsed;
                txtPassword2.Visibility = Visibility.Visible;
            }
            else
            {
                txtConfigPass.Password = txtPassword2.Text;
                iconPassword1.Visibility = Visibility.Visible;
                iconPassword2.Visibility = Visibility.Collapsed;
                txtConfigPass.Visibility = Visibility.Visible;
                txtPassword2.Visibility = Visibility.Collapsed;
            }

            showPassword1 = !showPassword1;
        }

        private void ShowHidePassWord(object sender, MouseButtonEventArgs e)
        {
            if (!showPassword)
            {
                txtPassword.Text = txtPass.Password;
                iconPassword3.Visibility = Visibility.Collapsed;
                iconPassword4.Visibility = Visibility.Visible;
                txtPass.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
            }
            else
            {
                txtPass.Password = txtPassword.Text;
                iconPassword3.Visibility = Visibility.Visible;
                iconPassword4.Visibility = Visibility.Collapsed;
                txtPass.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
            }

            showPassword = !showPassword;
        }



        private void KU_Confirm(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidateForm();
            }
        }

        //Trở lại khôi phục email
        private void Back(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

    }
}
