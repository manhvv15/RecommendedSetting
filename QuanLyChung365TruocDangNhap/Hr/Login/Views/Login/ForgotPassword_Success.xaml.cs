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

namespace QuanLyChung365TruocDangNhap.Hr.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassword_Email.xaml
    /// </summary>
    public partial class ForgotPassword_Success : Page
    {
        public ForgotPassword_Success(ForgotPasswordWindow win)
        {
            InitializeComponent();
            this.DataContext = this;
            WinForgot = win;
        }
        //
        public ForgotPasswordWindow WinForgot { get; set; }
        //
        private void Login(object sender, MouseButtonEventArgs e)
        {
            WinForgot.Close();
            WinForgot.WinLogin.Close();
            WinForgot.WinLogin.WinMain.ShowDialog();
        }

        private void Skip(object sender, MouseButtonEventArgs e)
        {
            WinForgot.Close();
            WinForgot.WinLogin.Close();
            WinForgot.WinLogin.WinMain.ShowDialog();
        }
    }
}
