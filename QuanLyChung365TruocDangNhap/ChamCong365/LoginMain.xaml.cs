using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.ChamCong365
{
    /// <summary>
    /// Interaction logic for LoginMain.xaml
    /// </summary>
    public partial class LoginMain : Window
    {
        public LoginMain()
        {
            InitializeComponent();
            this.DataContext = this;
            LoginSlectionPage.NavigationService.Navigate(new Views.Login.Login_Main(this));

            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = workingArea.Right - 200;
            this.Height = workingArea.Bottom - 100;
            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);

            Console.WriteLine(externalIp.ToString());
        }
        public int LoginType { get; set; } = 0;
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Minimimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CloseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth <= 960)
            {
                gridMain.ColumnDefinitions[0].Width = new GridLength(0);
            }
            else gridMain.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        }

        private void Maximize(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                return;
            }
            else this.WindowState = WindowState.Normal;
        }
    }
}
