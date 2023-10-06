using QuanLyChung365TruocDangNhap.Hr.View;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.LoginPages
{
    /// <summary>
    /// Interaction logic for RestorePasswordSuccessPage.xaml
    /// </summary>
    public partial class RestorePasswordSuccessPage : Page
    {
        LoginView loginView;
        public string token;
        public RestorePasswordSuccessPage(LoginView loginView)
        {
            InitializeComponent();
            this.loginView = loginView;
        }
        public RestorePasswordSuccessPage()
        {
            InitializeComponent();
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            //if (this.NavigationService.CanGoBack)
            //{
            //    this.NavigationService.GoBack();
            //}
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(LoginView))
                {
                    (window as LoginView).MainContent.Navigate(new SelectAccountPage(loginView, token));
                }
            }
        }
        private void SubmitBackLogin(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(LoginView))
                {
                    (window as LoginView).MainContent.Navigate(new SelectAccountPage(loginView,token));
                }
            }
        }
    }
}
