using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Popup.TruocDangNhap
{
	/// <summary>
	/// Interaction logic for DangKyCongTy.xaml
	/// </summary>
	
	public partial class DangKyCongTy : Window
	{
		private MainWindow Main;
		private frmMain frmMainW;
		QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api_Com;

		public DangKyCongTy(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}
		public DangKyCongTy(frmMain main)
		{
			InitializeComponent();
			frmMainW = main;
		}
		Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

		private void btnAddDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			frmMainW.pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpDangKy(frmMainW));
			//PopUpDangKy frm = new PopUpDangKy(Main);
			//pnlHienThi.Children.Clear();
			//object content = frm.Content;
			//frm.Content = null;
			//pnlHienThi.Children.Add(content as UIElement);
		}

		private void btnAddDangNhap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//PopupDangNhap frm = new PopupDangNhap(Main);
			//pnlHienThi.Children.Clear();
			//object content = frm.Content;
			//frm.Content = null;
			//pnlHienThi.Children.Add(content as UIElement);
		}

		private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			//ChonLoaiTaiKhoan frm = new ChonLoaiTaiKhoan(Main);
			//pnlHienThi.Children.Clear();
			//object content = frm.Content;
			//frm.Content = null;
			//pnlHienThi.Children.Add(content as UIElement);
		}

		private void Continue(object sender, MouseButtonEventArgs e)
		{
			
			//try
			//{
			//	using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/Company/register")))
			//	{
			//		RestRequest request = new RestRequest();
			//		request.Method = Method.Post;
			//		request.AlwaysMultipartFormData = true;
			//		request.AddParameter("email", txtEmail.Text);
			//		request.AddParameter("phoneTK", txtSDT.Text);
			//		request.AddParameter("userName", txtTenCongTy.Text);
			//		request.AddParameter("address",  txtAddress.Text);
			//		request.AddParameter("password", passboxEmailPassWord.Password);
			//		RestResponse resAlbum = restclient.Execute(request);
			//		var b = resAlbum.Content;
			//	}


			//}
			//catch
			//{
				
			//}
			//XacThucOTP frm = new XacThucOTP(Main);
			//pnlHienThi.Children.Clear();
			//object content = frm.Content;
			//frm.Content = null;
			//pnlHienThi.Children.Add(content as UIElement);
			//bodDangKyTKCongTy.Visibility = Visibility.Collapsed;

		}

		private void txtSDT_LostFocus(object sender, RoutedEventArgs e)
		{


		}


		private void txtSDT_TextChanged(object sender, TextChangedEventArgs e)
		{


		}

		private void txtTenCongTy_LostFocus(object sender, RoutedEventArgs e)
		{

		}
		private string _Password = "";
		public string Password
		{
			get { return _Password; }
			set
			{
				_Password = value;
				OnPropertyChanged();
			}
		}
		

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool _ShowPass = false;
		public bool ShowPass
		{
			get { return _ShowPass; }
			set { _ShowPass = value; OnPropertyChanged(); }
		}
		private void InputPassword(object sender, RoutedEventArgs e)
		{
			Password = passboxEmailPassWord.Password;

		}

		private void ShowPassword(object sender, MouseButtonEventArgs e)
		{
			ShowPass = !ShowPass;
			if (ShowPass)
			{
				txtEmailPassWord.Focus();
				txtEmailPassWord.SelectionStart = txtEmailPassWord.Text.Length;
			}
			else
			{
				ShowPass = false;
				passboxEmailPassWord.Password = Password;
				passboxEmailPassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxEmailPassWord, new object[] { Password.Length, 0 });
				passboxEmailPassWord.Focus();
			}
		}

        private void btnHome_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
