using Newtonsoft.Json;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Popup.QuanLyChungSauDangNhap;
using RestSharp;
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

namespace QuanLyChung365TruocDangNhap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class SauDangNhap : Window
	{
		List<string> ListGioiTinh = new List<string>() { "Nam", "Nữ", "Khác" };
		List<string> ListTrinhDoHocVan = new List<string>() { "Chưa cập nhật", "Trên Đại học", "Đại học", "Cao đẳng", "Trung cấp", "Đào tạo nghề", "Trung học phổ thông", "Trung học cơ sở", "Tiểu học" };
		List<string> ListKinhNghiemLamViec = new List<string>() { "Chưa cập nhật", "Dưới 1 năm kinh nghiệm", "1 năm", "2 năm", "3 năm", "4 năm", "5 năm", "Trên 5 năm " };
		List<string> ListTo = new List<string>() { "Chọn tổ" };
		List<string> ListTinhTrangHonNhan = new List<string>() { "Độc thân", "Đã có gia đình" };
		List<string> ListNhom = new List<string>() { "Chọn nhóm" };

		private MainWindow Main;
		QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api_Com;
		QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff.API_Login_Staff api_staff;
		public int type = 0;
		public SauDangNhap(QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api)
		{
			InitializeComponent();
			textUserName.Text = api.data.data.user_info.com_name;
			api_Com = api;
			type = 1;
			lsvGioiTinh.ItemsSource = ListGioiTinh;
			lsvTrinhDoHocVan.ItemsSource = ListTrinhDoHocVan;
			lsvKinhNghiemLamViec.ItemsSource = ListKinhNghiemLamViec;
			lsvTo.ItemsSource = ListTo;
			lsvTinhTrangHonNhan.ItemsSource = ListTinhTrangHonNhan;
			lsvNhom.ItemsSource = ListNhom;
		}
		public SauDangNhap(QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff.API_Login_Staff api)
		{
			InitializeComponent();
			api_staff = api;
			type = 2;
			lsvGioiTinh.ItemsSource = ListGioiTinh;
			lsvTrinhDoHocVan.ItemsSource = ListTrinhDoHocVan;
			lsvKinhNghiemLamViec.ItemsSource = ListKinhNghiemLamViec;
			lsvTo.ItemsSource = ListTo;
			lsvTinhTrangHonNhan.ItemsSource = ListTinhTrangHonNhan;
			lsvNhom.ItemsSource = ListNhom;
		}




		private void AddNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (stpListMethond.Visibility == Visibility.Collapsed)
			{
				stpListMethond.Visibility = Visibility.Visible;
			}
			else
			{
				stpListMethond.Visibility = Visibility.Collapsed;
			}
		}

		private void dopAddSaff_MouseUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void ListNhanVien_MouseUp(object sender, MouseButtonEventArgs e)
		{

		}



		private void Delete_MouseUp(object sender, MouseButtonEventArgs e)
		{

			if (bodCancel.Visibility == Visibility.Collapsed)
			{
				bodCancel.Visibility = Visibility.Visible;

			}
			else
			{
				bodCancel.Visibility = Visibility.Collapsed;

			}
			if (stpListMethond.Visibility == Visibility.Collapsed)
			{
				stpListMethond.Visibility = Visibility.Visible;
			}
			else
			{
				stpListMethond.Visibility = Visibility.Collapsed;
			}
		}



		private void EditError_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bodInformation.Visibility = Visibility.Collapsed;
			bodEvaluate.Visibility = Visibility.Collapsed;

			if (bodError.Visibility == Visibility.Collapsed)
			{
				bodError.Visibility = Visibility.Visible;

			}
			else
			{
				bodError.Visibility = Visibility.Collapsed;

			}

			if (stpListMethond.Visibility == Visibility.Collapsed)
			{
				stpListMethond.Visibility = Visibility.Visible;
			}
			else
			{
				stpListMethond.Visibility = Visibility.Collapsed;
			}


		}



		private void Evaluate_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bodInformation.Visibility = Visibility.Collapsed;
			bodError.Visibility = Visibility.Collapsed;

			if (bodEvaluate.Visibility == Visibility.Collapsed)
			{

				bodEvaluate.Visibility = Visibility.Visible;
			}
			else
			{

				bodEvaluate.Visibility = Visibility.Collapsed;
			}
			if (stpListMethond.Visibility == Visibility.Collapsed)
			{
				stpListMethond.Visibility = Visibility.Visible;
			}
			else
			{
				stpListMethond.Visibility = Visibility.Collapsed;
			}

		}

		private void dopAddInformation_MouseUp(object sender, MouseButtonEventArgs e)
		{

			bodEvaluate.Visibility = Visibility.Collapsed;
			bodError.Visibility = Visibility.Collapsed;
			if (bodInformation.Visibility == Visibility.Collapsed)
			{
				bodInformation.Visibility = Visibility.Visible;
			}
			else
			{

				bodInformation.Visibility = Visibility.Collapsed;
			}

			if (stpListMethond.Visibility == Visibility.Collapsed)
			{
				stpListMethond.Visibility = Visibility.Visible;
			}
			else
			{
				stpListMethond.Visibility = Visibility.Collapsed;
			}

		}

		private void AddChangeInformation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			if (bodeditInformation.Visibility == Visibility.Collapsed)
			{
				bodeditInformation.Visibility = Visibility.Visible;
			}
			else
			{
				bodeditInformation.Visibility = Visibility.Collapsed;
			}

		}

		private void AddNewPassword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			if (bodNewPassword.Visibility == Visibility.Collapsed)
			{
				bodNewPassword.Visibility = Visibility.Visible;
			}
			else
			{
				bodNewPassword.Visibility = Visibility.Collapsed;
			}

		}

		private void btnExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodNewPassword.Visibility = Visibility.Collapsed;
		}

		private void btnHuy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodNewPassword.Visibility = Visibility.Collapsed;
		}

		private void Image_MouseLeftButtonUp_SelectGioiTinh(object sender, MouseButtonEventArgs e)
		{
			if (bodGioiTinh.Visibility == Visibility.Collapsed)
			{
				bodGioiTinh.Visibility = Visibility.Visible;
			}
			else
			{
				bodGioiTinh.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvGioiTinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectGioiTinh.Text = lsvGioiTinh.SelectedItem.ToString();
			bodGioiTinh.Visibility = Visibility.Collapsed;
		}

		private void Image_MouseLeftButtonUp_SelectTrinhDoHocVan(object sender, MouseButtonEventArgs e)
		{
			if (bodTrinhDoHocVan.Visibility == Visibility.Collapsed)
			{
				bodTrinhDoHocVan.Visibility = Visibility.Visible;
			}
			else
			{
				bodTrinhDoHocVan.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvTrinhDoHocVan_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectTrinhDoHocVan.Text = lsvTrinhDoHocVan.SelectedItem.ToString();
			bodTrinhDoHocVan.Visibility = Visibility.Collapsed;
		}

		private void Image_MouseLeftButtonUp_SelectKinhNghiemLamViec(object sender, MouseButtonEventArgs e)
		{
			if (bodKinhNghiemLamViec.Visibility == Visibility.Collapsed)
			{
				bodKinhNghiemLamViec.Visibility = Visibility.Visible;
			}
			else
			{
				bodKinhNghiemLamViec.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvKinhNghiemLamViec_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectKinhNghiemLamViec.Text = lsvKinhNghiemLamViec.SelectedItem.ToString();
			bodKinhNghiemLamViec.Visibility = Visibility.Collapsed;
		}

		private void Image_MouseLeftButtonUp_SelectTo(object sender, MouseButtonEventArgs e)
		{
			if (bodTo.Visibility == Visibility.Collapsed)
			{
				bodTo.Visibility = Visibility.Visible;
			}
			else
			{
				bodTo.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectTo.Text = lsvTo.SelectedItem.ToString();
			bodTo.Visibility = Visibility.Collapsed;
		}

		private void Image_MouseLeftButtonUp_SelectTinhTrangHonNhan(object sender, MouseButtonEventArgs e)
		{
			if (bodTinhTrangHonNhan.Visibility == Visibility.Collapsed)
			{
				bodTinhTrangHonNhan.Visibility = Visibility.Visible;
			}
			else
			{
				bodTinhTrangHonNhan.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvTinhTrangHonNhan_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectTinhTrangHonNhan.Text = lsvTinhTrangHonNhan.SelectedItem.ToString();
			bodTinhTrangHonNhan.Visibility = Visibility.Collapsed;
		}


		private void bodOpenCalendar_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (bodCalendarMonth.Visibility == Visibility.Collapsed)
			{
				bodCalendarMonth.Visibility = Visibility.Visible;
			}
			else
			{
				bodCalendarMonth.Visibility = Visibility.Collapsed;
			}
		}

		private void dapMonth_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectMonth.Text = dapMonth.ToString();
			bodCalendarMonth.Visibility = Visibility.Collapsed;
		}

		private void Image_MouseLeftButtonUp_SelectNhom(object sender, MouseButtonEventArgs e)
		{
			if (bodNhom.Visibility == Visibility.Collapsed)
			{
				bodNhom.Visibility = Visibility.Visible;
			}
			else
			{
				bodNhom.Visibility = Visibility.Collapsed;
			}
		}

		private void lsvNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectNhom.Text = lsvNhom.SelectedItem.ToString();
			bodNhom.Visibility = Visibility.Collapsed;
		}

		private void bodHuy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			bodeditInformation.Visibility = Visibility.Collapsed;
		}

		private void bodSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/updateInfoEmployee")))
				{
					RestRequest request = new RestRequest();
					request.Method = Method.Post;
					request.AlwaysMultipartFormData = true;
					request.AddParameter("email", txtEmail.Text);
					request.AddParameter("phoneTK", txtPhoneNumber.Text);
					request.AddParameter("userName", txtName.Text);
					request.AddParameter("address", txtAddress.Text);
					if (txbSelectGioiTinh.Text == "Nam")
					{
						request.AddParameter("gender", 0);
					}
					else if (txbSelectGioiTinh.Text == "Nữ")
					{
						request.AddParameter("gender", 1);
					}
					if (txbSelectTinhTrangHonNhan.Text == "Độc thân")
					{
						request.AddParameter("married", 0);
					}
					else if (txbSelectTinhTrangHonNhan.Text == "Đã có gia đình")
					{
						request.AddParameter("married", 1);
					}
					//request.AddParameter("married", 0);
					request.AddParameter("education", 2);
					request.AddParameter("experience", 0);
					request.AddParameter("idQLC", api_Com.data.data.user_info.com_id);
					//request.AddParameter("birthday", bodOpenCalendarMonth.ye);
					request.AddHeader("Authorization", "Bearer " + api_Com.data.data.access_token);
					RestResponse resAlbum = restclient.Execute(request);
					var b = resAlbum.Content;
					textUserName.Text = txtName.Text;
					bodeditInformation.Visibility = Visibility.Collapsed;
				}
					
					
			}
			catch
			{

			}
			
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

		public void ChangeBorderColor(Border border)
		{
			border.BorderThickness = new Thickness(0, 0, 0, 5);
			border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C5BD4"));
			((TextBlock)border.Child).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#474747"));
		}

		private void btnAddTatCa_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();
		}

		private void btnAddNhanSu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SetDefaultMenuColor();
			ChangeBorderColor((Border)sender);
			SetAllTableCollapsed();
		}

		private void btnAddQuanLyCongViec_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

		private void btnSideBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}



		private void txtMKCu_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void txtMKMoi_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void Complete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			bodNewPassword.Visibility = Visibility.Collapsed;
		}

		private void txtError_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void txtEvaluate_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void CancelPopup(object sender, MouseButtonEventArgs e)
		{
			bodCancel.Visibility = Visibility.Collapsed;
		}

		private void AddYes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Close();	
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

		private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
			if(type == 1)
			{
				QuanLyChung365TruocDangNhap.ChamCong365.MainWindow Chamcong_Com = new ChamCong365.MainWindow(api_Com);
				this.Close();
				Chamcong_Com.Show();
			}
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
			
			if (type == 1)
			{
				LoginCompanyEntity dataHrCom = new LoginCompanyEntity();
				dataHrCom.data = new Data1();
				dataHrCom.data.data = new Data();
				dataHrCom.data.data.access_token = api_Com.data.data.access_token;
				dataHrCom.data.data.refresh_token = api_Com.data.data.refresh_token;
				dataHrCom.data.data.user_info = api_Com.data.data.user_info;

				QuanLyChung365TruocDangNhap.Hr.View.HomeView Hr_Com = new Hr.View.HomeView(dataHrCom);
				this.Close();
				Hr_Com.Show();
			}
		}
    }
}






