using Newtonsoft.Json;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Hr.View;
using QuanLyChung365TruocDangNhap.RecommendSetting;
using QuanLyChung365TruocDangNhap.ThietLapCongTy.CoCau_ViTri_ToChuc;
using QuanLyChung365TruocDangNhap.ThietLapCongTy.Them_Xoa_NhanVien;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static QRCoder.PayloadGenerator;

namespace QuanLyChung365TruocDangNhap
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        public string IdAcount = "";
        private string IdComRegister = "";
        public frmMain()
        {
            InitializeComponent();
            btnMaximize.Visibility = Visibility.Collapsed;
            btnNomal.Visibility = Visibility.Visible;
            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.WindowState = WindowState.Normal;
            Left = workingArea.TopLeft.X;
            Top = workingArea.TopLeft.Y;
            Width = workingArea.Width;
            Height = workingArea.Height;
            this.ResizeMode = ResizeMode.NoResize;

        }
        private void btnMaximize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //this.WindowState = WindowState.Maximized;
            btnMaximize.Visibility = Visibility.Collapsed;
            btnNomal.Visibility = Visibility.Visible;
            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.WindowState = WindowState.Normal;
            Left = workingArea.TopLeft.X;
            Top = workingArea.TopLeft.Y;
            Width = workingArea.Width;
            Height = workingArea.Height;
            this.ResizeMode = ResizeMode.NoResize;

        }

        private void btnMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
            //grShowPopup.Children.Add(new Popup.PopUpHoiTruocKhiDangXuat(this, frmLogin));
        }

        private void btnNomal_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            btnMaximize.Visibility = Visibility.Visible;
            btnNomal.Visibility = Visibility.Collapsed;
            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.WindowState = WindowState.Normal;
            Width = workingArea.Right - 180;
            Height = workingArea.Bottom - 100;
            Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
            Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
            this.ResizeMode = ResizeMode.CanResize;


        }

        private void pnlTieuDe1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnDisplayFullSizebar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pnlFullSizebar.Visibility = Visibility.Visible;
            pnlCollapseSizebar.Visibility = Visibility.Collapsed;
            btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
            btnCollapseSizebar.Visibility = Visibility.Visible;
        }

        private void btnCollapseSizebar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pnlFullSizebar.Visibility = Visibility.Collapsed;
            pnlCollapseSizebar.Visibility = Visibility.Visible;
            btnDisplayFullSizebar.Visibility = Visibility.Visible;
            btnCollapseSizebar.Visibility = Visibility.Collapsed;
        }

        private void btnAnCT_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            btnAnCT.Visibility = Visibility.Collapsed;
            btnHienCT.Visibility = Visibility.Visible;
            spnFullTLCT.Visibility = Visibility.Collapsed;
        }

        private void btnHienCT_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            btnAnCT.Visibility = Visibility.Visible;
            btnHienCT.Visibility = Visibility.Collapsed;
            spnFullTLCT.Visibility = Visibility.Visible;
        }

        private void spnFull_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borFull.Visibility = Visibility.Visible;
            borNS.Visibility = Visibility.Collapsed;
            borQLBH.Visibility = Visibility.Collapsed;
            borQLCV.Visibility = Visibility.Collapsed;
            borQLNB.Visibility = Visibility.Collapsed;
            gridFull.Visibility = Visibility.Visible;
            gridNhanSu.Visibility = Visibility.Collapsed;
            gridQLBanHang.Visibility = Visibility.Collapsed;
            gridQLCongViec.Visibility = Visibility.Collapsed;
            gridQLNoiBo.Visibility = Visibility.Collapsed;
        }

        private void spnNS_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borFull.Visibility = Visibility.Collapsed;
            borNS.Visibility = Visibility.Visible;
            borQLBH.Visibility = Visibility.Collapsed;
            borQLCV.Visibility = Visibility.Collapsed;
            borQLNB.Visibility = Visibility.Collapsed;
            gridFull.Visibility = Visibility.Collapsed;
            gridNhanSu.Visibility = Visibility.Visible;
            gridQLBanHang.Visibility = Visibility.Collapsed;
            gridQLCongViec.Visibility = Visibility.Collapsed;
            gridQLNoiBo.Visibility = Visibility.Collapsed;
        }

        private void spnQLCV_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borFull.Visibility = Visibility.Collapsed;
            borNS.Visibility = Visibility.Collapsed;
            borQLBH.Visibility = Visibility.Collapsed;
            borQLCV.Visibility = Visibility.Visible;
            borQLNB.Visibility = Visibility.Collapsed;
            gridFull.Visibility = Visibility.Collapsed;
            gridNhanSu.Visibility = Visibility.Collapsed;
            gridQLBanHang.Visibility = Visibility.Collapsed;
            gridQLCongViec.Visibility = Visibility.Visible;
            gridQLNoiBo.Visibility = Visibility.Collapsed;
        }

        private void spnQLNB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borFull.Visibility = Visibility.Collapsed;
            borNS.Visibility = Visibility.Collapsed;
            borQLBH.Visibility = Visibility.Collapsed;
            borQLCV.Visibility = Visibility.Collapsed;
            borQLNB.Visibility = Visibility.Visible;
            gridFull.Visibility = Visibility.Collapsed;
            gridNhanSu.Visibility = Visibility.Collapsed;
            gridQLBanHang.Visibility = Visibility.Collapsed;
            gridQLCongViec.Visibility = Visibility.Collapsed;
            gridQLNoiBo.Visibility = Visibility.Visible;
        }

        private void spnQLBH_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borFull.Visibility = Visibility.Collapsed;
            borNS.Visibility = Visibility.Collapsed;
            borQLBH.Visibility = Visibility.Visible;
            borQLCV.Visibility = Visibility.Collapsed;
            borQLNB.Visibility = Visibility.Collapsed;
            gridFull.Visibility = Visibility.Collapsed;
            gridNhanSu.Visibility = Visibility.Collapsed;
            gridQLBanHang.Visibility = Visibility.Visible;
            gridQLCongViec.Visibility = Visibility.Collapsed;
            gridQLNoiBo.Visibility = Visibility.Collapsed;
        }

        private void clearPopUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borThongTinChiTiet.Visibility = Visibility.Collapsed;
            popup.Visibility = Visibility.Collapsed;
        }

        private void btnShowSPN_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borThongTinChiTiet.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;
        }

        private void btnDangKy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpDangKy(this));
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
            //Password = passboxEmailPassWord.Password;

        }

        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {
            //ShowPass = !ShowPass;
            //if (ShowPass)
            //{
            //    txtEmailPassWord.Focus();
            //    txtEmailPassWord.SelectionStart = txtEmailPassWord.Text.Length;
            //}
            //else
            //{
            //    ShowPass = false;
            //    passboxEmailPassWord.Password = Password;
            //    passboxEmailPassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxEmailPassWord, new object[] { Password.Length, 0 });
            //    passboxEmailPassWord.Focus();
            //}
        }

        private void btnHome_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pnlMain.Visibility = Visibility.Visible;
            btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
            btnCollapseSizebar.Visibility = Visibility.Visible;
            pnlFullSizebar.Visibility = Visibility.Visible;
            pnlCollapseSizebar.Visibility = Visibility.Collapsed;
            pnlDangKy.Visibility = Visibility.Collapsed;
        }

        private void btnContinue_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDCompany.Text))
            {
                textTB.Visibility = Visibility.Visible;
            }
            else
            {
                pnlDangKy.Visibility = Visibility.Visible;
                pnlMain.Visibility = Visibility.Collapsed;
                PopCT.Visibility = Visibility.Collapsed;
                PopNV.Visibility = Visibility.Visible;
                PopIdCT.Visibility = Visibility.Collapsed;
                btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                btnCollapseSizebar.Visibility = Visibility.Collapsed;
                textTB.Visibility = Visibility.Collapsed;
                IdComRegister = txtIDCompany.Text;
            }

        }

        private void btnDangNhap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            textThongBao.Visibility = Visibility.Collapsed;
            pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopupDangNhap(this));

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

            //Border b = sender as Border;
            //if (b.Name == "QR" && TypeLogin == 1 || b.Name == "TaiKhoan" && TypeLogin == 0)
            //{

            //}
            //else
            //    TypeLogin = TypeLogin == 1 ? 0 : 1;
            //if (TypeLogin == 1)
            //{
            //    spQRCode.Visibility = Visibility.Visible;
            //    Login_Account.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    Login_Account.Visibility = Visibility.Visible;
            //    spQRCode.Visibility = Visibility.Collapsed;
            //}
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            //tblValidateEmail.Text = "";
            //if (string.IsNullOrEmpty(txtEmail.Text))
            //{
            //    tblValidateEmail.Text = "Không được để trống";
            //}
            //else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            //{
            //    tblValidateEmail.Text = "Nhập không đúng định dạng";
            //}
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            //tblValidateEmail.Text = "";
            //if (string.IsNullOrEmpty(txtEmail.Text))
            //{
            //    tblValidateEmail.Text = "Không được để trống";
            //}
            //else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            //{
            //    tblValidateEmail.Text = "Nhập không đúng định dạng";
            //}
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Pass = txtPass.Password;
            //tblValidatePass.Text = "";
            //if (string.IsNullOrEmpty(Pass))
            //{
            //    tblValidatePass.Text = "Không được để trống";
            //}
        }

        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            //tblValidatePass.Text = "";
            //if (string.IsNullOrEmpty(Pass))
            //{
            //    tblValidatePass.Text = "Không được để trống";
            //}
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
            //ForgotPassword frm = new ForgotPassword(Main);
            //pnlHienThi.Children.Clear();
            //object content = frm.Content;
            //frm.Content = null;
            //pnlHienThi.Children.Add(content as UIElement);
            //pnlHienThi.Visibility = Visibility.Visible;
        }
        private string b = "";
        private void btnDangNhapGo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/login")))
                {
                    RestRequest request = new RestRequest();
                    request.Method = Method.Post;
                    request.AlwaysMultipartFormData = true;

                    request.AddParameter("account", tb_TaiKhoanDangNhap.Text);
                    request.AddParameter("password", tb_MatKhauGo.Password);
                    request.AddParameter("type", "1");
                    RestResponse resAlbum = restclient.Execute(request);
                    b = resAlbum.Content;
                    Popup.OOP.Login.clsLogin.Root receivedInfo = JsonConvert.DeserializeObject<Popup.OOP.Login.clsLogin.Root>(b);
                    if (receivedInfo.data != null)
                    {
                        if (receivedInfo.data.data.type == "1")
                        {
                            if (textLoaiTK.Text == "TÀI KHOẢN CÔNG TY")
                            {
                                textThongBao.Visibility = Visibility.Collapsed;
                                pnlMain.Visibility = Visibility.Visible;
                                btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                                btnCollapseSizebar.Visibility = Visibility.Visible;
                                pnlFullSizebar.Visibility = Visibility.Visible;
                                pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                                pnlDangKy.Visibility = Visibility.Collapsed;
                                docListBtn.Visibility = Visibility.Collapsed;
                                btnShowSPN.Visibility = Visibility.Visible;
                                IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                                textUserName.Text = receivedInfo.data.data.user_info.com_name;
                                //LoadTokenTL();
                                if (chkLuuTKMK.IsChecked == true)
                                {
                                    Properties.Settings.Default.User = tb_TaiKhoanDangNhap.Text;
                                    Properties.Settings.Default.Pass = tb_MatKhauGo.Password;
                                    Properties.Settings.Default.Check = "1";
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    Properties.Settings.Default.User = "";
                                    Properties.Settings.Default.Pass = "";
                                    Properties.Settings.Default.Check = "0";
                                    Properties.Settings.Default.Save();
                                }
                                //Properties.Settings.Default.Token = receivedInfo.data.data.access_token;
                                //this.Hide();
                                //MainWindow main = new MainWindow(receivedInfo.data, this);
                                //main.Show();
                                //textThongBao.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                textThongBao.Visibility = Visibility.Visible;
                            }
                        }
                        else if (receivedInfo.data.data.type == "2")
                        {
                            if (textLoaiTK.Text == "TÀI KHOẢN NHÂN VIÊN")
                            {
                                textThongBao.Visibility = Visibility.Collapsed;
                                pnlMain.Visibility = Visibility.Visible;
                                btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                                btnCollapseSizebar.Visibility = Visibility.Visible;
                                pnlFullSizebar.Visibility = Visibility.Visible;
                                pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                                pnlDangKy.Visibility = Visibility.Collapsed;
                                docListBtn.Visibility = Visibility.Collapsed;
                                btnShowSPN.Visibility = Visibility.Visible;
                                IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                                textUserName.Text = receivedInfo.data.data.user_info.com_name;
                                if (receivedInfo.data.data.user_info.com_logo != null)
                                {
                                    var img = new Uri("https://chamcong.24hpay.vn/upload/employee/" + receivedInfo.data.data.user_info.com_logo);
                                    BitmapImage bm = new BitmapImage(img);
                                    ImgAc.ImageSource = bm;
                                }

                                if (chkLuuTKMK.IsChecked == true)
                                {
                                    Properties.Settings.Default.UserEp = tb_TaiKhoanDangNhap.Text;
                                    Properties.Settings.Default.PassEp = tb_MatKhauGo.Password;
                                    Properties.Settings.Default.Check = "1";
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    Properties.Settings.Default.UserEp = "";
                                    Properties.Settings.Default.PassEp = "";
                                    Properties.Settings.Default.Check = "0";
                                    Properties.Settings.Default.Save();
                                }
                                Properties.Settings.Default.Token = receivedInfo.data.data.access_token;

                                //this.Hide();
                                //MainChamCong main = new MainChamCong(receivedInfo.data, this);
                                //main.Show();
                                //textThongBao.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                textThongBao.Visibility = Visibility.Visible;
                            }

                        }

                    }
                    else
                    {
                        textThongBao.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                textThongBao.Visibility = Visibility.Visible;
            }
        }

        private void tb_MatKhauGo_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tb_MatKhauGo.Password == "")
            {
                txtMK.Visibility = Visibility.Visible;
            }
            else
            {
                txtMK.Visibility = Visibility.Collapsed;
            }
        }

        private void LogOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pnlShowPopUp.Children.Add(new Popup.QuanLyChungSauDangNhap.PopUpDangXuat(this));
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (pnlDangNhapCTy.Visibility == Visibility.Visible)
            {
                if (e.Key == Key.Enter)
                {
                    try
                    {
                        using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/login")))
                        {
                            RestRequest request = new RestRequest();
                            request.Method = Method.Post;
                            request.AlwaysMultipartFormData = true;

                            request.AddParameter("account", tb_TaiKhoanDangNhap.Text);
                            request.AddParameter("password", tb_MatKhauGo.Password);
                            request.AddParameter("type", "1");
                            RestResponse resAlbum = restclient.Execute(request);
                            b = resAlbum.Content;
                            Popup.OOP.Login.clsLogin.Root receivedInfo = JsonConvert.DeserializeObject<Popup.OOP.Login.clsLogin.Root>(b);
                            if (receivedInfo.data != null)
                            {
                                if (receivedInfo.data.data.type == "1")
                                {
                                    if (textLoaiTK.Text == "TÀI KHOẢN CÔNG TY")
                                    {
                                        textThongBao.Visibility = Visibility.Collapsed;
                                        pnlMain.Visibility = Visibility.Visible;
                                        btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                                        btnCollapseSizebar.Visibility = Visibility.Visible;
                                        pnlFullSizebar.Visibility = Visibility.Visible;
                                        pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                                        pnlDangKy.Visibility = Visibility.Collapsed;
                                        docListBtn.Visibility = Visibility.Collapsed;
                                        btnShowSPN.Visibility = Visibility.Visible;
                                        IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                                        textUserName.Text = receivedInfo.data.data.user_info.com_name;
                                        //LoadTokenTL();
                                        if (chkLuuTKMK.IsChecked == true)
                                        {
                                            Properties.Settings.Default.User = tb_TaiKhoanDangNhap.Text;
                                            Properties.Settings.Default.Pass = tb_MatKhauGo.Password;
                                            Properties.Settings.Default.Check = "1";
                                            Properties.Settings.Default.Save();
                                        }
                                        else
                                        {
                                            Properties.Settings.Default.User = "";
                                            Properties.Settings.Default.Pass = "";
                                            Properties.Settings.Default.Check = "0";
                                            Properties.Settings.Default.Save();
                                        }
                                        //Properties.Settings.Default.Token = receivedInfo.data.data.access_token;
                                        //this.Hide();
                                        //MainWindow main = new MainWindow(receivedInfo.data, this);
                                        //main.Show();
                                        //textThongBao.Visibility = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        textThongBao.Visibility = Visibility.Visible;
                                    }
                                }
                                else if (receivedInfo.data.data.type == "2")
                                {
                                    if (textLoaiTK.Text == "TÀI KHOẢN NHÂN VIÊN")
                                    {
                                        textThongBao.Visibility = Visibility.Collapsed;
                                        pnlMain.Visibility = Visibility.Visible;
                                        btnDisplayFullSizebar.Visibility = Visibility.Collapsed;
                                        btnCollapseSizebar.Visibility = Visibility.Visible;
                                        pnlFullSizebar.Visibility = Visibility.Visible;
                                        pnlCollapseSizebar.Visibility = Visibility.Collapsed;
                                        pnlDangKy.Visibility = Visibility.Collapsed;
                                        docListBtn.Visibility = Visibility.Collapsed;
                                        btnShowSPN.Visibility = Visibility.Visible;
                                        IdAcount = receivedInfo.data.data.user_info.com_id.ToString();
                                        textUserName.Text = receivedInfo.data.data.user_info.com_name;
                                        if (receivedInfo.data.data.user_info.com_logo == null)
                                        {
                                            var imgDefault = new Uri("/Resource/image/CompanyLogo.png");
                                            BitmapImage bmDefault = new BitmapImage(imgDefault);
                                            ImgAc.ImageSource = bmDefault;
                                        }
                                        else
                                        {
                                            var img = new Uri("https://chamcong.24hpay.vn/upload/employee/" + receivedInfo.data.data.user_info.com_logo);
                                            BitmapImage bm = new BitmapImage(img);
                                            ImgAc.ImageSource = bm;
                                        }

                                        if (chkLuuTKMK.IsChecked == true)
                                        {
                                            Properties.Settings.Default.UserEp = tb_TaiKhoanDangNhap.Text;
                                            Properties.Settings.Default.PassEp = tb_MatKhauGo.Password;
                                            Properties.Settings.Default.Check = "1";
                                            Properties.Settings.Default.Save();
                                        }
                                        else
                                        {
                                            Properties.Settings.Default.UserEp = "";
                                            Properties.Settings.Default.PassEp = "";
                                            Properties.Settings.Default.Check = "0";
                                            Properties.Settings.Default.Save();
                                        }
                                        Properties.Settings.Default.Token = receivedInfo.data.data.access_token;

                                        //this.Hide();
                                        //MainChamCong main = new MainChamCong(receivedInfo.data, this);
                                        //main.Show();
                                        //textThongBao.Visibility = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        textThongBao.Visibility = Visibility.Visible;
                                    }

                                }

                            }
                            else
                            {
                                textThongBao.Visibility = Visibility.Visible;
                            }
                        }
                    }
                    catch
                    {
                        textThongBao.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void btnDKyEp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (textTKDangNhap.Text == "")
            {
                textThongBaoNhapSDT.Visibility = Visibility.Visible;
            }
            else if (textHoTen.Text == "")
            {
                textThongBaoHVT.Visibility = Visibility.Visible;
            }
            else if (textMatKhau.Password == "")
            {
                textThongBaoNhapMK.Visibility = Visibility.Visible;
            }
            else if (textNhapLaiMK.Password == "")
            {
                textThongBaoNhapLaiMK.Visibility = Visibility.Visible;
            }
            else if (textDiaChi.Text == "")
            {
                textThongBaoNhapDiaChi.Visibility = Visibility.Visible;
            }
            else if (dtpNgaySinh.Text == "")
            {
                textThongBaoChonNgaySinh.Visibility = Visibility.Visible;
            }
            else if (textMatKhau.Password != textNhapLaiMK.Password)
            {

            }

            else
            {
                try
                {
                    using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/employee/register")))
                    {
                        RestRequest request = new RestRequest();
                        request.Method = Method.Post;
                        request.AlwaysMultipartFormData = true;
                        request.AddParameter("email", textEmail.Text);
                        request.AddParameter("phoneTK", textTKDangNhap.Text);
                        request.AddParameter("userName", textHoTen.Text);
                        request.AddParameter("password", textMatKhau.Password);
                        request.AddParameter("address", textDiaChi.Text);
                        //request.AddParameter("dep_id", tb_TaiKhoanDangNhap.Text);
                        request.AddParameter("com_id", IdComRegister);
                        string[] NS = dtpNgaySinh.Text.Split(Convert.ToChar("/"));
                        string y = NS[2];
                        string d = NS[1];
                        string m = NS[0];
                        request.AddParameter("birthday", y + "-" + m + "-" + d);
                        if (cboGioiTinh.Text == "Nam")
                        {
                            request.AddParameter("gender", 0);

                        }
                        else if (cboGioiTinh.Text == "Nữ")
                        {
                            request.AddParameter("gender", 1);
                        }
                        if (cboTinhTrangHonNhan.Text == "Đã kết hôn")
                        {
                            request.AddParameter("married", 2);
                        }
                        else
                        {
                            request.AddParameter("married", 1);
                        }
                        if (cboKinhNghiemLv.Text == "Chưa có kinh nghiệm")
                        {
                            request.AddParameter("experience", 1);

                        }
                        else if (cboKinhNghiemLv.Text == "Dưới 1 năm kinh nghệm")
                        {
                            request.AddParameter("experience", 2);
                        }
                        else if (cboKinhNghiemLv.Text == "1 năm")
                        {
                            request.AddParameter("experience", 3);
                        }
                        else if (cboKinhNghiemLv.Text == "2 năm")
                        {
                            request.AddParameter("experience", 4);

                        }
                        else if (cboKinhNghiemLv.Text == "3 năm")
                        {
                            request.AddParameter("experience", 5);
                        }
                        else if (cboKinhNghiemLv.Text == "4 năm")
                        {
                            request.AddParameter("experience", 6);
                        }
                        else if (cboKinhNghiemLv.Text == "5 năm")
                        {
                            request.AddParameter("experience", 7);
                        }
                        else if (cboKinhNghiemLv.Text == "Trên 5 năm")
                        {
                            request.AddParameter("experience", 8);
                        }

                        RestResponse resAlbum = restclient.Execute(request);
                        var b = resAlbum.Content;
                        pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoDangKyTKThanhCong(textTKDangNhap.Text, textMatKhau.Password, this));

                    }

                }
                catch
                {

                }
            }
        }

        private void btnDkyCom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (txtSDT.Text == "")
            {
                textThongBaoNhapSDTCom.Visibility = Visibility.Visible;
            }
            else if (textTenCT.Text == "")
            {
                textThongBaoNhapTenCTCom.Visibility = Visibility.Visible;
            }
            else if (passboxEmailPassWordNhanVien.Password == "")
            {
                textThongBaoNhapMKCTCom.Visibility = Visibility.Visible;
            }
            else if (passboxEmailPassWordNL.Password == "")
            {
                textThongBaoNhapLaiMKCTCom.Visibility = Visibility.Visible;
            }
            else if (textDC.Text == "")
            {
                textThongBaoNhapDiaChiCom.Visibility = Visibility.Visible;
            }

            else if (passboxEmailPassWordNhanVien.Password != passboxEmailPassWordNL.Password)
            {
                textThongBaoNhapLaiMKCTCom.Text = "Mật khẩu không khớp";
                textThongBaoNhapLaiMKCTCom.Visibility = Visibility.Visible;
            }

            else
            {
                try
                {
                    using (RestClient restclient = new RestClient(new Uri("http://210.245.108.202:3000/api/qlc/Company/register")))
                    {
                        RestRequest request = new RestRequest();
                        request.Method = Method.Post;
                        request.AlwaysMultipartFormData = true;
                        request.AddParameter("phoneTK", txtSDT.Text);
                        request.AddParameter("userName", textTenCT.Text);
                        request.AddParameter("password", passboxEmailPassWordNhanVien.Password);
                        request.AddParameter("address", textDC.Text);

                        RestResponse resAlbum = restclient.Execute(request);
                        var b = resAlbum.Content;
                        pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoDangKyTKThanhCong(txtSDT.Text, passboxEmailPassWordNhanVien.Password, this));

                    }

                }
                catch
                {

                }
            }
        }

        private void btnAppQTNS_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (b == "")
            {
                pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoCanDangNhap(this));

            }
            else
            {
                LoginEmployeeEntity result = JsonConvert.DeserializeObject<LoginEmployeeEntity>(b);
                if (result != null)
                {

                    HomeView homeView = new HomeView(result);
                    homeView.Show();
                    this.Close();
                }
            }
        }
        private async Task Login()
        {
            // var httpClient = new HttpClient();

            // var httpRequestMessage = new HttpRequestMessage();
            // httpRequestMessage.Method = HttpMethod.Post;
            // string api = "http://210.245.108.202:3000/api/qlc/employee/login";
            // httpRequestMessage.RequestUri = new Uri(api);

            // var parameters = new List<KeyValuePair<string, string>>();
            // parameters.Add(new KeyValuePair<string, string>("account", txtEmail.Text));

            // string passLogin = "";

            // parameters.Add(new KeyValuePair<string, string>("password", passLogin));
            // parameters.Add(new KeyValuePair<string, string>("type", "2"));

            // Thiết lập Content
            //var content = new FormUrlEncodedContent(parameters);
            // httpRequestMessage.Content = content;

            // Thực hiện Post
            //var response = await httpClient.SendAsync(httpRequestMessage);

            // var responseContent = await response.Content.ReadAsStringAsync();

            // LoginEmployeeEntity result = JsonConvert.DeserializeObject<LoginEmployeeEntity>(responseContent);


            // if (result.data != null && result.data.result)
            // {

            //     HomeView homeView = new HomeView(result);
            //     homeView.Show();
            //     this.Close();
            // }

        }

        private void btnChamCong_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (b == "")
            {
                pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoCanDangNhap(this));

            }
            else
            {
                API_Login_Staff api = JsonConvert.DeserializeObject<API_Login_Staff>(b);
                if (api != null)
                {
                    ChamCong365.MainWindow home = new ChamCong365.MainWindow(api);
                    //home.WindowState = Login.WindowState;
                    var workingArea = System.Windows.SystemParameters.WorkArea;
                    home.Width = workingArea.Right - 100;
                    home.Height = workingArea.Bottom - 80;
                    home.Show();
                    this.Close();
                }
            }


        }

        private void bod_QuanLyNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (b == "")
            {
                pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoCanDangNhap(this));

            }
            else
            {
                API_Login_Staff api = JsonConvert.DeserializeObject<API_Login_Staff>(b);
                if (api != null)
                {
                    ucQuanLyNhanVien qlnv = new ucQuanLyNhanVien(this);
                    stp_ShowPopup.Children.Clear();
                    object Content = qlnv.Content;
                    qlnv.Content = null;
                    stp_ShowPopup.Children.Add(Content as UIElement);
                }
            }
        }

        private void bod_CoCau_ViTri_ToChuc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (b == "")
            {
                pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoCanDangNhap(this));

            }
            else
            {
                API_Login_Staff api = JsonConvert.DeserializeObject<API_Login_Staff>(b);
                if (api != null)
                {
                    ucCoCauToChuc_ViTri sodo = new ucCoCauToChuc_ViTri(this);
                    stp_ShowPopup.Children.Clear();
                    object Content = sodo.Content;
                    sodo.Content = null;
                    stp_ShowPopup.Children.Add(Content as UIElement);
                }
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (b == "")
            {
                pnlShowPopUp.Children.Add(new Popup.TruocDangNhap.PopUpThongBaoCanDangNhap(this));

            }
            else
            {
                ucRecommended uc = new ucRecommended(this);
                stp_ShowPopup.Children.Clear();
                object Content = uc.Content;
                uc.Content = null;
                stp_ShowPopup.Children.Add(Content as UIElement);
            }

        }
    }
}
