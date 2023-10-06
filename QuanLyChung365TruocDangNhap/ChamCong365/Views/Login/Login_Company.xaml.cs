using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
    /// Interaction logic for Login_CompanyNoSavePassWord.xaml
    /// </summary>
    public partial class Login_Company : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Login_Company(LoginMain login)
        {
            InitializeComponent();
            this.DataContext = this;
            Login = login;
            txtEmail.Text = Properties.Settings.Default.Email;
            if (Properties.Settings.Default.RememberPass)
            {
                passboxEmailPassWord.Password = Properties.Settings.Default.Password;
            }
            CheckBoxRemember.IsChecked = true;
        }
        private LoginMain Login;

        private string _Password = "";
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }

        private bool _ShowPass = false;
        public bool ShowPass
        {
            get { return _ShowPass; }
            set { _ShowPass = value; OnPropertyChanged(); }
        }

        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);


        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {
            ShowPass = !ShowPass;
            if (ShowPass)
            {
                txtEmailPassWord.Focus();
                txtEmailPassWord.SelectionStart = txtEmailPassWord.Text.Length;
            }
            else
            {
                ShowPass = false;
                passboxEmailPassWord.Password = Password;
                passboxEmailPassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxEmailPassWord, new object[] { Password.Length, 0 });
                passboxEmailPassWord.Focus();
            }
        }
        private void InputPassword(object sender, RoutedEventArgs e)
        {
            Password = passboxEmailPassWord.Password;

        }
        private void Login_Main(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new Login_Main(Login));
        }
        private void Forgot_Password(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new ForgotPassword(Login));
        }
        private async void LoginCompany(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblValidateEmail.Text = "";
            tblValidateEmailPassword.Text = "";
            //validate EmailBox
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Email không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Không đúng định dạng";
            }
            else
            {
                tblValidateEmail.Text = "";
            }
            //validate password
            if (string.IsNullOrEmpty(txtEmailPassWord.Text))
            {
                allow = false;
                tblValidateEmailPassword.Text = "Mật khẩu không được để trống";
            }
            //else if (passboxEmailPassWord.Password.Length < 8)
            //{
            //    allow = false;
            //    tblValidateEmailPassword.Text = "không được ít hơn 8 ký tự";
            //}
            else
            {
                tblValidateEmailPassword.Text = "";
            }
            if (allow)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("email", txtEmail.Text);
                    form.Add("pass", Password);
                    HttpClient http = new HttpClient();
                    HttpResponseMessage respon = await http.PostAsync("https://chamcong.24hpay.vn/service/login_company.php", new FormUrlEncodedContent(form));
                    API_Login_Company api = JsonConvert.DeserializeObject<API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null && api.data.result == true)
                    {
                        if (CheckBoxRemember.IsChecked == true)
                        {
                            Properties.Settings.Default.Password = Password;
                            Properties.Settings.Default.RememberPass = true;
                        }
                        else
                        {
                            Properties.Settings.Default.Password = string.Empty;
                            Properties.Settings.Default.RememberPass = false;
                        }
                        Properties.Settings.Default.Email = txtEmail.Text;
                        Properties.Settings.Default.Save();

                        MainWindow home = new MainWindow(api);
                        home.WindowState = Login.WindowState;
                        Login.Hide();
                        var workingArea = System.Windows.SystemParameters.WorkArea;
                        home.Width = workingArea.Right - 100;
                        home.Height = workingArea.Bottom - 80;
                        home.Show();
                    }
                    else
                    {
                        tblValidateEmailPassword.Text = api.error.message;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Register(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://quanlychung.timviec365.vn/lua-chon-dang-ky.html");
        }
    }
}
