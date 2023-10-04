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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_Rate
{
    /// <summary>
    /// Interaction logic for Noti_Rate.xaml
    /// </summary>
    public partial class Noti_Rate : Page
    {
        public Noti_Rate(MainWindow main)
        {
            InitializeComponent();
            Main = main;
        }
        private MainWindow Main;
        private void ExitPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }
    }
}
