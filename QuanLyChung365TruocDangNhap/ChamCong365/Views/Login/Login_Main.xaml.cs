using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for Login_Main.xaml
    /// </summary>
    public partial class Login_Main : Page
    {
        public Login_Main(LoginMain _login)
        {
            InitializeComponent();
            MainLogin = _login;
            MainStaff = _login;
        }

        private LoginMain MainLogin;
        private LoginMain MainStaff;

        private void LoginCompany(object sender, MouseButtonEventArgs e)
        {
            MainLogin.LoginType = 2;
            MainLogin.LoginSlectionPage.NavigationService.Navigate(new Views.Login.Login_Company(MainLogin));
        }
        
        private void LoginStaff(object sender, MouseButtonEventArgs e)
        {
            MainLogin.LoginType = 1;
            MainLogin.LoginSlectionPage.NavigationService.Navigate(new Views.Login.Login_Staff(MainStaff));
        }
        private void Register_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://quanlychung.timviec365.vn/lua-chon-dang-ky.html");
        }
    }
}
