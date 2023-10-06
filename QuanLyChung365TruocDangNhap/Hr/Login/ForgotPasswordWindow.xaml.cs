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

namespace QuanLyChung365TruocDangNhap.Hr.Login
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
        //
        private void WinMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void WinClose(object sender, MouseButtonEventArgs e)
        {
            this.Close();

            WinLogin.Width = this.ActualWidth;
            WinLogin.Height = this.ActualHeight;
            WinLogin.Left = this.Left;
            WinLogin.Top = this.Top;
            WinLogin.WindowState = this.WindowState;

            WinLogin.Show();
        }
        private void WinMiniMize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void WinMaximize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            if (this.WindowState == WindowState.Normal)
            {
                var workingArea = System.Windows.SystemParameters.WorkArea;
                this.Width = workingArea.Right - 180;
                this.Height = workingArea.Bottom - 100;
                this.Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                this.Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
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

    }
}
