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

namespace QuanLyChung365TruocDangNhap.Popup.TruocDangNhap
{
    /// <summary>
    /// Interaction logic for PopUpThongBaoCanDangNhap.xaml
    /// </summary>
    public partial class PopUpThongBaoCanDangNhap : UserControl
    {
        private frmMain frmMainW;
        public PopUpThongBaoCanDangNhap(frmMain frm)
        {
            InitializeComponent();
            frmMainW = frm;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmMainW.pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpDangKy(frmMainW));
            this.Visibility = Visibility.Collapsed;
        }

        private void btnDangNhap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmMainW.textThongBao.Visibility = Visibility.Collapsed;
            frmMainW.pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopupDangNhap(frmMainW));
            this.Visibility = Visibility.Collapsed;
        }
    }
}
