using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
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
    /// Interaction logic for Info_ChamCong.xaml
    /// </summary>
    public partial class Info_ChamCong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string latitude;
        public string longitute;
        public string ipWifi;
        public APICheckFace DataEpFace;
        public string Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }
        public string Longitute
        {
            get { return longitute; }
            set
            {
                longitute = value;
                OnPropertyChanged("Longitute");
            }
        }
        public string IpWifi
        {
            get { return ipWifi; }
            set
            {
                ipWifi = value;
                OnPropertyChanged("IpWifi");
            }
        }

        public Info_ChamCong(ChamCong_Main main, List<ItemShift> shifts, APICheckFace dataEpFace)
        {
            InitializeComponent();
            Latitude = main.latitude;
            Longitute = main.longitute;
            IpWifi = main.ipWifi;
            DataEpFace = dataEpFace;
            ChamCongMain = main;
            this.DataContext = this;
            if (shifts != null && shifts.Count > 0)
            {
                ListViewShift.ItemsSource = shifts;
                ShiftNone.Visibility = Visibility.Collapsed;
            }
            else
            {
                ListViewShift.Visibility = Visibility.Collapsed;
                ShiftNone.Visibility = Visibility.Visible;
            }
        }
        private ChamCong_Main ChamCongMain;
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
            if (ListViewShift.SelectedIndex != -1)
            {
                if (ChamCongMain.Main.Type == 1)
                {
                    ChamCongMain.CheckFace(((ItemShift)ListViewShift.SelectedItem).shift_id, DataEpFace.user_id, DataEpFace.company_id);
                }
                else
                {
                    ChamCongMain.DiemDanh(((ItemShift)ListViewShift.SelectedItem).shift_id, DataEpFace);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ca!");
            }
        }

        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            if (ChamCongMain.IsWebCamInUse())
            {
                var x = new Error_NhanDien(ChamCongMain);
                x.Message = "Thiết bị ghi hình đang được sử dụng bởi một ứng dụng khác, ";
                x.Message2 = "hãy thử tắt các thiết bị phần mềm đang sử dụng thiết bị ghi hình và thử chấm công lại";
                ChamCongMain.Popup.NavigationService.Navigate(x);
                ChamCongMain.PopupChamCong.Visibility = Visibility.Visible;
            }
            else if (ChamCongMain.divices.Count <= 0)
            {
                var x = new PopUp.PopUp_ChamCong.Error_NhanDien(ChamCongMain);
                x.Message = "Không tìm thấy thiết bị ghi hình được kết nối";
                x.Message2 = "Hãy thử bật thiết bị ghi hình của bạn hoặc kết nối với thiết bị ghi hình dời và thử chấm công lại";
                ChamCongMain.Popup.NavigationService.Navigate(x);
                ChamCongMain.PopupChamCong.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                ChamCongMain.startCapture(true);
            }
        }
    }
}
