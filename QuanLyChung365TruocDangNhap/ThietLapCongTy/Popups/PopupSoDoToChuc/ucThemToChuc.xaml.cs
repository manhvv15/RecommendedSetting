using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Emgu.Util.Platform;
using static QuanLyChung365TruocDangNhap.ThietLapCongTy.Them_Xoa_NhanVien.ucTatCaNhanVien;

namespace QuanLyChung365TruocDangNhap.ThietLapCongTy.Popups.PopupSoDoToChuc
{
    /// <summary>
    /// Interaction logic for ucThemToChuc.xaml
    /// </summary>
    public partial class ucThemToChuc : UserControl
    {
        BrushConverter br = new BrushConverter();
        ObservableCollection<String> themThongTin = new ObservableCollection<String>();
        public class Saff1
        {
            public string name { get; set; }
            public string id { get; set; }
            public string tochuc { get; set; }
            public string vitri { get; set; }
        }
        List<Saff1> saffList1 = new List<Saff1>();
        public ucThemToChuc()
        {
            InitializeComponent();
            lsvAddInfo.ItemsSource = themThongTin;
            #region FACKE
            for (int i = 0; i < 10; i++)
            {
                saffList1.Add(new Saff1() {  id = $"{i}", name = $"Ky{i}", tochuc = $"Tổ chức {i}", vitri = $"Nhân Viên Cấp {i}" });
            }
            
            #endregion
        }

        #region Click Event
        private void btn_ThemThongTin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            themThongTin.Add($"Mục mới thêm {themThongTin.Count + 1}");
        }

        private void XoaDoiTuong_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (themThongTin.Count > 0)
                themThongTin.RemoveAt(themThongTin.Count - 1);
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void bodExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void bodLuuThongTinNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        private void bodQuayLai_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility= Visibility.Collapsed;
        }

        public bool shouldProcessEvent = true;
        private void bod_TimKiemNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (shouldProcessEvent)
            {
                if (bod_DSNhanVien.Visibility == Visibility.Collapsed)
                {
                    bod_DSNhanVien.Visibility = Visibility.Visible;
                    popup.Visibility = Visibility.Visible;

                }
                else
                {
                    bod_DSNhanVien.Visibility = Visibility.Collapsed;
                    popup.Visibility = Visibility.Collapsed;

                }
            }
        }
        private void lsvCheckNhanVien_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollListNhanVien.ScrollToVerticalOffset(scrollListNhanVien.VerticalOffset - e.Delta);
        }
        private void popup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (bod_DSNhanVien.Visibility == Visibility.Visible)
            {
                bod_DSNhanVien.Visibility = Visibility.Collapsed;
                popup.Visibility = Visibility.Collapsed;

            }
        }
        private void xoaNvDaChon_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

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
            bodLuuThongTinNhanVien.Background = (Brush)br.ConvertFrom("#339DFA");
        }

        private void bodLuuThongTinNhanVien_MouseLeave(object sender, MouseEventArgs e)
        {
            bodLuuThongTinNhanVien.BorderThickness = new Thickness(0);
            bodLuuThongTinNhanVien.Background = (Brush)br.ConvertFrom("#4C5DB4");
        }

        private void btn_ThemThongTin_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_ThemThongTin.BorderThickness = new Thickness(1);
            btn_ThemThongTin.Background = (Brush)br.ConvertFrom("#FF6048");
        }

        private void btn_ThemThongTin_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_ThemThongTin.BorderThickness = new Thickness(0);
            btn_ThemThongTin.Background = (Brush)br.ConvertFrom("#FF7A00");
        }




        #endregion

      
    }
}
