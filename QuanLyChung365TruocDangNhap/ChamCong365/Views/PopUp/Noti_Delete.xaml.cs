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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp
{
    /// <summary>
    /// Interaction logic for Noti_Delete.xaml
    /// </summary>
    public partial class Noti_Delete : Page
    {
        public Noti_Delete(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        //
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Noti_Delete));



        public Action Accept
        {
            get { return (Action)GetValue(AcceptProperty); }
            set { SetValue(AcceptProperty, value); }
        }
        public static readonly DependencyProperty AcceptProperty =
            DependencyProperty.Register("Accept", typeof(Action), typeof(Noti_Delete));



        public MainWindow Main { get; set; }
        //
        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Accept != null) Accept();
            Main.ClosePopup();
        }
    }
}
