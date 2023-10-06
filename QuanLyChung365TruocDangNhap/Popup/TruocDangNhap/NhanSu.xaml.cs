using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	/// Interaction logic for NhanSu.xaml
	/// </summary>
	public partial class NhanSu : UserControl
	{
		protected virtual void OnPropertyChanged(string newName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(newName));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private MainWindow Main;
		public NhanSu(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}
		private int _sideBarStatus;
		public int sideBarStatus
		{
			get { return _sideBarStatus; }
			set { _sideBarStatus = value; OnPropertyChanged("sideBarStatus"); }
		}

		private void btnAddAll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void btnAddDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void btnAddDangNhap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
