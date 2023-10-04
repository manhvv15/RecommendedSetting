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
	/// Interaction logic for PopUpDangXuat.xaml
	/// </summary>
	public partial class PopUpDangKy : UserControl
	{

		private MainWindow Main;
		public PopUpDangKy(MainWindow main)
		{
			InitializeComponent();

			Main = main;
		}

		private void btnExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodDangKy.Visibility = Visibility.Collapsed;
		}

		
		private void NhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			IDCongTy frm = new IDCongTy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void CongTy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DangKyCongTy frm = new DangKyCongTy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void CaNhan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DangKyCaNhan frm = new DangKyCaNhan(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		
	}
}
