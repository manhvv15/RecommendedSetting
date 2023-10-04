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
    /// Interaction logic for SelectAccountPage.xaml
    /// </summary>
    public partial class SelectAccountPage : Page
    {
        LoginView loginView;
        public string otp_code;
        public string token;
        public SelectAccountPage(LoginView loginView,string token)
        {
            InitializeComponent();
            this.token = token;
            this.loginView = loginView;
        }
        public SelectAccountPage()
        {
            InitializeComponent();
        }
        //click đăng nhập công ty
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(LoginView))
                {
                    (window as LoginView).MainContent.Navigate(new LoginCompanyPage(loginView,token,otp_code));
                }
            }
        }
        //click đăng nhập nhân viên
        private void employee_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(LoginView))
                {
                    (window as LoginView).MainContent.Navigate(new LoginEmployeePage(loginView,token,otp_code));
                }
            }
        }

		//private void Border_MouseEnter(object sender, MouseEventArgs e)
		//{
		//var converter = new BrushConverter();
		//Border border = sender as Border;
		//border.Background = (Brush)converter.ConvertFromString("#BADBFF");
		//}

		//private void Border_MouseLeave(object sender, MouseEventArgs e)
		//{
		//var converter = new BrushConverter();
		//Border border = sender as Border;
		//border.Background = (Brush)converter.ConvertFromString("#FFFFFF");
		//}
	}
}
