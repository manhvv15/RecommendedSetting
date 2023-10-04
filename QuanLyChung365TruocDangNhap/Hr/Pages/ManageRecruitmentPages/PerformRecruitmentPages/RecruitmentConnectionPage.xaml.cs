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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.PerformRecruitmentPages
{
    /// <summary>
    /// Interaction logic for RecruitmentConnectionPage.xaml
    /// </summary>
    public partial class RecruitmentConnectionPage : Page
    {
        public RecruitmentConnectionPage()
        {
            InitializeComponent();
        }

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "PerformRecruitment":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PerformRecruitmentPage());
                        }
                    };
                    break;
                case "RecruitmentInformation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentInformationPage());
                        }
                    };
                    break;
                case "RecruitmentConnection":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentConnectionPage());
                        }
                    };
                    break;
                case "RecruitmentFee":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentFeePage());
                        }
                    };
                    break;
            }
        }
    }
}
