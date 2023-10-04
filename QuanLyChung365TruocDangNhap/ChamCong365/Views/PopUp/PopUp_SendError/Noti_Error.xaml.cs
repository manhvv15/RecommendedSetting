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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_SendError
{
    /// <summary>
    /// Interaction logic for Noti_Error.xaml
    /// </summary>
    public partial class Noti_Error : Page
    {
        public Noti_Error(MainWindow main)
        {
            InitializeComponent();
            Main = main;
        }
        public MainWindow Main;
        private void ExitPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }
    }
}
