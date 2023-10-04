using QuanLyChung365TruocDangNhap.Popup.OOP;
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
	
	public partial class DangKyTaiKhoanNhanVien : Window
	{
		public List<OOP.ClassGioiTinh> lstGT = new List<OOP.ClassGioiTinh>();

		public List<OOP.ClassChucVu> lstCV = new List<OOP.ClassChucVu>();
		public List<OOP.ClassTinhTrangHonNhan> lstTTHN = new List<OOP.ClassTinhTrangHonNhan>();
		public List<OOP.ClassTrinhDoHocVan> lstTDHV = new List<OOP.ClassTrinhDoHocVan>();
		public List<OOP.ClassKinhNghiemLamViec> lstKNLV = new List<OOP.ClassKinhNghiemLamViec>();

		private MainWindow Main;
		QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api_Com;
		private string IdCompany = "";

		public DangKyTaiKhoanNhanVien(MainWindow main, string IdCom)
		{
			
			InitializeComponent();
			lsvChucVu.ItemsSource = lstCV;
		    lsvGioiTinh.ItemsSource = lstGT;
			lsvTinhTrangHonNhan.ItemsSource = lstTTHN;
			lsvTrinhDoHocVan.ItemsSource = lstTDHV;
			lsvKinhNghiemLamViec.ItemsSource = lstKNLV;
			IdCompany = IdCom;
			Main = main;
			LoadDataChucVu();
			LoadDataGioiTinh();
			LoadDataTinhTrangHonNhan();
			LoadDataTrinhDoHocVan();
			LoadDataKinhNghiemLamViec();
		}

		private void LoadDataKinhNghiemLamViec()
		{
			OOP.ClassKinhNghiemLamViec Knlv = new OOP.ClassKinhNghiemLamViec();
			Knlv.IDKinhNghiemLamViec = "1";
			Knlv.TenKinhNghiemLamViec = "Chưa có kinh nghiệm";
			lstKNLV.Add(Knlv);
			OOP.ClassKinhNghiemLamViec Knlv1 = new OOP.ClassKinhNghiemLamViec();
			Knlv1.IDKinhNghiemLamViec = "2";
			Knlv1.TenKinhNghiemLamViec = "Dưới 1 năm kinh nghiệm";
			lstKNLV.Add(Knlv1);
			OOP.ClassKinhNghiemLamViec Knlv2 = new OOP.ClassKinhNghiemLamViec();
			Knlv2.IDKinhNghiemLamViec = "3";
			Knlv2.TenKinhNghiemLamViec = "1 năm";
			lstKNLV.Add(Knlv2);
			OOP.ClassKinhNghiemLamViec Knlv3 = new OOP.ClassKinhNghiemLamViec();
			Knlv3.IDKinhNghiemLamViec = "4";
			Knlv3.TenKinhNghiemLamViec = "2 năm";
			lstKNLV.Add(Knlv3);
			OOP.ClassKinhNghiemLamViec Knlv4 = new OOP.ClassKinhNghiemLamViec();
			Knlv4.IDKinhNghiemLamViec = "5";
			Knlv4.TenKinhNghiemLamViec = "3 năm";
			lstKNLV.Add(Knlv4);
			OOP.ClassKinhNghiemLamViec Knlv5 = new OOP.ClassKinhNghiemLamViec();
			Knlv5.IDKinhNghiemLamViec = "6";
			Knlv5.TenKinhNghiemLamViec = "4 năm";
			lstKNLV.Add(Knlv5);
			OOP.ClassKinhNghiemLamViec Knlv6 = new OOP.ClassKinhNghiemLamViec();
			Knlv6.IDKinhNghiemLamViec = "7";
			Knlv6.TenKinhNghiemLamViec = "5 năm";
			lstKNLV.Add(Knlv6);
			OOP.ClassKinhNghiemLamViec Knlv7 = new OOP.ClassKinhNghiemLamViec();
			Knlv7.IDKinhNghiemLamViec = "8";
			Knlv7.TenKinhNghiemLamViec = "Trên 5 năm";
			lstKNLV.Add(Knlv7);
		}

		private void LoadDataTrinhDoHocVan()
		{
			OOP.ClassTrinhDoHocVan Tdhv = new OOP.ClassTrinhDoHocVan();
			Tdhv.IDTrinhDoHocVan = "1";
			Tdhv.TenTrinhDoHocVan = "Trên Đại học";
			lstTDHV.Add(Tdhv);
			OOP.ClassTrinhDoHocVan Tdhv1 = new OOP.ClassTrinhDoHocVan();
			Tdhv1.IDTrinhDoHocVan = "2";
			Tdhv1.TenTrinhDoHocVan = "Đại học";
			lstTDHV.Add(Tdhv1);
			OOP.ClassTrinhDoHocVan Tdhv2 = new OOP.ClassTrinhDoHocVan();
			Tdhv2.IDTrinhDoHocVan = "3";
			Tdhv2.TenTrinhDoHocVan = "Cao đẳng";
			lstTDHV.Add(Tdhv2);
			OOP.ClassTrinhDoHocVan Tdhv3 = new OOP.ClassTrinhDoHocVan();
			Tdhv3.IDTrinhDoHocVan = "4";
			Tdhv3.TenTrinhDoHocVan = "Trung cấp";
			lstTDHV.Add(Tdhv3);
			OOP.ClassTrinhDoHocVan Tdhv4 = new OOP.ClassTrinhDoHocVan();
			Tdhv4.IDTrinhDoHocVan = "5";
			Tdhv4.TenTrinhDoHocVan = "Đào tạo nghề";
			lstTDHV.Add(Tdhv4);
			OOP.ClassTrinhDoHocVan Tdhv5 = new OOP.ClassTrinhDoHocVan();
			Tdhv5.IDTrinhDoHocVan = "6";
			Tdhv5.TenTrinhDoHocVan = "Trung học phổ thông";
			lstTDHV.Add(Tdhv5);
			OOP.ClassTrinhDoHocVan Tdhv6 = new OOP.ClassTrinhDoHocVan();
			Tdhv6.IDTrinhDoHocVan = "7";
			Tdhv6.TenTrinhDoHocVan = "Trung học cơ sở";
			lstTDHV.Add(Tdhv6);
			OOP.ClassTrinhDoHocVan Tdhv7 = new OOP.ClassTrinhDoHocVan();
			Tdhv7.IDTrinhDoHocVan = "8";
			Tdhv7.TenTrinhDoHocVan = "Tiểu học";
			lstTDHV.Add(Tdhv7);

		}

		private void LoadDataTinhTrangHonNhan()
		{
			OOP.ClassTinhTrangHonNhan Tthn = new OOP.ClassTinhTrangHonNhan();
			Tthn.IDTinhTrangHonNhan = "1";
			Tthn.TenTinhTrangHonNhan = "Độc thân";
			lstTTHN.Add(Tthn);
			OOP.ClassTinhTrangHonNhan Tthn1 = new OOP.ClassTinhTrangHonNhan();
			Tthn1.IDTinhTrangHonNhan = "2";
			Tthn1.TenTinhTrangHonNhan = "Đã có gia đình";
			lstTTHN.Add(Tthn1);
		}

		private void LoadDataGioiTinh()
		{
			OOP.ClassGioiTinh Gt = new OOP.ClassGioiTinh();
			Gt.IDGioiTinh = "1";
			Gt.TenGioiTinh = "Nam";
			lstGT.Add(Gt);
			OOP.ClassGioiTinh Gt1 = new OOP.ClassGioiTinh();
			Gt1.IDGioiTinh = "2";
			Gt1.TenGioiTinh = "Nữ";
			lstGT.Add(Gt1);
		}

		private void LoadDataChucVu()
		{
			OOP.ClassChucVu Cv = new OOP.ClassChucVu();
			Cv.IDCV = "1";
			Cv.TenCV = "Sinh viên thực tập";
			lstCV.Add(Cv);
			OOP.ClassChucVu Cv1 = new OOP.ClassChucVu();
			Cv1.IDCV = "2";
			Cv1.TenCV = "NHÂN VIÊN THỬ VIỆC";
			lstCV.Add(Cv1);
			OOP.ClassChucVu Cv2 = new OOP.ClassChucVu();
			Cv2.IDCV = "3";
			Cv2.TenCV = "NHÂN VIÊN CHÍNH THỨC";
			lstCV.Add(Cv2);
			OOP.ClassChucVu Cv3 = new OOP.ClassChucVu();
			Cv3.IDCV = "4";
			Cv3.TenCV = "TRƯỞNG NHÓM";
			lstCV.Add(Cv3);
			OOP.ClassChucVu Cv4 = new OOP.ClassChucVu();
			Cv4.IDCV = "5";
			Cv4.TenCV = "PHÓ TRƯỞNG PHÒNG";
			lstCV.Add(Cv4);
			OOP.ClassChucVu Cv5 = new OOP.ClassChucVu();
			Cv5.IDCV = "6";
			Cv5.TenCV = "TRƯỞNG PHÒNG";
			lstCV.Add(Cv5);
			OOP.ClassChucVu Cv6 = new OOP.ClassChucVu();
			Cv6.IDCV = "7";
			Cv6.TenCV = "PHÓ GIÁM ĐỐC";
			lstCV.Add(Cv6);
			OOP.ClassChucVu Cv7 = new OOP.ClassChucVu();
			Cv7.IDCV = "8";
			Cv7.TenCV = "GIÁM ĐỐC";
			lstCV.Add(Cv7);
			OOP.ClassChucVu Cv8 = new OOP.ClassChucVu();
			Cv8.IDCV = "9";
			Cv8.TenCV = "NHÂN VIÊN PART TIME";
			lstCV.Add(Cv8);
			OOP.ClassChucVu Cv9 = new OOP.ClassChucVu();
			Cv9.IDCV = "10";
			Cv9.TenCV = "PHÓ BAN DỰ ÁN";
			lstCV.Add(Cv9);
			OOP.ClassChucVu Cv10 = new OOP.ClassChucVu();
			Cv10.IDCV = "11";
			Cv10.TenCV = "TRƯỞNG BAN DỰ ÁN";
			lstCV.Add(Cv10);
			OOP.ClassChucVu Cv11 = new OOP.ClassChucVu();
			Cv11.IDCV = "12";
			Cv11.TenCV = "PHÓ TỔ TRƯỞNG";
			lstCV.Add(Cv11);
			OOP.ClassChucVu Cv12 = new OOP.ClassChucVu();
			Cv12.IDCV = "13";
			Cv12.TenCV = "TỔ TRƯỞNG";
			lstCV.Add(Cv12);
			OOP.ClassChucVu Cv13 = new OOP.ClassChucVu();
			Cv13.IDCV = "14";
			Cv13.TenCV = "PHÓ TỔNG GIÁM ĐỐC";
			lstCV.Add(Cv13);
			OOP.ClassChucVu Cv14 = new OOP.ClassChucVu();
			Cv14.IDCV = "16";
			Cv14.TenCV = "TỔNG GIÁM ĐỐC";
			lstCV.Add(Cv14);
			OOP.ClassChucVu Cv15 = new OOP.ClassChucVu();
			Cv15.IDCV = "17";
			Cv15.TenCV = "THÀNH VIÊN HỘI ĐỒNG QUẢN TRỊ";
			lstCV.Add(Cv15);
			OOP.ClassChucVu Cv16 = new OOP.ClassChucVu();
			Cv16.IDCV = "18";
			Cv16.TenCV = "PHÓ CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ";
			lstCV.Add(Cv16);
			OOP.ClassChucVu Cv17 = new OOP.ClassChucVu();
			Cv17.IDCV = "19";
			Cv17.TenCV = "CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ";
			lstCV.Add(Cv17);
			OOP.ClassChucVu Cv18 = new OOP.ClassChucVu();
			Cv18.IDCV = "20";
			Cv18.TenCV = "NHÓM PHÓ";
			lstCV.Add(Cv18);
			OOP.ClassChucVu Cv19 = new OOP.ClassChucVu();
			Cv19.IDCV = "21";
			Cv19.TenCV = "TỔNG GIÁM ĐỐC TẬP ĐOÀN";
			lstCV.Add(Cv19);
			OOP.ClassChucVu Cv20 = new OOP.ClassChucVu();
			Cv20.IDCV = "22";
			Cv20.TenCV = "PHÓ TỔNG GIÁM ĐỐC TẬP ĐOÀN";
			lstCV.Add(Cv19);
			
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
			IDCongTy frm = new IDCongTy(Main);
			pnlHienThi.Children.Clear();
			object content = frm.Content;
			frm.Content = null;
			pnlHienThi.Children.Add(content as UIElement);
		}

		private void Continue(object sender, MouseButtonEventArgs e)
		{
			try
			{
				using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/register")))
				{
					RestRequest request = new RestRequest();
					request.Method = Method.Post;
					request.AlwaysMultipartFormData = true;
					request.AddParameter("email", txtName.Text);
					request.AddParameter("com_id", IdCompany);
					request.AddParameter("phoneTK", txtSDT.Text);
					request.AddParameter("userName", txtName.Text);
					request.AddParameter("address", txtAddress.Text);
					request.AddParameter("password", passboxEmailPassWord.Password);
			

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

					request.AddParameter("birthday", DateTime.Now.Year + "-" + "0" + DateTime.Now.Month + "-" + DateTime.Now.Day);
					request.AddParameter("education", 2);
					request.AddParameter("experience", 0);
					request.AddParameter("position_id", 1);

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
			var selectedGT = (ClassGioiTinh)lsvGioiTinh.SelectedItem;
			if (selectedGT != null)
			{
				txbSelectGioiTinh.Text = selectedGT.TenGioiTinh;
				bodGioiTinh.Visibility = Visibility.Collapsed;
			}
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
			
			var selectedTDHV = (ClassTrinhDoHocVan)lsvTrinhDoHocVan.SelectedItem;
			if (selectedTDHV != null)
			{
				txbSelectTrinhDoHocVan.Text = selectedTDHV.TenTrinhDoHocVan;
				bodTrinhDoHocVan.Visibility = Visibility.Collapsed;
			}

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
			var selectedKNLV = (ClassKinhNghiemLamViec)lsvKinhNghiemLamViec.SelectedItem;
			if (selectedKNLV != null)
			{
				txbSelectKinhNghiemLamViec.Text = selectedKNLV.TenKinhNghiemLamViec;
				bodKinhNghiemLamViec.Visibility = Visibility.Collapsed;
			}		
		}

		private void Image_MouseLeftButtonUp_SelectLoaiToChuc(object sender, MouseButtonEventArgs e)
		{
			if (bodLoaiToChuc.Visibility == Visibility.Collapsed)
			{
				bodLoaiToChuc.Visibility = Visibility.Visible;
			}
			else
			{
				bodLoaiToChuc.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvLoaiToChuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectLoaiToChuc.Text = lsvLoaiToChuc.SelectedItem.ToString();
			bodLoaiToChuc.Visibility = Visibility.Collapsed;
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

			var selectedTTHN = (ClassTinhTrangHonNhan)lsvTinhTrangHonNhan.SelectedItem;
			if (selectedTTHN != null)
			{
				txbSelectTinhTrangHonNhan.Text = selectedTTHN.TenTinhTrangHonNhan;
				bodTinhTrangHonNhan.Visibility = Visibility.Collapsed;
			}

		}

		private void Image_MouseLeftButtonUp_SelectChucVu(object sender, MouseButtonEventArgs e)
		{
			if (bodChucVu.Visibility == Visibility.Collapsed)
			{
				bodChucVu.Visibility = Visibility.Visible;
			}
			else
			{
				bodChucVu.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvChucVu_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedCV = (ClassChucVu)lsvChucVu.SelectedItem;
			if (selectedCV != null)
			{
				txbSelectChucVu.Text = selectedCV.TenCV;
				bodChucVu.Visibility = Visibility.Collapsed;
			}


		}

		private void Image_MouseLeftButtonUp_SelectTenToChuc(object sender, MouseButtonEventArgs e)
		{
			if (bodTenToChuc.Visibility == Visibility.Collapsed)
			{
				bodTenToChuc.Visibility = Visibility.Visible;
			}
			else
			{
				bodTenToChuc.Visibility -= Visibility.Collapsed;

			}
		}

		private void lsvTenToChuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			txbSelectTenToChuc.Text = lsvTenToChuc.SelectedItem.ToString();
			bodTenToChuc.Visibility = Visibility.Collapsed;
		}
	}
}
