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
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap
{
	/// <summary>
	/// Interaction logic for ChonLoaiTaiKhoan.xaml
	/// </summary>
	public partial class ChonLoaiTaiKhoanDangNhap : Window
	{
		private MainWindow Main;
	
		public ChonLoaiTaiKhoanDangNhap(MainWindow main)
		{

			InitializeComponent();
			Main = main;
		}


		private void LoginStaff(object sender, MouseButtonEventArgs e)
		{
			DangNhapDangKy frm = new DangNhapDangKy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void LoginCompany(object sender, MouseButtonEventArgs e)
		{
			CongTy frm = new CongTy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void btnAddDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			PopUpDangKy frm = new PopUpDangKy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void btnAddDangNhap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			PopupDangNhap frm = new PopupDangNhap(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void LoginIndividual(object sender, MouseButtonEventArgs e)
		{
			CaNhan frm = new CaNhan(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}


	}
}
