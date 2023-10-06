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
		private frmMain frmMainW;
		public PopupDangNhap(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}
		public PopupDangNhap(frmMain main)
		{
			InitializeComponent();
			frmMainW = main;
		}
		private void btnExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodDangNhap.Visibility = Visibility.Collapsed;
		}

		private void btnAddNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//DangNhapDangKy frm = new DangNhapDangKy(Main);
			//pnlHienThi.Children.Clear();
			//object content = frm.Content;
			//frm.Content = null;
			//pnlHienThi.Children.Add(content as UIElement);
			frmMainW.pnlDangKy.Visibility = Visibility.Visible;
			frmMainW.pnlMain.Visibility = Visibility.Collapsed;
			frmMainW.PopCT.Visibility = Visibility.Collapsed;
			frmMainW.PopNV.Visibility = Visibility.Collapsed;
			frmMainW.PopIdCT.Visibility = Visibility.Collapsed;
			frmMainW.btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
			frmMainW.btnCollapseSizebar.Visibility = Visibility.Collapsed;
			frmMainW.pnlDangNhapCTy.Visibility = Visibility.Visible;

			frmMainW.textLoaiTK.Text = "TÀI KHOẢN NHÂN VIÊN";
			frmMainW.tb_TaiKhoanDangNhap.Text = Properties.Settings.Default.UserEp;
			frmMainW.tb_MatKhauGo.Password = Properties.Settings.Default.PassEp;
			this.Visibility = Visibility.Collapsed;
			
				
			
		}
		public void SetAllTableCollapsed()
		{

		}

		private void btnAddCongTy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			frmMainW.pnlDangKy.Visibility = Visibility.Visible;
			frmMainW.pnlMain.Visibility = Visibility.Collapsed;
			frmMainW.PopCT.Visibility = Visibility.Collapsed;
			frmMainW.PopNV.Visibility = Visibility.Collapsed;
			frmMainW.PopIdCT.Visibility = Visibility.Collapsed;
			frmMainW.btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
			frmMainW.btnCollapseSizebar.Visibility = Visibility.Collapsed;
			frmMainW.pnlDangNhapCTy.Visibility = Visibility.Visible;
			frmMainW.textLoaiTK.Text = "TÀI KHOẢN CÔNG TY";
			frmMainW.tb_TaiKhoanDangNhap.Text = Properties.Settings.Default.User;
			frmMainW.tb_MatKhauGo.Password = Properties.Settings.Default.Pass;
			this.Visibility = Visibility.Collapsed;
			//CongTy frm = new CongTy(Main);
			//frm.Show();
			//Main.Close();
		}

		private void btnAddCaNhan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CaNhan frm = new CaNhan(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
			this.Visibility = Visibility.Collapsed;
        }
    }
}
