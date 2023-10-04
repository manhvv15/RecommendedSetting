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
	public partial class DangKyCaNhan : Window
	{
		private MainWindow Main;
		public DangKyCaNhan(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}
		Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

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

		private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			ChonLoaiTaiKhoan frm = new ChonLoaiTaiKhoan(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void Continue(object sender, MouseButtonEventArgs e)
		{
			try
			{
				using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/individual/register")))
				{
					RestRequest request = new RestRequest();
					request.Method = Method.Post;
					request.AlwaysMultipartFormData = true;
					request.AddParameter("phoneTK", txtPhoneTK.Text);
					request.AddParameter("userName", txtUsername.Text);
					request.AddParameter("password", passboxEmailPassWord.Password);
					request.AddParameter("phone", txtSDT.Text);
					request.AddParameter("address", txtAddress.Text);
					RestResponse resAlbum = restclient.Execute(request);
					var b = resAlbum.Content;
				}


			}
			catch
			{

			}
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
	}
}
