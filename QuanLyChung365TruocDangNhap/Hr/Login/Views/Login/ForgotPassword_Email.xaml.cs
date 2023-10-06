using QuanLyChung365TruocDangNhap.Hr.Login.Entities;
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

namespace QuanLyChung365TruocDangNhap.Hr.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassword_Email.xaml
    /// </summary>
    public partial class ForgotPassword_Email : Page
    {
        public ForgotPassword_Email(ForgotPasswordWindow win)
        {
            InitializeComponent();
            this.DataContext = this;
            WinForgot = win;
            txtEmail.Focus();
        }
        //
        public ForgotPasswordWindow WinForgot { get; set; }

        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        //
        private async void run()
        {
            bool allow = true;
            tblValidateEmail.Text ="";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Nhập không đúng định dạng email";
            }
            if (allow)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("email", txtEmail.Text);
                    form.Add("type_user", WinForgot.Type.ToString());

                    HttpClient httpClient = new HttpClient();
                    var respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/send_otp.php", new FormUrlEncodedContent(form));
                    API_Response api = JsonConvert.DeserializeObject<API_Response>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null && api.data.result)
                    {
                        WinForgot.ForgotItem.Email = txtEmail.Text;
                        var z = new Views.Login.ForgotPassword_OTP(WinForgot);
                        WinForgot.SelectionPage.NavigationService.Navigate(z);
                    }
                    else
                    {
                        var n = new Notify();
                        n.Type = Notify.NotifyType.Error;
                        if(api.error!=null && api.error.message!=null)n.setMessage(api.error.message);
                        else n.setMessage("");
                        WinForgot.ShowPopup(n);
                    }
                }
                catch (Exception ex)
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Error;
                    n.setMessage(ex.Message);
                    WinForgot.ShowPopup(n);
                }
            }
        }
        //
        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            tblValidateEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                tblValidateEmail.Text = "Nhập không đúng định dạng email";
            }
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            tblValidateEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                tblValidateEmail.Text = "Nhập không đúng định dạng email";
            }
        }
        private void Continue(object sender, MouseButtonEventArgs e)
        {
            run();
        }
        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            WinForgot.Hide();

            WinForgot.WinLogin.Width=WinForgot.ActualWidth;
            WinForgot.WinLogin.Height=WinForgot.ActualHeight;
            WinForgot.WinLogin.Left=WinForgot.Left;
            WinForgot.WinLogin.Top=WinForgot.Top;
            WinForgot.WinLogin.WindowState = WinForgot.WindowState;

            WinForgot.WinLogin.Show();
        }
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                run();
            }
        }
    }
}
