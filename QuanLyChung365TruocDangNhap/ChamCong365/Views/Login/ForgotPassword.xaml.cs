using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Forgot_Password;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Page
    {
        public ForgotPassword(LoginMain login)
        {
            InitializeComponent();
            Login = login;
            this.DataContext = this;
        }
        //
        private LoginMain Login;
        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        private void Login_Main(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new Login_Main(Login));
        }

        private void ComeBack(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.GoBack();
        }

        private async void Continue(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Email không được bỏ trống!";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Vui lòng nhập đúng định dạng của Email!";
            }
            else
            {

                tblValidateEmail.Text = "";
            }
            if (allow)
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("email", txtEmail.Text);
                form.Add("type_user", Login.LoginType.ToString());
                HttpClient http = new HttpClient();
                HttpResponseMessage respon = await http.PostAsync("https://chamcong.24hpay.vn/service/send_otp.php", new FormUrlEncodedContent(form));
                API_Porgot_Password api = JsonConvert.DeserializeObject<API_Porgot_Password>(respon.Content.ReadAsStringAsync().Result);
                switch (Login.LoginType)
                {
                    case 1:
                        
                        if (api.data != null && api.data.result == true)
                        {
                            
                            Login.LoginSlectionPage.NavigationService.Navigate(new Verify_OTP(Login,txtEmail.Text));
                        }
                        else
                        {
                            tblValidateEmail.Text = api.error.message;
                        }
                        break;
                    case 2:
                        if (api.data != null && api.data.result == true)
                        {
                            Login.LoginSlectionPage.NavigationService.Navigate(new Verify_OTP(Login,txtEmail.Text));
                        }
                        else
                        {
                            tblValidateEmail.Text = api.error.message;
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
