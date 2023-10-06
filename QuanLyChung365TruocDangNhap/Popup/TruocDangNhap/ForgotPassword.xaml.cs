using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for SideBar.xaml
	/// </summary>
	public partial class ForgotPassword : Window
	{
		private MainWindow Main;
		public ForgotPassword(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}
		Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

		private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
		{
			tblValidateEmail.Text = "";
			if (string.IsNullOrEmpty(txtEmail.Text))
			{
				tblValidateEmail.Text = "Không được để trống";
			}
			else if (!regex.IsMatch(txtEmail.Text))
			{
				tblValidateEmail.Text = "Nhập không đúng định dạng email";
			}
		}

		private void txtEmail_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void Continue(object sender, MouseButtonEventArgs e)
		{
			bodQuenMatKhau.Visibility = Visibility.Collapsed;
			XacThucOTP frm = new XacThucOTP(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DangNhapDangKy frm = new DangNhapDangKy(Main);
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

		private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
		{
			tblValidateEmail.Text = "";
			if (string.IsNullOrEmpty(txtEmail.Text))
			{
				tblValidateEmail.Text = "Không được để trống";
			}
			else if (!regex.IsMatch(txtEmail.Text))
			{
				tblValidateEmail.Text = "Nhập không đúng định dạng email";
			}
		}
	}
}
