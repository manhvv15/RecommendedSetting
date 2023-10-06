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

namespace QuanLyChung365TruocDangNhap.GioiHanIpVaPhanMem.Popup
{
    /// <summary>
    /// Interaction logic for ucChonNhanVienVaCapQuyen.xaml
    /// </summary>
    public partial class ucChonNhanVienVaCapQuyen : UserControl
    {
        public ucChonNhanVienVaCapQuyen()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            var grid = rectangle.Parent as Grid;
            grid.Visibility= Visibility.Collapsed;
            grid = null;
        }

        private void NavigateToSetUpPermission(object sender, MouseButtonEventArgs e)
        {
            ucCapQuyen uc = new ucCapQuyen();
            this.AddPopupBody.Children.Clear();
            object content = uc.Content;
            uc.Content = null;
            this.AddPopupBody.Children.Add(content as UIElement);
            uc = null;
            content = null;
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
   
        }
    }
}
