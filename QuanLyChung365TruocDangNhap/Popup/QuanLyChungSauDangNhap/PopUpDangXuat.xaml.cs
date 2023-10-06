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

namespace QuanLyChung365TruocDangNhap.Popup.QuanLyChungSauDangNhap
{
	/// <summary>
	/// Interaction logic for PopUpDangXuat.xaml
	/// </summary>
	public partial class PopUpDangXuat : UserControl
	{
		private frmMain frmMainW;
		public PopUpDangXuat(frmMain main)
		{
			InitializeComponent();
			frmMainW = main;
		}

		private void CancelPopup(object sender, MouseButtonEventArgs e)
		{
			this.Visibility = Visibility.Collapsed;
		}

		private void AddYes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			frmMainW.IdAcount = "";
			frmMainW.popup.Visibility = Visibility.Collapsed;
			frmMainW.borThongTinChiTiet.Visibility = Visibility.Collapsed;
			frmMainW.docListBtn.Visibility = Visibility.Visible;
			frmMainW.btnShowSPN.Visibility = Visibility.Collapsed;
			this.Visibility = Visibility.Collapsed;
		}

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
			this.Visibility = Visibility.Collapsed;
        }
    }
}
