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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages
{
    /// <summary>
    /// Interaction logic for ListMemberPage.xaml
    /// </summary>
    public partial class ListMemberPage : Page
    {
        public ListMemberPage()
        {
            InitializeComponent();
        }

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            String uri = "SettingPages/" + name + "Page";

            //switch (name)
            //{
            //    case "Setting":
            //        foreach (Window window in Application.Current.Windows)
            //        {
            //            if (window.GetType() == typeof(HomeView))
            //            {
            //                (window as HomeView).MainContent.Navigate(new SettingPage());
            //            }
            //        };
            //        break;
            //    case "Profile":
            //        foreach (Window window in Application.Current.Windows)
            //        {
            //            if (window.GetType() == typeof(HomeView))
            //            {
            //                (window as HomeView).MainContent.Navigate(new ProfilePage());
            //            }
            //        };
            //        break;
            //    case "SecurityInformation":
            //        foreach (Window window in Application.Current.Windows)
            //        {
            //            if (window.GetType() == typeof(HomeView))
            //            {
            //                (window as HomeView).MainContent.Navigate(new SecurityInformationPage());
            //            }
            //        };
            //        break;
            //    case "ActivityLog":
            //        foreach (Window window in Application.Current.Windows)
            //        {
            //            if (window.GetType() == typeof(HomeView))
            //            {
            //                (window as HomeView).MainContent.Navigate(new ActivityLogPage());
            //            }
            //        };
            //        break;
            //    case "ListMember":
            //        foreach (Window window in Application.Current.Windows)
            //        {
            //            if (window.GetType() == typeof(HomeView))
            //            {
            //                (window as HomeView).MainContent.Navigate(new ListMemberPage());
            //            }
            //        };
            //        break;
            //}
        }
    }
}
