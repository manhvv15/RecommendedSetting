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
	/// Interaction logic for XacThucOTP.xaml
	/// </summary>
	public partial class XacThucOTP : UserControl
	{
		private MainWindow Main;
		public XacThucOTP(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}

		private void NhanMa_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodXacThucOTP.Visibility = Visibility.Collapsed;
			NhanMaOTP frm = new NhanMaOTP(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}
	}
}
