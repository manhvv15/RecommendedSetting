using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Forgot_Password;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Login
{
    /// <summary>
    /// Interaction logic for Verify_OTP.xaml
    /// </summary>
    public partial class Verify_OTP : Page
    {
        public Verify_OTP(LoginMain login, string email)
        {
            InitializeComponent();
            this.DataContext = this;
            Login = login;

            Email = email;
        }

        private LoginMain Login;
        private List<TextBox> listTxt;

        private string Email;
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
            if (string.IsNullOrEmpty(txt1.Text) || string.IsNullOrEmpty(txt2.Text) || string.IsNullOrEmpty(txt3.Text) || string.IsNullOrEmpty(txt4.Text) || string.IsNullOrEmpty(txt5.Text) || string.IsNullOrEmpty(txt6.Text))
            {
                allow = false;
                tblValidate_OTP.Text = "OTP không được bỏ trống";
            }
            else
            {
                tblValidate_OTP.Text = "";
            }
            if (allow)
            {
                string otp = "";
                listTxt.ForEach(i => otp += i.Text);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("email", Email);
                form.Add("type_user", Login.LoginType.ToString());
                form.Add("otp_code", otp);
                HttpClient http = new HttpClient();
                HttpResponseMessage respon = await http.PostAsync("https://chamcong.24hpay.vn/service/verify_otp.php", new FormUrlEncodedContent(form));
                API_Porgot_Password api = JsonConvert.DeserializeObject<API_Porgot_Password>(respon.Content.ReadAsStringAsync().Result);
                switch (Login.LoginType)
                {
                    case 1:

                        if (api.data != null && api.data.result == true)
                        {

                            Login.LoginSlectionPage.NavigationService.Navigate(new RecoveryPassWord(Login, Email, otp));
                        }
                        else
                        {
                            tblValidate_OTP.Text = api.error.message;
                        }
                        break;
                    case 2:
                        if (api.data != null && api.data.result == true)
                        {

                            Login.LoginSlectionPage.NavigationService.Navigate(new RecoveryPassWord(Login, Email, otp));
                        }
                        else
                        {
                            tblValidate_OTP.Text = api.error.message;
                        }
                        break;
                    default:
                        break;

                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listTxt = new List<TextBox> { txt1, txt2, txt3, txt4, txt5, txt6 };
            listTxt[0].Focus();
        }

        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (listTxt != null)
                if (txt != listTxt[listTxt.Count - 1] && !string.IsNullOrEmpty(txt.Text))
                {
                    var index = listTxt.FindIndex(i => i == txt);
                    listTxt[index + 1].Focus();
                }
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != listTxt[0] && e.Key == Key.Back && string.IsNullOrEmpty(txt.Text))
            {
                listTxt[listTxt.IndexOf(txt) - 1].Focus();
            }
        }

        //Re_Send otp code
        private async void ReSend_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("email", Email);
            form.Add("type_user", Login.LoginType.ToString());
            HttpClient http = new HttpClient();
            HttpResponseMessage respon = await http.PostAsync("https://chamcong.24hpay.vn/service/send_otp.php", new FormUrlEncodedContent(form));
            API_Porgot_Password api = JsonConvert.DeserializeObject<API_Porgot_Password>(respon.Content.ReadAsStringAsync().Result);
            switch (Login.LoginType)
            {
                case 1:
                    if (api.data != null && api.data.result == true)
                    {
                        MessageBox.Show("Vui lòng check lại email!");
                    }
                    else
                    {
                        MessageBox.Show(" api.error.message");
                    }
                    break;
                case 2:
                    if (api.data != null && api.data.result == true)
                    {
                        MessageBox.Show("Vui lòng check lại email!");
                    }
                    else
                    {
                        MessageBox.Show(" api.error.message");
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
