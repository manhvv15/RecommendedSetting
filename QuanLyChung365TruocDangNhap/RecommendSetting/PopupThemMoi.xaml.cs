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

namespace QuanLyChung365TruocDangNhap.RecommendSetting
{
    /// <summary>
    /// Interaction logic for PopupThemMoi.xaml
    /// </summary>
    public partial class PopupThemMoi : UserControl
    {
        ucRecommended Uc;
        PopupThemMoi Pop;
        public PopupThemMoi(ucRecommended uc, PopupThemMoi pop)
        {
            InitializeComponent();
            Uc = uc;
            Pop = pop;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
