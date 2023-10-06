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
    /// Interaction logic for OtpAuthentication.xaml
    /// </summary>
    public partial class OtpAuthentication : Page
    {
        LoginView loginView;
        string email;
        int type_user;
        string otp_code;
        public OtpAuthentication(LoginView loginView,string email, int type_user)
        {
            InitializeComponent();
            this.loginView = loginView;
            this.email = email;
            this.type_user = type_user;
        }
        //nhập lại thì mất validate
        private void txtOtp_TextChanged(object sender, TextChangedEventArgs e)
        {
            error.Visibility = Visibility.Collapsed;
            messageError.Visibility = Visibility.Collapsed;
        }
        //Trở lại khôi phục email
        private void Back(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void ClickSendOtp(object sender, MouseButtonEventArgs e)
        {
            SendOtp();
        }

        private async void SendOtp()
        {
            var httpClient = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.send_otp_password_api;

            httpRequestMessage.RequestUri = new Uri(api);

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("email", email));
            parameters.Add(new KeyValuePair<string, string>("type_user", type_user.ToString()));
            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            ForgotPassCompanyEntity result = JsonConvert.DeserializeObject<ForgotPassCompanyEntity>(responseContent);
            if (result.data != null && result.data.result)
            {
                MessageBox.Show("Mã xác thực đã được gửi đến email của bạn.");
            }
            else
            {
                //messageError1.Visibility = Visibility.Collapsed;
                MessageBox.Show("Mã OTP nhập vào không đúng, vui lòng nhập lại");
                btnConfirmVerifyOtp.Opacity = 1;
            }
        }
        private void KU_ConfirmVerifyOtp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ClickVerifyOtp();
            }
        }
        private async void ClickVerifyOtp()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.verify_otp;

                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("email", email));
                parameters.Add(new KeyValuePair<string, string>("type_user", type_user.ToString()));
                parameters.Add(new KeyValuePair<string, string>("otp_code", txtOtp.Text));
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                verifyOtp result = JsonConvert.DeserializeObject<verifyOtp>(responseContent);
                if (result.data != null)
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(LoginView))
                        {
                            (window as LoginView).MainContent.Navigate(new RestorePasswordPage(loginView,email,type_user,txtOtp.Text,otp_code));
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Mã OTP nhập vào không đúng, vui lòng nhập lại");
                btnConfirmVerifyOtp.Opacity = 1;
            }
        }
        private void SubmitConfirmVerifyOtp(object sender, MouseButtonEventArgs e)
        {
            ClickVerifyOtp();
        }
        //chỉ nhập số vào mã xác thực
        private void txtOtp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            //TextBox txtcode = sender as TextBox;
            //if (txtcode.Text.Length == 6)
            //{
            //    txtcode.Text = txtcode.Text.Remove(0, 1);
            //    txtcode.CaretIndex = txtcode.Text.Length;
            //}
        }
        private async void txtOTp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                ClickVerifyOtp();
            }

        }
    }
}
