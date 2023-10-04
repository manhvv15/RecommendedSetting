using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginWithPhone.xaml
    /// </summary>
    public partial class LoginWithPhone : Page
    {
        private LoginCom DataLoginCom { get; set; }
        private LoginEP DataLoginEP { get; set; }

        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex regexPhone = new Regex(@"^((09|03|07|08|05)+([0-9]{8})\b)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        public LoginWithPhone(LoginCom dataLoginCom)
        {
            InitializeComponent();
            DataLoginCom = dataLoginCom;
        }

        public LoginWithPhone(LoginEP dataLoginEP)
        {
            InitializeComponent();
            DataLoginEP = dataLoginEP;
        }

        private void LoginClick(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                lbWrongEmail.Text = "Email hoặc số điện thoại không được để trống!";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {

                if (txtEmail.Text.Contains("@")) { allow = false; lbWrongEmail.Text = "Email hoặc số điện thoại chưa đúng định dạng!"; }
                else if (!regexPhone.IsMatch(txtEmail.Text))
                {
                    allow = false;
                    lbWrongEmail.Text = "Email hoặc số điện thoại chưa đúng định dạng!";
                }
                else
                {
                    lbWrongEmail.Text = "";
                }
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://quanlychung.timviec365.vn/");
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbWrongEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                lbWrongEmail.Text = "Email hoặc số điện thoại không được để trống!";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                if (txtEmail.Text.Contains("@")) { lbWrongEmail.Text = "Email hoặc số điện thoại chưa đúng định dạng!"; }
                else if (!regexPhone.IsMatch(txtEmail.Text))
                {
                    lbWrongEmail.Text = "Email hoặc số điện thoại chưa đúng định dạng!";
                }
                else
                {
                    lbWrongEmail.Text = "";
                }
            }
        }

        private void returnLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataLoginCom != null)
            {
                DataLoginCom.FrameLogin.NavigationService.Navigate(null);
                /*DataLoginCom.bd_login.Visibility = Visibility.Visible;
                DataLoginCom.GrMoreOption.Visibility = Visibility.Collapsed;
                DataLoginCom.GrMoreOptionOpacity.Visibility = Visibility.Collapsed;*/
            }
            else
            {
                DataLoginEP.FrameLogin.NavigationService.Navigate(null);
                /*DataLoginEP.bd_login.Visibility = Visibility.Visible;
                DataLoginEP.GrMoreOption.Visibility = Visibility.Collapsed;
                DataLoginEP.GrMoreOptionOpacity.Visibility = Visibility.Collapsed;*/
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            lbWrongEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                lbWrongEmail.Text = "Email hoặc số điện thoại không được để trống!";
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                if (txtEmail.Text.Contains("@")) { lbWrongEmail.Text = "Email hoặc số điện thoại chưa đúng định dạng!"; }
                else if (!regexPhone.IsMatch(txtEmail.Text))
                {
                    lbWrongEmail.Text = "Email hoặc số điện thoại chưa đúng định dạng!";
                }
                else
                {
                    lbWrongEmail.Text = "";
                }
            }
        }
    }
}
