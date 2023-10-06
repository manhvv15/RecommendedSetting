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
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ForgotPasswordWindow(LoginWindow login)
        {
            InitializeComponent();
            this.DataContext = this;
            WinLogin = login;

            SelectionPage.NavigationService.Navigate(new Views.Login.ForgotPassword_Email(this));

            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.MinHeight = workingArea.Bottom - 100;
        }
        //
        public LoginWindow WinLogin { get; set; }
        private int _Type;
        public int Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }

        public class ForgotPassItem
        {
            public string Email { get; set; }
            public string OTP { get; set; }
        }
        public ForgotPassItem ForgotItem { get; set; } = new ForgotPassItem();

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
            this.Close();

            WinLogin.IsFull = this.IsFull;
            WinLogin.Width = this.ActualWidth;
            WinLogin.Height = this.ActualHeight;
            WinLogin.Left = this.Left;
            WinLogin.Top = this.Top;

            WinLogin.Show();
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
        }
    }
}
