using QuanLyChung365TruocDangNhap.Popup.TruocDangNhap;
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

namespace QuanLyChung365TruocDangNhap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window

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
		public MainWindow()

		{
			InitializeComponent();
			
		}
		private int _sideBarStatus;
		public int sideBarStatus
		{
			get { return _sideBarStatus; }
			set { _sideBarStatus = value; OnPropertyChanged("sideBarStatus"); }
		}
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (SideBar.Width != 60)
			{

				SideBar.Width = 60;
				tb.Visibility = Visibility.Collapsed;
				
				
			}
			else
			{
				SideBar.Width = 414;
				tb.Visibility = Visibility.Visible;
				
			}
		
		}


		private void btnAddDangNhap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			PopupDangNhap frm = new PopupDangNhap(this);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void btnAddDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();

			SetAllTableCollapsed();

			PopUpDangKy frm = new PopUpDangKy(this);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);

		}


		public void SetDefaultMenuColor()
		{
			foreach (var child in GridMenu.Children)
			{
				if (child is Border)
				{
					var border = (Border)child;
					border.BorderThickness = new Thickness(0, 0, 0, 1);
					border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#474747"));
					((TextBlock)border.Child).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#474747"));

				}
			}
		}
		public void SetAllTableCollapsed()
		{

		}

		private void btnAddNhanSu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();

		}
		public void ChangeBorderColor(Border border)
		{
			border.BorderThickness = new Thickness(0, 0, 0, 5);
			border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
			((TextBlock)border.Child).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#474747"));


		}

		private void btnAddQuanLyCongViec_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();
		}

		private void btnAddTatCa_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
		
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();
		}

		private void btnAddQuanLyNoiBo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();
		}

		private void btnAddQuanLyBanHang_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();
		}


		private void btnAddAll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			PopupNhanSu frm = new PopupNhanSu(this);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void btnDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			PopUpDangKy frm = new PopUpDangKy(this);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

	
	}
}
