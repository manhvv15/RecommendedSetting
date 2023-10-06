using Emgu.CV.CvEnum;
using QuanLyChung365TruocDangNhap.Popup.TruocDangNhap;
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
using static QRCoder.PayloadGenerator;

namespace QuanLyChung365TruocDangNhap.RecommendSetting
{
    /// <summary>
    /// Interaction logic for ucRecommended.xaml
    /// </summary>
    public partial class ucRecommended : UserControl
    {

        frmMain Main;
        public ucRecommended(frmMain frmmain)
        {
            InitializeComponent();
            Main = frmmain;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetDefaultMenuColor();
            ChangeBorderColor((Border)sender);
            stackTime.Visibility = Visibility.Collapsed;
            hinhthucduyet.Visibility = Visibility.Collapsed;
            //stackKeHoach.Visibility = Visibility.Collapsed;
            stackSoCapVaHinhThuc.Visibility = Visibility.Visible;
        }


        public void ChangeBorderColor(Border border)
        {
            border.BorderThickness = new Thickness(0, 0, 0, 5);
            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
            ((TextBlock)border.Child).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
        }

        ///<summary>
        /// set default color menu
        /// </summary>
        public void SetDefaultMenuColor()
        {
            foreach (var child in Menu.Children)
            {
                if (child is Border)
                {
                    var border = (Border)child;
                    border.BorderThickness = new Thickness(0, 0, 0, 1);
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#474747"));
                    ((TextBlock)border.Child).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#474747"));

                }
            }
        }

        private void Border_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            SetDefaultMenuColor();
            ChangeBorderColor((Border)sender);
            stackTime.Visibility = Visibility.Collapsed;
            hinhthucduyet.Visibility = Visibility.Visible;
            stackSoCapVaHinhThuc.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            SetDefaultMenuColor();
            ChangeBorderColor((Border)sender);
            stackTime.Visibility = Visibility.Visible;
           // stackKeHoach.Visibility = Visibility.Visible;
            stackSoCapVaHinhThuc.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {
            borKeHoach.BorderThickness = new Thickness(0, 0, 0, 5);
            borKeHoach.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
            txtKeHoach.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
            borDotXuat.BorderThickness = new Thickness(0, 0, 0, 0);
            //borKeHoach.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
            txtDotXuat.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            stackKeHoach.Visibility = Visibility.Visible;   
            stackDotXuat.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borDotXuat.BorderThickness = new Thickness(0, 0, 0, 5); 
            borDotXuat.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
            txtDotXuat.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));

            borKeHoach.BorderThickness = new Thickness(0, 0, 0, 0);
            //borKeHoach.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
            txtKeHoach.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));

            stackDotXuat.Visibility = Visibility.Visible;
            stackKeHoach.Visibility =Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e)
        {
            Main.pnlShowPopUp.Children.Add(new PopupThemMoi(Main));
             //Main.pnlShowPopUp.Children.Add(new PopUpThongBaoCanDangNhap(Main));
            //Main.stp_ShowPopup.Children.Add(new PopupThemMoi(Main, this));
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MessageBox.Show("Hello");
        }
    }
}
