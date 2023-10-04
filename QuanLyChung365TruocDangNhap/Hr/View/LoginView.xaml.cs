using QuanLyChung365TruocDangNhap.Hr.Pages.LoginPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO.Compression;
using System.IO;
using System.Deployment.Application;

namespace QuanLyChung365TruocDangNhap.Hr.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public Scale scale;
        public string token;
        private bool checkUpdate()
        {
            WebClient httpClient = new WebClient();
            try
            {
                var response = httpClient.DownloadString("https://mess.timviec365.vn/User/GetLastLersionHR");
                //MessageBox.Show(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString());

                if (response.Equals(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()) || String.IsNullOrEmpty(response))
                {
                    return true;
                }
                else
                {
                    string Path = Environment.GetEnvironmentVariable("APPDATA") + @"\HR\InstallAndUpdate\";

                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }
                    if (File.Exists(Path + "HR.zip"))
                    {
                        File.Delete(Path + "HR.zip");
                        Directory.Delete(Path + "HR", true);
                    }
                    httpClient.DownloadFile(new Uri("https://mess.timviec365.vn/HR/Update/HR.zip"), Path + "HR.zip");
                    ZipFile.ExtractToDirectory(Path + "HR.zip", Path);
                    Process prc = new System.Diagnostics.Process();
                    prc.StartInfo.FileName = Path + "HR/" + "HR.application";
                    prc.Start();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public LoginView()
        {

            if (checkUpdate())
            {
                InitializeComponent();
                MainContent.NavigationService.Navigate(new SelectAccountPage(this,token));
            }
            else
            {
                this.Close();
            }
        }

        private void Minimimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            this.WindowState = WindowState.Minimized;
        }
        private int _IsFull = 0;

        public int IsFull
        {
            get { return _IsFull; }
            set
            {
                _IsFull = value;
                var workingArea = System.Windows.SystemParameters.WorkArea;
                switch (IsFull)
                {
                    case 0:
                        this.WindowState = WindowState.Normal;
                        Width = workingArea.Right - 180;
                        Height = workingArea.Bottom - 100;
                        Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                        Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
                        this.ResizeMode = ResizeMode.CanResize;
                        break;
                    case 1:
                        this.WindowState = WindowState.Normal;
                        Left = workingArea.TopLeft.X;
                        Top = workingArea.TopLeft.Y;
                        Width = workingArea.Width;
                        Height = workingArea.Height;
                        this.ResizeMode = ResizeMode.NoResize;
                        break;
                    default:
                        break;
                }
            }
        }
        private void NomarlSize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
            NomarlSize.Visibility = Visibility.Collapsed;
            Maximimize.Visibility = Visibility.Visible;
            logoInTitle.Margin = new Thickness(0, 0, 0, 0);
            pageTitle.Height = 40;
            //pageDisplay.Margin = new Thickness(0, 25, 0, 0);
        }

        private void Maximimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
            NomarlSize.Visibility = Visibility.Visible;
            Maximimize.Visibility = Visibility.Collapsed;
            pageTitle.Height = 45;
            logoInTitle.Margin = new Thickness(0, 5, 0, 0);
            //pageDisplay.Margin = new Thickness(0, 30, 0, 0);
        }

        private void CloseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void pageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void upload_size(object sender, SizeChangedEventArgs e)
        {
            // reponsive left_background
            if (Screen.ActualWidth > 1366)
            {
                left_background.Width = new GridLength(1, GridUnitType.Star);
                background_gradien_color.Visibility = Visibility.Visible;
            }
            else if (Screen.ActualWidth >= 1024 && Screen.ActualWidth <= 1366)
            {
                left_background.Width = new GridLength(384 + (Screen.ActualWidth - 1024) * 299 / 342);
                background_gradien_color.Visibility = Visibility.Visible;
            }
            else if (Screen.ActualWidth < 1024 && Screen.ActualWidth >= 768)
            {
                left_background.Width = new GridLength(248 + (Screen.ActualWidth - 768) * 136 / 256);
                background_gradien_color.Visibility = Visibility.Visible;
            }
            else
            {
                left_background.Width = new GridLength(0);
                background_gradien_color.Visibility = Visibility.Hidden;
            }

            scale = new Scale(Screen.ActualWidth / 1366, Screen.ActualWidth / 1366);
            this.DataContext = scale;

        }

        public class Scale
        {
            public double X_image { get; set; }

            public double Y_image { get; set; }

            public Scale(double X_image, double Y_image)
            {
                this.X_image = X_image;
                this.Y_image = Y_image;
            }
        }
    }
}