using QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.ChamCong;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_ChamCong
{
    /// <summary>
    /// Interaction logic for Attenden_Fail.xaml
    /// </summary>
    public partial class Attenden_Fail : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Attenden_Fail(ChamCong_Main chamCong_Main)
        {
            InitializeComponent();
            ChamCongMain = chamCong_Main;
            string h = DateTime.Now.Hour <10 ?"0"+ DateTime.Now.Hour.ToString(): DateTime.Now.Hour.ToString();
            string m = DateTime.Now.Minute <10 ?"0"+ DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString();

            Day = h + ":" + m + " - " + DateTime.Now.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));
            this.DataContext = this;
        }

        private ChamCong_Main ChamCongMain;
        private string day;

        public string Day
        {
            get { return day; }
            set { day = value; OnPropertyChanged("Day"); }
        }


        public bool Type
        {
            get { return (bool)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(bool), typeof(Attenden_Fail));

        public ImageSource Avatar
        {
            get { return (ImageSource)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(ImageSource), typeof(Attenden_Fail));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Attenden_Fail));

        private void TrangChu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (ChamCongMain.Main.Type)
            {
                case 1:
                    ChamCongMain.Main.SideBarIndexNV = 0;
                    break;
                case 2:
                    ChamCongMain.Main.SideBarIndex = 0;
                    break;
                default:
                    break;
            }
            ChamCongMain.Main.ClosePopup();
        }

        private void Attendence_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChamCongMain.startCapture(true);
        }
    }
}
