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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecentDeletedDataPopups
{
    /// <summary>
    /// Interaction logic for RestoreNoDataPopup.xaml
    /// </summary>
    public partial class RestoreNoDataPopup : UserControl
    {
        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public RestoreNoDataPopup()
        {
            InitializeComponent();
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
    }
}
