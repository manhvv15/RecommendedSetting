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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.popUp_MainWindow
{
    /// <summary>
    /// Interaction logic for dropDown.xaml
    /// </summary>
    public partial class dropDown : Page
    {
        public dropDown(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        //
        private MainWindow Main;
        //
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var y = Mouse.GetPosition(Main).Y;
            var x = Mouse.GetPosition(Main).X;

            if (x >= Main.ActualWidth / 2)
            {
                this.HorizontalAlignment = HorizontalAlignment.Right;
                var right = Main.ActualWidth - x - (this.ActualWidth / 2);
                if (right >= 20) this.Margin = new Thickness(0, y, right, 0);
                else this.Margin = new Thickness(0, y, 20, 0);
            }
            else
            {
                this.HorizontalAlignment = HorizontalAlignment.Left;
                this.Margin = new Thickness(20, y, 0, 0);
            }


        }
        private void ThongTinTK(object sender, MouseButtonEventArgs e)
        {
            var p = new Views.Pages.Setting_Account(Main);
            Main.sidebar.UnselectAll();
            Main.sidebarNV.UnselectAll();
            Main.NewPage(p);
        }

        private void BaoLoi(object sender, MouseButtonEventArgs e)
        {
            var p = new Views.Pages.Send_Error(Main);
            Main.sidebar.UnselectAll();
            Main.sidebarNV.UnselectAll();
            Main.NewPage(p);
        }

        private void DanhGia(object sender, MouseButtonEventArgs e)
        {
            var p = new Views.Pages.Rate(Main);
            Main.sidebar.UnselectAll();
            Main.sidebarNV.UnselectAll();
            Main.NewPage(p);
        }

        private void DangXuat(object sender, MouseButtonEventArgs e)
        {
            var c = new PopUp.ConfirmBox();
            c.ConfirmTitle = App.Current.Resources["textDangXuat"] as string;
            c.setMessage(App.Current.Resources["textDangXuat_Ques"] as string);
            Main.ShowPopup(c);

            c.Accept = () =>
            {
                //Main.Close();
                //var p = Application.Current.Windows.OfType<Window>().SingleOrDefault(x=>x.GetType()==typeof(LoginMain)) as LoginMain;
                //p.LoginSlectionPage.NavigationService.Navigate(new Views.Login.Login_Main(p));
                //p.ShowDialog();

                if (Main.LogOut != null) Main.LogOut();
                Properties.Settings.Default.EpPass = Properties.Settings.Default.ComPass = "";
                Properties.Settings.Default.Save();
            };
        }

        private void InfoAcc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.sidebar.UnselectAll();
            Main.sidebarNV.UnselectAll();
            Main.HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Setting_Account(Main));
        }

        private void SignOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
