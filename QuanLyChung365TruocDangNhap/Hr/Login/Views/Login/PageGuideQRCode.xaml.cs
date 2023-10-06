﻿using System;
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
    /// Interaction logic for PageGuideQRCode.xaml
    /// </summary>
    public partial class PageGuideQRCode : Page
    {
        LoginWindow loginHome;
        public PageGuideQRCode(LoginWindow login)
        {
            InitializeComponent();
            loginHome = login;
        }

        private void returnLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loginHome.LoginSelectionPage.NavigationService.GoBack();
        }
    }
}
