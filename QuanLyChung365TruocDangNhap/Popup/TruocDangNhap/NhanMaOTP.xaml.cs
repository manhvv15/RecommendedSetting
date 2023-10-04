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
	/// Interaction logic for NhanMaOTP.xaml
	/// </summary>
	public partial class NhanMaOTP : UserControl
	{
		public NhanMaOTP(MainWindow main)
		{
			InitializeComponent();

			
		}
		private List<TextBox> listTxt = new List<TextBox>();

		private void txt_TextChanged(object sender, TextChangedEventArgs e)
		{
			var txt = sender as TextBox;
			if (txt != null)
			{
				foreach (TextBox item in listTxt)
				{
					if (item == txt)
					{
						if (txt != listTxt[listTxt.Count - 1] && !string.IsNullOrEmpty(txt.Text)) listTxt[listTxt.IndexOf(item) + 1].Focus();
						break;
					}
				}
			}
		}

		private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
		{

		}

		private void TiepTuc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			bodNhanMaOTP.Visibility = Visibility.Collapsed;
		}
	}
}
