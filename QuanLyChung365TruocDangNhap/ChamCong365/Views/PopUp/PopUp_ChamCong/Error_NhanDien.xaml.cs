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
    /// Interaction logic for Error_NhanDien.xaml
    /// </summary>
    public partial class Error_NhanDien : Page, INotifyPropertyChanged
    {
        public enum EditType { DiemDanh, UpdateFace }

        private EditType _Type;
        public EditType Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Error_NhanDien(ChamCong_Main chamCong_Main)
        {
            InitializeComponent();
            this.DataContext = this;
            ChamCongMain = chamCong_Main;
            Type = EditType.DiemDanh;
        }
        public Error_NhanDien(ChamCong_Main chamCong_Main, EditType type)
        {
            InitializeComponent();
            this.DataContext = this;
            ChamCongMain = chamCong_Main;
            Type = type;
        }

        public Action Accept
        {
            get { return (Action)GetValue(AcceptProperty); }
            set { SetValue(AcceptProperty, value); }
        }
        public static readonly DependencyProperty AcceptProperty =
            DependencyProperty.Register("Accept", typeof(Action), typeof(Error_NhanDien));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Error_NhanDien));

        public string Message2
        {
            get { return (string)GetValue(Message2Property); }
            set { SetValue(Message2Property, value); }
        }
        public static readonly DependencyProperty Message2Property =
            DependencyProperty.Register("Message2", typeof(string), typeof(Error_NhanDien));
        public object ContentNoti
        {
            get { return (object)GetValue(ContentNotiProperty); }
            set { SetValue(ContentNotiProperty, value); }
        }
        public static readonly DependencyProperty ContentNotiProperty =
            DependencyProperty.Register("ContentNoti", typeof(object), typeof(Error_NhanDien));
        public ChamCong_Main ChamCongMain { get; set; }

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
            //else if (ChamCongMain.divices.Count <= 0)
            //{
            //    var x = new PopUp.PopUp_ChamCong.Error_NhanDien(ChamCongMain);
            //    x.Message = "Không tìm thấy thiết bị ghi hình được kết nối";
            //    x.Message2 = "Hãy thử bật thiết bị ghi hình của bạn hoặc kết nối với thiết bị ghi hình dời và thử chấm công lại";
            //    ChamCongMain.Popup.NavigationService.Navigate(x);
            //    ChamCongMain.PopupChamCong.Visibility = Visibility.Visible;
            //}
            else
            {
                this.Visibility = Visibility.Collapsed;
                ChamCongMain.startCapture(true);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void DiemdanhLai(object sender, MouseButtonEventArgs e)
        {
            if (ChamCongMain.IsWebCamInUse())
            {
                var x = new Error_NhanDien(ChamCongMain);
                x.Message = "Thiết bị ghi hình đang được sử dụng bởi một ứng dụng khác, ";
                x.Message2 = "hãy thử tắt các thiết bị phần mềm đang sử dụng thiết bị ghi hình và thử chấm công lại";
                ChamCongMain.Popup.NavigationService.Navigate(x);
                ChamCongMain.PopupChamCong.Visibility = Visibility.Visible;
            }
            //else if (ChamCongMain.divices.Count <= 0)
            //{
            //    var x = new PopUp.PopUp_ChamCong.Error_NhanDien(ChamCongMain);
            //    x.Message = "Không tìm thấy thiết bị ghi hình được kết nối";
            //    x.Message2 = "Hãy thử bật thiết bị ghi hình của bạn hoặc kết nối với thiết bị ghi hình dời và thử chấm công lại";
            //    ChamCongMain.Popup.NavigationService.Navigate(x);
            //    ChamCongMain.PopupChamCong.Visibility = Visibility.Visible;
            //}
            else
            {
                switch (Type)
                {
                    case EditType.DiemDanh:
                        this.Visibility = Visibility.Collapsed;
                        ChamCongMain.startCapture(true);
                        //ChamCongMain.startChamCong();
                        break;
                    case EditType.UpdateFace:
                        this.Visibility = Visibility.Collapsed;
                        ChamCongMain.startCapture(true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
