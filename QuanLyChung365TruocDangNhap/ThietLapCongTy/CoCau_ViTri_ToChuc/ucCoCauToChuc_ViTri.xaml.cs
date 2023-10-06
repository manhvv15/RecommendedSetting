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

namespace QuanLyChung365TruocDangNhap.ThietLapCongTy.CoCau_ViTri_ToChuc
{
    /// <summary>
    /// Interaction logic for ucCoCauToChuc_ViTri.xaml
    /// </summary>
    public partial class ucCoCauToChuc_ViTri : UserControl
    {
        BrushConverter br = new BrushConverter();
        frmMain Main;
        public ucCoCauToChuc_ViTri(frmMain main)
        {
            InitializeComponent();
            Main = main;
            bod_ThietLapCoCauToChuc.BorderThickness = new Thickness(0, 0, 0, 5);
            bod_ThietLapCoCauToChuc.BorderBrush = (Brush)br.ConvertFrom("#4C5DB4");
            bod_ThietLapCoCauToChuc.CornerRadius = new CornerRadius(10);
            tb_ThietLapCoCauToChuc.Foreground = (Brush)br.ConvertFrom("#4C5DB4");
            grHienThiSoDo.Children.Add(new ucSoDoToChuc(main));
           
        }
        #region Click Event
        private void ThietLapCoCauToChuc(object sender, MouseButtonEventArgs e)
        {
            bod_ThietLapCoCauToChuc.BorderThickness = new Thickness(0, 0, 0, 5);
            bod_ThietLapCoCauToChuc.BorderBrush = (Brush)br.ConvertFrom("#4C5DB4");
            bod_ThietLapCoCauToChuc.CornerRadius = new CornerRadius(10);
            tb_ThietLapCoCauToChuc.Foreground = (Brush)br.ConvertFrom("#4C5DB4");

            bod_SoDoViTri.BorderThickness = new Thickness(0);
            tb_SoDoViTri.Foreground = (Brush)br.ConvertFrom("#474747");
            if (grHienThiSoDo.Children != null)
            {
                grHienThiSoDo.Children.Clear();
                grHienThiSoDo.Children.Add(new ucSoDoToChuc(Main));
            }
        }

        private void SoDoViTri(object sender, MouseButtonEventArgs e)
        {
            bod_SoDoViTri.BorderThickness = new Thickness(0, 0, 0, 5);
            bod_SoDoViTri.BorderBrush = (Brush)br.ConvertFrom("#4C5DB4");
            bod_SoDoViTri.CornerRadius = new CornerRadius(10);
            tb_SoDoViTri.Foreground = (Brush)br.ConvertFrom("#4C5DB4");

            bod_ThietLapCoCauToChuc.BorderThickness = new Thickness(0);
            tb_ThietLapCoCauToChuc.Foreground = (Brush)br.ConvertFrom("#474747");
        }
        #endregion
    }
}
