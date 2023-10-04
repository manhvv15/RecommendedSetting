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
	/// Interaction logic for PopupDangNhap.xaml
	/// </summary>
	public partial class PopupDangNhap : UserControl
	{
		private MainWindow Main;
		public PopupDangNhap(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}

		private void btnExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodDangNhap.Visibility = Visibility.Collapsed;
		}

		private void btnAddNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DangNhapDangKy frm = new DangNhapDangKy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);

		}
		public void SetAllTableCollapsed()
		{

		}

		private void btnAddCongTy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CongTy frm = new CongTy(Main);
			frm.Show();
			Main.Close();
		}

		private void btnAddCaNhan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CaNhan frm = new CaNhan(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}
	}
}
