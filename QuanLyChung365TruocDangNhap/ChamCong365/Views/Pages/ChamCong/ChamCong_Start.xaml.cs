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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.ChamCong
{
    /// <summary>
    /// Interaction logic for ChamCong.xaml
    /// </summary>
    public partial class ChamCong_Start : Page
    {
        public ChamCong_Start(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        //
        public MainWindow Main { get; set; }
        //
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var z= Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as MainWindow;
            //z.ClosePopup();

            var z = new Views.Pages.ChamCong.ChamCong_Main(Main);
            Main.ShowPopup(z);

        }
    }
}
