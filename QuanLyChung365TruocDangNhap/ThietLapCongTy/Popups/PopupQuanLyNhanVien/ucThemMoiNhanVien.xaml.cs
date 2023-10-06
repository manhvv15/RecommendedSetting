using QuanLyChung365TruocDangNhap.ThietLapCongTy.Them_Xoa_NhanVien;
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

namespace QuanLyChung365TruocDangNhap.ThietLapCongTy.Popups.PopupQuanLyNhanVien
{
    /// <summary>
    /// Interaction logic for ucThemMoiNhanVien.xaml
    /// </summary>
    public partial class ucThemMoiNhanVien : UserControl
    {
        frmMain Main;
        BrushConverter br = new BrushConverter();
        public ucThemMoiNhanVien(frmMain main)
        {
            InitializeComponent();
            Main = main;
        }
        bool allow = true;
        public void ValidateAddSaff()
        {
            
            if (string.IsNullOrEmpty(textNhapHoTen.Text))
            {
                allow = false;
                tb_ValidateHoVaTen.Visibility = Visibility.Visible;
                tb_ValidateHoVaTen.Text = "Họ tên không được để trống";
            }
            else
            {
                tb_ValidateHoVaTen.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(textEmail2.Text))
            {
                allow = false;
                tb_ValidateEmail.Visibility = Visibility.Visible;
                tb_ValidateEmail.Text = "Email không được để trống";
            }
            else
            {
                tb_ValidateEmail.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(textSoDT.Text))
            {
                allow = false;
                tb_ValidateSDT.Visibility = Visibility.Visible;
                tb_ValidateSDT.Text = "Email không được để trống";
            }
            else
            {
                tb_ValidateSDT.Visibility = Visibility.Collapsed;
            }
            if (cboTinhThanh.Text == null)
            {
                allow = false;
                tb_ValidateTinhThanh.Visibility = Visibility.Visible;
                tb_ValidateTinhThanh.Text = "không được để trống";
            }
            else
            {
                tb_ValidateTinhThanh.Visibility = Visibility.Collapsed;
            }
            if (cboQuanHuyen.Text == null)
            {
                allow = false;
                tb_ValidateQuanHuyen.Visibility = Visibility.Visible;
                tb_ValidateQuanHuyen.Text = "không được để trống";
            }
            else
            {
                tb_ValidateQuanHuyen.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(textDiaChi2.Text))
            {
                allow = false;
                tb_ValidateDiaChi.Visibility = Visibility.Visible;
                tb_ValidateDiaChi.Text = "Địa chỉ không được để trống";
            }
            else
            {
                tb_ValidateDiaChi.Visibility = Visibility.Collapsed;
            }
            if (cboGioiTinh.Text == null)
            {
                allow = false;
                tb_ValidateGioiTinh.Visibility = Visibility.Visible;
                tb_ValidateGioiTinh.Text = "Giới Tính không được để trống";
            }
            else
            {
                tb_ValidateGioiTinh.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(textPhoneTaiKhoan.Text))
            {
                allow = false;
                tb_ValidatePhoneTaiKhoan.Visibility = Visibility.Visible;
                tb_ValidatePhoneTaiKhoan.Text = "không được để trống";
            }
            else
            {
                tb_ValidatePhoneTaiKhoan.Visibility = Visibility.Collapsed;
            }
            if (cboLoaiToChuc.Text == null)
            {
                allow = false;
                tb_ValidateLoaiToChuc.Visibility = Visibility.Visible;
                tb_ValidateLoaiToChuc.Text = "không được để trống";
            }
            else
            {
                tb_ValidateLoaiToChuc.Visibility = Visibility.Collapsed;
            }
            if (cboTenToChuc.Text == null)
            {
                allow = false;
                tb_ValidateTenToChuc.Visibility = Visibility.Visible;
                tb_ValidateTenToChuc.Text = "không được để trống";
            }
            else
            {
                tb_ValidateTenToChuc.Visibility = Visibility.Collapsed;
            }
            if (cboViTri.Text == null)
            {
                allow = false;
                tb_ValidateViTri.Visibility = Visibility.Visible;
                tb_ValidateViTri.Text = "không được để trống";
            }
            else
            {
                tb_ValidateViTri.Visibility = Visibility.Collapsed;
            }
        }

        #region Clicl Event
        private void bodLuuThongTinNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ValidateAddSaff();
        }

        private void bodQuayLai_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucQuanLyNhanVien qlnv = new ucQuanLyNhanVien(Main);
            Main.stp_ShowPopup.Children.Clear();
            object Content = qlnv.Content;
            qlnv.Content = null;
            Main.stp_ShowPopup.Children.Add(Content as UIElement);
        }
        #endregion

        #region Hover Event
        private void bodQuayLai_MouseEnter(object sender, MouseEventArgs e)
        {
            bodQuayLai.Background = (Brush)br.ConvertFrom("#4C5DB4");
            tb_QuayLai.Foreground = (Brush)br.ConvertFrom("#FFFFFF");
        }

        private void bodQuayLai_MouseLeave(object sender, MouseEventArgs e)
        {
            bodQuayLai.Background = (Brush)br.ConvertFrom("#FFFFFF");
            tb_QuayLai.Foreground = (Brush)br.ConvertFrom("#4C5DB4");
        }

        private void bodLuuThongTinNhanVien_MouseEnter(object sender, MouseEventArgs e)
        {
            bodLuuThongTinNhanVien.BorderThickness = new Thickness(1);
        }

        private void bodLuuThongTinNhanVien_MouseLeave(object sender, MouseEventArgs e)
        {
            bodLuuThongTinNhanVien.BorderThickness = new Thickness(0);
        }
        #endregion
    }
}
