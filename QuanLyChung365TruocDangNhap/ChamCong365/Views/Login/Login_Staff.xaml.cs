using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff;
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
    /// Interaction logic for Login_Staff.xaml
    /// </summary>
    public partial class Login_Staff : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Login_Staff(LoginMain login)
        {
            InitializeComponent();
            this.DataContext = this;
            Login = login;
            if (Properties.Settings.Default.RememberPassEP)
            {
                txtEmail.Text = Properties.Settings.Default.EmailEp;
                passboxEmailPassWord.Password = Properties.Settings.Default.PassEp;
                ckSave.IsChecked = true;
            }

        }
        private LoginMain Login;

        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex regexSDT = new Regex(@"^((09|03|07|08|05|04)+([0-9]{8})\b)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
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
        //private LoginMain StaffLogin;
        private bool _ShowPass = false;
        public bool ShowPass
        {
            get { return _ShowPass; }
            set { _ShowPass = value; OnPropertyChanged(); }
        }
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
        private void Forgot_PassWord(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new ForgotPassword(Login));
        }
        private void Login_Main(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new Login_Main(Login));
        }

        private async void LoginStaff(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblValidateEmail.Text = "";
            tblValidateEmailPassword.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Email không được bỏ trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Vui lòng nhập đúng định dạng";
            }
            else
            {
                tblValidateEmail.Text = "";
            }
            if (string.IsNullOrEmpty(txtEmailPassWord.Text))
            {
                allow = false;
                tblValidateEmailPassword.Text = "Mật khẩu không được bỏ trống";
            }
            else if (txtEmailPassWord.Text.Length < 4)
            {
                allow = false;
                tblValidateEmailPassword.Text = "Mật không không được ít hơn 8 ký tự";
            }
            else
            {
                tblValidateEmailPassword.Text = "";
            }
            if (allow)
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("account", txtEmail.Text);
                form.Add("password", txtEmailPassWord.Text);
                form.Add("type", "2");

                HttpClient http = new HttpClient();
                HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                API_Login_Staff api = JsonConvert.DeserializeObject<API_Login_Staff>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    if (ckSave.IsChecked == true)
                    {
                        Properties.Settings.Default.EmailEp = txtEmail.Text;
                        Properties.Settings.Default.PassEp = Password;
                        Properties.Settings.Default.RememberPassEP = true;
                    }
                    else
                    {
                        Properties.Settings.Default.EmailEp = "";
                        Properties.Settings.Default.PassEp = "";
                        Properties.Settings.Default.RememberPassEP = false;
                    }
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
        }

        private void Register_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://quanlychung.timviec365.vn/lua-chon-dang-ky.html");
        }

        private void ckSave_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.EmailEp = "";
            Properties.Settings.Default.PassEp = "";
            Properties.Settings.Default.RememberPassEP = false;
            Properties.Settings.Default.Save();
        }
    }
}
