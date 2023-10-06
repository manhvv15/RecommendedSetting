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
    /// Interaction logic for ucNhanVienChoDuyet.xaml
    /// </summary>
    public partial class ucNhanVienChoDuyet : UserControl
    {
        BrushConverter br = new BrushConverter();
        public ucNhanVienChoDuyet()
        {
            InitializeComponent();
        }
        #region Hover Event
        private void bod_XoaNhanVienChoDuyet_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_XoaNhanVienChoDuyet.Background = (Brush)br.ConvertFrom("#4C5BD4");
            tb_XoaNhanVienChoDuyet.Foreground = (Brush)br.ConvertFrom("#FFFFFF");
        }

        private void bod_XoaNhanVienChoDuyet_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_XoaNhanVienChoDuyet.Background = (Brush)br.ConvertFrom("#FFFFFF");
            tb_XoaNhanVienChoDuyet.Foreground = (Brush)br.ConvertFrom("#4C5BD4");
        }

        private void bod_Duyet_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_Duyet.BorderThickness = new Thickness(1);
            bod_Duyet.Background = (Brush)br.ConvertFrom("#339DFA");
        }

        private void bod_Duyet_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_Duyet.BorderThickness = new Thickness(0);
            bod_Duyet.Background = (Brush)br.ConvertFrom("#4C5BD4");
        }

        private void bod_DuyetTatCa_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_DuyetTatCa.BorderThickness = new Thickness(1);
            bod_DuyetTatCa.Background = (Brush)br.ConvertFrom("#339DFA");
        }

        private void bod_DuyetTatCa_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_DuyetTatCa.BorderThickness = new Thickness(0);
            bod_DuyetTatCa.Background = (Brush)br.ConvertFrom("#4C5BD4");
        }
        private void bod_TimKiem_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_TimKiem.BorderThickness = new Thickness(2);
            bod_TimKiem.Background = (Brush)br.ConvertFrom("#339DFA");
        }

        private void bod_TimKiem_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_TimKiem.BorderThickness = new Thickness(0);
            bod_TimKiem.Background = (Brush)br.ConvertFrom("#4C5BD4");
        }
        #endregion


    }
}
