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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.HRReportPages.HRReportPages
{
    /// <summary>
    /// Interaction logic for NewStaffPage.xaml
    /// </summary>
    public partial class NewStaffPage : Page
    {
        public NewStaffPage()
        {
            InitializeComponent();
        }

        private void NavigateBack(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.NavigationService.GoBack();
                }
            }
        }

        private void ShowHideDepartment(object sender, MouseButtonEventArgs e)
        {
            if(CbxDepartment.Visibility == Visibility.Visible)
            {
                CbxDepartment.Visibility = Visibility.Collapsed;
            }
            else
            {
                CbxDepartment.Visibility = Visibility.Visible;
            }
        }
    }
}
