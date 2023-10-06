using QuanLyChung365TruocDangNhap.ThietLapCongTy.Popups.PopupQuanLyNhanVien;
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
    /// Interaction logic for ucTatCaNhanVien.xaml
    /// </summary>
    public partial class ucTatCaNhanVien : UserControl
    {
        BrushConverter br = new BrushConverter();
        frmMain Main;

        public class Saff
        {
            public int stt { get; set; }
            public string name { get; set; }
            public string id { get; set; }
            public string tochuc { get; set; }
            public string vitri { get; set; }
            public string sdt { get; set; }
        }
        List<Saff> saffList = new List<Saff>();
        public ucTatCaNhanVien(frmMain main)
        {
            InitializeComponent();
            Main = main;

            #region FackeValue
            for (int i = 0; i < 10; i++)
            {
                saffList.Add(new Saff() { stt = i, id = $"{i}", name = $"Ky{i}", tochuc = $"Tổ chức {i}", sdt = $"039303939{i}", vitri = $"Nhân Viên Cấp {i}"});
            }
            dgvDanhSachNhanVien.ItemsSource = saffList;
            #endregion
        }
        #region Hover Event
        private void bod_ThemMoiNhanVien_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_ThemMoiNhanVien.BorderThickness = new Thickness(2);
            bod_Pluss.BorderBrush = (Brush)br.ConvertFrom("#4C5BD4");
            tb_Pluss.Foreground = (Brush)br.ConvertFrom("#4C5BD4");
            tb_ThemMoiNhanVien.Foreground = (Brush)br.ConvertFrom("#4C5BD4");
        }

        private void bod_ThemMoiNhanVien_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_ThemMoiNhanVien.BorderThickness = new Thickness(0);
            bod_Pluss.BorderBrush = (Brush)br.ConvertFrom("#666666");
            tb_Pluss.Foreground = (Brush)br.ConvertFrom("#474747");
            tb_ThemMoiNhanVien.Foreground = (Brush)br.ConvertFrom("#474747");
        }
        private void bod_TimKiem_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_TimKiem.BorderThickness = new Thickness(2);
        }

        private void bod_TimKiem_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_TimKiem.BorderThickness = new Thickness(0);
        }
        
        private void bod_ViTriNhanVien_MouseEnter(object sender, MouseEventArgs e)
        {
            // Lấy hàng (row) được nhấn chuột
            DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row != null)
            {
                // Tìm Border có x:Name="bodXoaNhanVien" bên trong hàng
                Border bodThayDoiViTri = FindChild<Border>(row, "bodThayDoiViTri");

                if (bodThayDoiViTri != null)
                {
                    // Thực hiện xử lý khi chuột vào Border "bodXoaNhanVien"
                    // Ví dụ: Hiển thị nội dung khi chuột hover vào đó
                    bodThayDoiViTri.Visibility = Visibility.Visible;
                }
            }
        }
        private void bod_ViTriNhanVien_MouseLeave(object sender, MouseEventArgs e)
        {
            // Lấy hàng (row) được nhấn chuột
            DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row != null)
            {
                // Tìm Border có x:Name="bodXoaNhanVien" bên trong hàng
                Border bodThayDoiViTri = FindChild<Border>(row, "bodThayDoiViTri");

                if (bodThayDoiViTri != null)
                {
                    // Thực hiện xử lý khi chuột vào Border "bodXoaNhanVien"
                    // Ví dụ: Hiển thị nội dung khi chuột hover vào đó
                    bodThayDoiViTri.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void bod_LuanChuyenNhanVien_MouseEnter(object sender, MouseEventArgs e)
        {
            // Lấy hàng (row) được nhấn chuột
            DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row != null)
            {
                // Tìm Border có x:Name="bodXoaNhanVien" bên trong hàng
                Border bodLuanChuyenPhongBan = FindChild<Border>(row, "bodLuanChuyenPhongBan");

                if (bodLuanChuyenPhongBan != null)
                {
                    // Thực hiện xử lý khi chuột vào Border "bodXoaNhanVien"
                    // Ví dụ: Hiển thị nội dung khi chuột hover vào đó
                    bodLuanChuyenPhongBan.Visibility = Visibility.Visible;
                }
            }
        }
        private void bod_LuanChuyenNhanVien_MouseLeave(object sender, MouseEventArgs e)
        {
            // Lấy hàng (row) được nhấn chuột
            DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row != null)
            {
                // Tìm Border có x:Name="bodXoaNhanVien" bên trong hàng
                Border bodLuanChuyenPhongBan = FindChild<Border>(row, "bodLuanChuyenPhongBan");

                if (bodLuanChuyenPhongBan != null)
                {
                    // Thực hiện xử lý khi chuột vào Border "bodXoaNhanVien"
                    // Ví dụ: Hiển thị nội dung khi chuột hover vào đó
                    bodLuanChuyenPhongBan.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void bod_XoaNhanVien_MouseEnter(object sender, MouseEventArgs e)
        {

            // Lấy hàng (row) được nhấn chuột
            DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row != null)
            {
                // Tìm Border có x:Name="bodXoaNhanVien" bên trong hàng
                Border bodXoaNhanVien = FindChild<Border>(row, "bodXoaNhanVien");

                if (bodXoaNhanVien != null)
                {
                    // Thực hiện xử lý khi chuột vào Border "bodXoaNhanVien"
                    // Ví dụ: Hiển thị nội dung khi chuột hover vào đó
                    bodXoaNhanVien.Visibility = Visibility.Visible;
                }
            }
        }
        private void bod_XoaNhanVien_MouseLeave(object sender, MouseEventArgs e)
        {
            // Lấy hàng (row) được nhấn chuột
            DataGridRow row = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

            if (row != null)
            {
                // Tìm Border có x:Name="bodXoaNhanVien" bên trong hàng
                Border bodXoaNhanVien = FindChild<Border>(row, "bodXoaNhanVien");

                if (bodXoaNhanVien != null)
                {
                    // Thực hiện xử lý khi chuột vào Border "bodXoaNhanVien"
                    // Ví dụ: Hiển thị nội dung khi chuột hover vào đó
                    bodXoaNhanVien.Visibility = Visibility.Collapsed;
                }
            }
        }
        // Hàm giúp tìm kiếm đối tượng con trong VisualTree
        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && (child as FrameworkElement)?.Name == childName)
                {
                    return typedChild;
                }
                else
                {
                    T childOfChild = FindChild<T>(child, childName);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        // Hàm giúp tìm cha của một đối tượng trong VisualTree
        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
        #endregion

        #region Click Event
        private void bod_ThemMoiNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucThemMoiNhanVien ucadd = new ucThemMoiNhanVien(Main);
            Main.stp_ShowPopup.Children.Clear();
            object Content = ucadd.Content;
            ucadd.Content = null;
            Main.stp_ShowPopup.Children.Add(Content as UIElement);
        }

        private void bod_ViTriNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.pnlShowPopUp.Children.Add(new ucThayDoiViTri());
        }

        private void bod_LuanChuyenNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.pnlShowPopUp.Children.Add(new ucLuanChuyenPhongBan());
        }

        private void bod_XoaNhanVien_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.pnlShowPopUp.Children.Add(new ucXoaNhanVien());
        }
        #endregion


    }
}
