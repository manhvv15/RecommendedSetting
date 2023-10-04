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
	
	public partial class PopUpDoiMatKhau : UserControl
	{
		public PopUpDoiMatKhau(MainWindow main)
		{
			InitializeComponent();
		}

		private void btnHuy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodNewPassword.Visibility = Visibility.Collapsed;
		}

		private void btnExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodNewPassword.Visibility = Visibility.Collapsed;
		}
	}
}
