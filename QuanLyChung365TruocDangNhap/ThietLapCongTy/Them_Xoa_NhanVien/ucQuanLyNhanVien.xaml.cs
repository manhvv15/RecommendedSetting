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

namespace QuanLyChung365TruocDangNhap.ThietLapCongTy.Them_Xoa_NhanVien
{
    /// <summary>
    /// Interaction logic for ucQuanLyNhanVien.xaml
    /// </summary>
    public partial class ucQuanLyNhanVien : UserControl
    {
        BrushConverter br = new BrushConverter();
        frmMain Main;
        public ucQuanLyNhanVien(frmMain main)
        {
            InitializeComponent();
            bod_TatCaNhanVien.BorderThickness = new Thickness(0, 0, 0, 5);
            bod_TatCaNhanVien.BorderBrush = (Brush)br.ConvertFrom("#4C5DB4");
            bod_TatCaNhanVien.CornerRadius = new CornerRadius(10);
            tb_TatCaNhanVien.Foreground = (Brush)br.ConvertFrom("#4C5DB4");
            grHienThiNhanVien.Children.Add(new ucTatCaNhanVien(main));
            Main = main;
        }
        #region Even GiaoDien
        private void TatCaNhanVien(object sender, MouseButtonEventArgs e)
        {
            bod_TatCaNhanVien.BorderThickness = new Thickness(0, 0, 0, 5);
            bod_TatCaNhanVien.BorderBrush = (Brush)br.ConvertFrom("#4C5DB4");
            bod_TatCaNhanVien.CornerRadius = new CornerRadius(10);
            tb_TatCaNhanVien.Foreground = (Brush)br.ConvertFrom("#4C5DB4");

            bod_NhanVienChoDuyet.BorderThickness = new Thickness(0);
            tb_NhanVienChoDuyet.Foreground = (Brush)br.ConvertFrom("#474747");
            if (grHienThiNhanVien.Children != null)
            {
                grHienThiNhanVien.Children.Clear();
                grHienThiNhanVien.Children.Add(new ucTatCaNhanVien(Main));
            }
            
        }
        private void NhanVienChoDuyet(object sender, MouseButtonEventArgs e)
        {
            bod_NhanVienChoDuyet.BorderThickness = new Thickness(0, 0, 0, 5);
            bod_NhanVienChoDuyet.BorderBrush = (Brush)br.ConvertFrom("#4C5DB4");
            bod_NhanVienChoDuyet.CornerRadius = new CornerRadius(10);
            tb_NhanVienChoDuyet.Foreground = (Brush)br.ConvertFrom("#4C5DB4");

            bod_TatCaNhanVien.BorderThickness = new Thickness(0);
            tb_TatCaNhanVien.Foreground = (Brush)br.ConvertFrom("#474747");
            if (grHienThiNhanVien.Children != null)
            {
                grHienThiNhanVien.Children.Clear();
                grHienThiNhanVien.Children.Add(new ucNhanVienChoDuyet());
            }
           
        }
        #endregion

    }
}
