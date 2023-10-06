using QuanLyChung365TruocDangNhap.Hr.Entities;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Hr.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.OverviewPopups
{
    /// <summary>
    /// Interaction logic for MenuPopups.xaml
    /// </summary>
    public partial class MenuPopups : UserControl
    {
        public delegate void ShowPopup();

        public static event ShowPopup onShowPopup;

        public delegate void HideAllPopup();

        public static event HideAllPopup hideAllPopup;

        public MenuPopups()
        {
            InitializeComponent();
        }

        public MenuPopups(LoginEmployeeEntity loginEmployeeEntity)
        {
            InitializeComponent();
            txtUserName.Text = loginEmployeeEntity.data.data.user_info.ep_name;
            txtComName.Text = loginEmployeeEntity.data.data.user_info.com_name;
            txtID.Text = loginEmployeeEntity.data.data.user_info.ep_id;
            string uri_avatar = loginEmployeeEntity.data.data.user_info.ep_image;
            if(uri_avatar != "")
            {
                imageAvatar.ImageSource = new BitmapImage(new Uri(APIs.API.avatar_uri + uri_avatar));
            }
        }

        public MenuPopups(LoginCompanyEntity loginCompanyEntity)
        {
            InitializeComponent();
            txtUserName.Text = loginCompanyEntity.data.data.user_info.com_name;
            txtComName.Text = loginCompanyEntity.data.data.user_info.com_name;
            txtID.Text = loginCompanyEntity.data.data.user_info.com_id;
            string uri_avatar = loginCompanyEntity.data.data.user_info.com_logo;
            if (uri_avatar != "")
            {
                imageAvatar.ImageSource = new BitmapImage(new Uri(APIs.API.logo_uri + uri_avatar));
            }
        }

        private void ShowExitPopup(object sender, MouseButtonEventArgs e)
        {
            onShowPopup();
            hideAllPopup();
        }

        private void clickProfile(object sender, MouseButtonEventArgs e)
        {
            HomeView Main = new HomeView();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    Main = (HomeView)window;
                }
            };
            if (Main.TypeUser == 1)
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://quanlychung.timviec365.vn/quan-ly-thong-tin-tai-khoan-cong-ty.html");
            }
            else
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://quanlychung.timviec365.vn/quan-ly-thong-tin-tai-khoan-cong-ty.html");
            }
        }

        private void clickVote(object sender, MouseButtonEventArgs e)
        {
            HomeView Main = new HomeView();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    Main = (HomeView)window;
                }
            };
            if (Main.TypeUser == 1)
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty/danh-gia.html");
            }
            else
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty/danh-gia.html");
            }
        }

        private void clickError(object sender, MouseButtonEventArgs e)
        {
            HomeView Main = new HomeView();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    Main = (HomeView)window;
                }
            };
            if (Main.TypeUser == 1)
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty/gui-loi.html");
            }
            else
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty/gui-loi.html");
            }
        }

        private void clickSupport(object sender, MouseButtonEventArgs e)
        {
            HomeView Main = new HomeView();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    Main = (HomeView)window;
                }
            };
            if (Main.TypeUser == 1)
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://phanmemnhansu.timviec365.vn/huong-dan.html");
            }
            else
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://phanmemnhansu.timviec365.vn/huong-dan.html");
            }
        }
    }
}
