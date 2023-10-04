using System;
using System.Collections.Generic;
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
    /// Interaction logic for PasswordChangedSucces.xaml
    /// </summary>
    public partial class PasswordChangedSucces : Page
    {
        public PasswordChangedSucces(LoginMain login)
        {
            InitializeComponent();
            this.DataContext = this;
            Login = login;
        }
        private LoginMain Login;
        private void Login_Main(object sender, MouseButtonEventArgs e)
        {
            Login.LoginSlectionPage.NavigationService.Navigate(new Login_Main(Login));
        }

        private void go_Back(object sender, MouseButtonEventArgs e)
        {
            switch (Login.LoginType)
            {
                case 1:
                    Login.LoginSlectionPage.NavigationService.Navigate(new Login_Staff(Login));
                    break;
                case 2:
                    Login.LoginSlectionPage.NavigationService.Navigate(new Login_Company(Login));
                    break;
                default:
                    break;
            }

        }
    }
}
