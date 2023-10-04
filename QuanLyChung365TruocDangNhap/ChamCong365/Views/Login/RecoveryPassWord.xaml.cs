using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Forgot_Password;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Login
{
    /// <summary>
    /// Interaction logic for RecoveryPassWord.xaml
    /// </summary>
    public partial class RecoveryPassWord : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RecoveryPassWord(LoginMain login, string email, string otp_Code)
        {
            InitializeComponent();
            this.DataContext = this;
            Login = login;
            Email = email;
            Otp_Code = otp_Code;
        }
        private LoginMain Login;
        private string Email;
        private string Otp_Code;
        //nhập mật khẩu mới
        private string _Password ="";
        public string Password
        {
            get { return _Password; }
            set {
                _Password = value;
                OnPropertyChanged();
            }
        }
        private bool _showPassword = false;
        public bool showPassword
        {
            get { return _showPassword; }
            set {
                _showPassword = value;
                OnPropertyChanged();
            }
        }
        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {

            showPassword = !showPassword;
            if (showPassword)
            {
                txtEmailPassWord.Focus();
                txtEmailPassWord.SelectionStart = txtEmailPassWord.Text.Length;
            }
            else
            {
                showPassword = false;
                passboxEmailPassWord.Password = Password;
                passboxEmailPassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxEmailPassWord, new object[] { Password.Length, 0 });
                passboxEmailPassWord.Focus();
            }
        }
        private void InputPassword(object sender, RoutedEventArgs e)
        {
            Password = passboxEmailPassWord.Password;
        }

        //Nhập lại mật khẩu mới
        private string _RePassword = "";
        public string RePassword
        {
            get { return _RePassword; }
            set { _RePassword = value; OnPropertyChanged(); }
        }
        private bool _showRePassword = false;
        public bool showRePassword
        {
            get { return _showRePassword; }
            set { _showRePassword = value; OnPropertyChanged(); }
        }
        private void ShowRePassword(object sender, MouseButtonEventArgs e)
        {

            showRePassword = !showRePassword;
            if (showRePassword)
            {
                txtEmailRePassWord.Focus();
                txtEmailRePassWord.SelectionStart = txtEmailRePassWord.Text.Length;
            }
            else
            {
                showRePassword = false;
                passboxReEmailPassWord.Password = RePassword;
                passboxReEmailPassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxReEmailPassWord, new object[] { RePassword.Length, 0 });
                passboxReEmailPassWord.Focus();
            }
        }
        private void InputRePassword(object sender, RoutedEventArgs e)
        {
            RePassword = passboxReEmailPassWord.Password;
        }

        private void Login_Main(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new Login_Main(Login));
        }
        //private void Login_Succesfull(object sender, MouseButtonEventArgs e)
        //{
        //    if(RePassword == Password)
        //    {
        //        Login.LoginSlectionPage.NavigationService.Navigate(new PasswordChangedSucces(Login));
        //    }
        //    else
        //    {
        //        MessageBox.Show("Mật khẩu vừa nhập chưa chính xác vui lòng nhập lại");
                
        //    }
        //}

        private async void MouseLeftButtonDown_ChangedPass(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblNewPasswordValidate.Text= tblReNewPasswordValidate.Text = "";
            if (string.IsNullOrEmpty(txtEmailPassWord.Text))
            {
                allow = false;
                tblNewPasswordValidate.Text = "Mật khẩu mới không được để trống";
            }else if (txtEmailPassWord.Text.Length < 8)
            {
                allow = false;
                tblNewPasswordValidate.Text = "Mật khẩu mới không được ít hơn 8 ký tự";
            }
            if (string.IsNullOrEmpty(txtEmailRePassWord.Text))
            {
                allow = false;
                tblReNewPasswordValidate.Text = "Nhập lại mật khẩu không được để trống";
            }
            else if (txtEmailRePassWord.Text.Length < 8)
            {
                allow = false;
                tblReNewPasswordValidate.Text = "Mật khẩu mới không được ít hơn 8 ký tự";
            }
            else if(txtEmailPassWord.Text != txtEmailRePassWord.Text) {
                allow = false;
                tblReNewPasswordValidate.Text = "Nhập lại mật khẩu chưa đúng"; 
            }
            if (allow)
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("new_pass", Password);
                form.Add("email", Email);
                form.Add("otp_code", Otp_Code);
                HttpClient http = new HttpClient();
                HttpResponseMessage respon = await http.PostAsync("https://chamcong.24hpay.vn/service/forget_pass_employee.php", new FormUrlEncodedContent(form));
                API_Porgot_Password api = JsonConvert.DeserializeObject<API_Porgot_Password>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    Login.LoginSlectionPage.NavigationService.Navigate(new PasswordChangedSucces(Login));
                }
                else
                {
                    MessageBox.Show(api.error.message);
                }
            }
            
        }
    }
}
