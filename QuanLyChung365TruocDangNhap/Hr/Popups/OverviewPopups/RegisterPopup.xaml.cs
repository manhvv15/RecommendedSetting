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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.OverviewPopups
{
    /// <summary>
    /// Interaction logic for RegisterPopup.xaml
    /// </summary>
    public partial class RegisterPopup : UserControl
    {
        public delegate void HidePopup();

        public static event HidePopup hidePopup;

        public RegisterPopup()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, MouseButtonEventArgs e)
        {
            hidePopup();
        }

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            hidePopup();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    window.Close();
                }
            }
        }
    }
}
