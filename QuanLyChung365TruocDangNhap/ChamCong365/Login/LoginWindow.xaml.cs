using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginWindow(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            WinMain = main;

            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.MinHeight = workingArea.Bottom - 100;
        }
        //
        public MainWindow WinMain { get; set; }

        public enum LoginTypes { Employee, Company }
        public LoginTypes Type { get; set; }
        private int _SizeScreen;

        public int SizeScreen
        {
            get { return _SizeScreen; }
            set { _SizeScreen = value; OnPropertyChanged(); }
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
                OnPropertyChanged();
            }
        }

        //
        private void WinMove(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) this.DragMove();
        }
        private void WinClose(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void WinMiniMize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void WinMaximize(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (Type)
            {
                case LoginTypes.Employee:
                    LoginSelectionPage.NavigationService.Navigate(new Views.Login.LoginEP(this));
                    break;
                case LoginTypes.Company:
                    LoginSelectionPage.NavigationService.Navigate(new Views.Login.LoginCom(this));
                    break;
                default:
                    break;
            }
        }
        public void ShowPopup(object e)
        {
            PopupSelectionPage.Visibility = Visibility.Visible;
            PopupSelectionPage.NavigationService.Navigate(e);
        }
        public void ClosePopup()
        {
            PopupSelectionPage.Visibility = Visibility.Collapsed;
            PopupSelectionPage.NavigationService.Navigate(null);
        }
        private void PopupSelectionPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PopupSelectionPage.Visibility == Visibility.Collapsed) PopupSelectionPage.NavigationService.Navigate(null);
        }
        private void ClickOutSidePopup(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) IsFull = 1;

            if (this.Width >= 1920)
            {
                SizeScreen = 1920;
            }
            else if (this.Width >= 1024)
            {
                SizeScreen = 1024;
            }
            else
            {
                SizeScreen = 768;
            }
            /*if (this.ActualWidth >= 780)
            {
                gridContent.ColumnDefinitions[0].Width = new GridLength(1,GridUnitType.Star);
            }
            else
            {
                gridContent.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
            }*/
        }
        public void LogOut(int full,Window w = null)
        {
            this.Close();

            if (w != null)
            {
                WinMain.IsFull = full;
                WinMain.Width = w.Width;
                WinMain.Height = w.Height;
                WinMain.Left = w.Left;
                WinMain.Top = w.Top;
            }

            WinMain.ShowDialog();
        }
    }
}
