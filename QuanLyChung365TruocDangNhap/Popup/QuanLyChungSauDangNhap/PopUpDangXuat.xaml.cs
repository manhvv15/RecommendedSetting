﻿using System;
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
		public PopUpDangXuat(MainWindow main)
		{
			InitializeComponent();
		}

		private void CancelPopup(object sender, MouseButtonEventArgs e)
		{
			bodCancel.Visibility = Visibility.Collapsed;
		}

		private void AddYes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodCancel.Visibility = Visibility.Collapsed;
		}
	}
}
