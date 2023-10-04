using QuanLyChung365TruocDangNhap.Hr.Login.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.Hr.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassword_Email.xaml
    /// </summary>
    public partial class ForgotPassword_NewPass : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ForgotPassword_NewPass(ForgotPasswordWindow win)
        {
            InitializeComponent();
            this.DataContext = this;
            WinForgot = win;

            txtNewPass.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(txtNewPass, new object[] { NewPass.Length, 0 });
            txtNewPass.Focus();
        }
        //
        public ForgotPasswordWindow WinForgot { get; set; }

        private string _NewPass = "";
        public string NewPass
        {
            get { return _NewPass; }
            set { _NewPass = value; OnPropertyChanged(); }
        }

        private string _RePass = "";
        public string RePass
        {
            get { return _RePass; }
            set { _RePass = value; OnPropertyChanged(); }
        }
        //
        private async void run()
        {
            var allow = true;
            tblValidateNewPass.Text = tblValidateRePass.Text = "";
            if (string.IsNullOrEmpty(NewPass))
            {
                allow = false;
                tblValidateNewPass.Text = "Không được để trống";
            }
            else if (NewPass.Length < 6)
            {
                allow = false;
                tblValidateNewPass.Text = "Vui lòng nhập mật khẩu có độ dài từ 6 ký tự và không chứa dấu cách";
            }
            else if (NewPass.Contains(" "))
            {
                allow = false;
                tblValidateNewPass.Text = "Vui lòng nhập mật khẩu có độ dài từ 6 ký tự và không chứa dấu cách";
            }
            if (NewPass != RePass)
            {
                allow = false;
                tblValidateRePass.Text = "nhập lại mật khẩu";
            }
            if (allow)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage respon;
                    API_Response api;
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("email", WinForgot.ForgotItem.Email);
                    form.Add("otp_code", WinForgot.ForgotItem.OTP);
                    form.Add("new_pass", NewPass);

                    switch (WinForgot.Type)
                    {
                        case 1:
                            respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/forget_pass_employee.php", new FormUrlEncodedContent(form));
                            api = JsonConvert.DeserializeObject<API_Response>(respon.Content.ReadAsStringAsync().Result);
                            if (api.data != null && api.data.result)
                            {
                                var z = new Views.Login.ForgotPassword_Success(WinForgot);
                                WinForgot.SelectionPage.NavigationService.Navigate(z);
                            }
                            else
                            {
                                var n = new Notify();
                                n.Type = Notify.NotifyType.Error;
                                if (api.error != null && api.error.message != null) n.setMessage(api.error.message);
                                else n.setMessage("");
                                WinForgot.ShowPopup(n);
                            }
                            break;
                        case 2:
                            respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/forget_pass_company.php", new FormUrlEncodedContent(form));
                            api = JsonConvert.DeserializeObject<API_Response>(respon.Content.ReadAsStringAsync().Result);
                            if (api.data != null && api.data.result)
                            {
                                var z = new Views.Login.ForgotPassword_Success(WinForgot);
                                WinForgot.SelectionPage.NavigationService.Navigate(z);
                            }
                            else
                            {
                                var n = new Notify();
                                n.Type = Notify.NotifyType.Error;
                                if (api.error != null && api.error.message != null) n.setMessage(api.error.message);
                                else n.setMessage("");
                                WinForgot.ShowPopup(n);
                            }
                            break;
                        default:
                            break;
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
        private void Continue(object sender, MouseButtonEventArgs e)
        {
            run();
        }

        private void txtNewPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPass = txtNewPass.Password;
        }

        private void txtRePass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RePass = txtRePass.Password;
        }

        private void txtNewPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                var allow = true;
                tblValidateNewPass.Text = tblValidateRePass.Text = "";
                if (string.IsNullOrEmpty(NewPass))
                {
                    allow = false;
                    tblValidateNewPass.Text = "Không được để trống";
                }
                else if (NewPass.Length < 6)
                {
                    allow = false;
                    tblValidateNewPass.Text = "Vui lòng nhập mật khẩu có độ dài từ 6 ký tự và không chứa dấu cách";
                }
                if (allow)
                {
                    txtRePass.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(txtRePass, new object[] { RePass.Length, 0 });
                    txtRePass.Focus();
                }
            }

        }

        private void txtRePass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) run();
        }
    }
}
