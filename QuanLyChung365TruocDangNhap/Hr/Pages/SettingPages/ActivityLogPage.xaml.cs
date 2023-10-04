using QuanLyChung365TruocDangNhap.Hr.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ActivityLogPage.xaml
    /// </summary>
    public partial class ActivityLogPage : Page, INotifyPropertyChanged
    {
        string token;
        int typeUser; // 1:nhân viên, 2:công ty
        string comId;
        string epId;
        private bool show;
        public bool Show
        {
            get { return show; }
            set
            {
                show = value;
                OnPropertyChanged("Show");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        public ActivityLogPage(string token, int typeUser, string comId, string epId)
        {
            InitializeComponent();
            this.token = token;
            this.typeUser = typeUser;
            this.comId = comId;
            this.epId = epId;
            if (this.typeUser == 1)
            {
                Show = false;
            }
            else
            {
                Show = true;
            }
            DataContext = this;
        }

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            String uri = "SettingPages/" + name + "Page";

            switch (name)
            {
                case "Setting":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new SettingPage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "Profile":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ProfilePage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "SecurityInformation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new SecurityInformationPage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "ActivityLog":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ActivityLogPage(token, typeUser, comId, epId));
                        }
                    };
                    break;
                case "ListMember":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ListMemberPage());
                        }
                    };
                    break;
            }
        }
    }
}
