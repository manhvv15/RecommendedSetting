using Newtonsoft.Json;
using QuanLyChung365TruocDangNhap.Popup.TruocDangNhap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace QuanLyChung365TruocDangNhap
{
	/// <summary>
	/// Interaction logic for DangNhapDangKy.xaml
	/// </summary>
	/// 


	public partial class CaNhan : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private MainWindow Main;
		public CaNhan(MainWindow main)
		{
			InitializeComponent();
			Main = main;
		}

		private int _TypeLogin = 1;

		public int TypeLogin
		{
			get { return _TypeLogin; }
			set { _TypeLogin = value; OnPropertyChanged(); }
		}
		private string _Pass = "";
		public string Pass
		{
			get { return _Pass; }
			set { _Pass = value; OnPropertyChanged(); }
		}
		Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
		Regex regexSDT = new Regex(@"^((09|03|07|08|05|04)+([0-9]{8})\b)$", System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.Singleline);


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

		private void GoBack(object sender, MouseButtonEventArgs e)
		{
			ChonLoaiTaiKhoanDangNhap frm = new ChonLoaiTaiKhoanDangNhap(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}


		public void ChangeBorderColor(Border border)
		{
			border.BorderThickness = new Thickness(0, 0, 0, 5);
			border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
			((TextBlock)border.Child).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));


		}
		public void SetDefaultMenuColor()
		{

		}

		private void TaiKhoan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();
		}

		private void SelectedTypeLogin(object sender, MouseButtonEventArgs e)
		{

			Border b = sender as Border;
			if (b.Name == "QR" && TypeLogin == 1 || b.Name == "TaiKhoan" && TypeLogin == 0)
			{

			}
			else
				TypeLogin = TypeLogin == 1 ? 0 : 1;
			if (TypeLogin == 1)
			{
				spQRCode.Visibility = Visibility.Visible;
				Login_Account.Visibility = Visibility.Collapsed;
			}
			else
			{
				Login_Account.Visibility = Visibility.Visible;
				spQRCode.Visibility = Visibility.Collapsed;
			}
		}

		private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
		{
			tblValidateEmail.Text = "";
			if (string.IsNullOrEmpty(txtEmail.Text))
			{
				tblValidateEmail.Text = "Không được để trống";
			}
			else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
			{
				tblValidateEmail.Text = "Nhập không đúng định dạng";
			}
		}

		private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
		{
			tblValidateEmail.Text = "";
			if (string.IsNullOrEmpty(txtEmail.Text))
			{
				tblValidateEmail.Text = "Không được để trống";
			}
			else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
			{
				tblValidateEmail.Text = "Nhập không đúng định dạng";
			}
		}

		private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
		{
			Pass = txtPass.Password;
			tblValidatePass.Text = "";
			if (string.IsNullOrEmpty(Pass))
			{
				tblValidatePass.Text = "Không được để trống";
			}
		}

		private void txtPass_LostFocus(object sender, RoutedEventArgs e)
		{
			tblValidatePass.Text = "";
			if (string.IsNullOrEmpty(Pass))
			{
				tblValidatePass.Text = "Không được để trống";
			}
		}

		private void txtPass_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void ckSave_Unchecked(object sender, RoutedEventArgs e)
		{
			QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpEmail = "";
			QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpPass = "";
			QuanLyChung365TruocDangNhap.Properties.Settings.Default.Save();
		}

		private void ForgotPass(object sender, MouseButtonEventArgs e)
		{
			ForgotPassword frm = new ForgotPassword(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private async void Login(object sender, MouseButtonEventArgs e)
		{
			try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("account", txtEmail.Text);
                form.Add("password", txtPass.Password);
                form.Add("type", "1");
                if (Properties.Settings.Default.ComTypePass == "1")
                {
                    form.Add("passtype", "1");
                }
                HttpClient httpClient = new HttpClient();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api = JsonConvert.DeserializeObject<QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
				if (api.data != null && api.data.result)
				{
					var main = new QuanLyChung365TruocDangNhap.SauDangNhap(api);

					var workingArea = System.Windows.SystemParameters.WorkArea;
					main.WindowState = WindowState.Normal;
					main.Width = workingArea.Right - 180;
					main.Height = workingArea.Bottom - 100;
					main.Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
					main.Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
					main.ResizeMode = ResizeMode.CanResize;
					/*main.LogOut = () =>
					{
						this.IsFull = main.IsFull;
						this.Width = main.Width;
						this.Height = main.Height;
						this.Left = main.Left;
						this.Top = main.Top;
						main.Close();
						this.Show();

					};*/
					/*pnlHienThi.Children.Clear();
					object content = main.Content;
					main.Content = null;
					pnlHienThi.Children.Add(content as UIElement);
					pnlHienThi.Visibility = Visibility.Visible;
					pnlHienThi.Background = App.Current.Resources["#FFFFFF"] as SolidColorBrush;*/
					this.Close();
					main.Show();
				}
				else
				{
					/*InitializeComponent();
					var workingArea = System.Windows.SystemParameters.WorkArea;
					this.Width = workingArea.Right - 180;
					this.Height = workingArea.Bottom - 100;
					listShowx = listShow;
					this.ShowDialog();*/
				}
			}
            catch (Exception ex)
            {
                /*InitializeComponent();
                var workingArea = System.Windows.SystemParameters.WorkArea;
                this.Width = workingArea.Right - 180;
                this.Height = workingArea.Bottom - 100;
                listShowx = listShow;
                this.ShowDialog();*/
            }
		}

		private void SignIn(object sender, MouseButtonEventArgs e)
		{
		    DangKyCaNhan frm = new DangKyCaNhan(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void GuideQrCode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Qr.Visibility == Visibility.Collapsed)
			{
				Qr.Visibility = Visibility.Visible;
			}
			else
			{
				Qr.Visibility = Visibility.Collapsed;
			}
		}

		private void returnLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Qr.Visibility = Visibility.Collapsed;
		}


	}

}

