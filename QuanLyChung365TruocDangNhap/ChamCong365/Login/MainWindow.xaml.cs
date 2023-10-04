using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindow()
        {
            this.DataContext = this;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ComEmail) && !string.IsNullOrEmpty(Properties.Settings.Default.ComPass))
            {
                runLoginCom(Properties.Settings.Default.ComEmail, Properties.Settings.Default.ComPass);
                this.Hide();
            }
            else if (!string.IsNullOrEmpty(Properties.Settings.Default.EpEmail) && !string.IsNullOrEmpty(Properties.Settings.Default.EpPass))
            {
                runLoginEp(Properties.Settings.Default.EpEmail, Properties.Settings.Default.EpPass);
                this.Hide();
            }
            else
            {
                InitializeComponent();
                var workingArea = System.Windows.SystemParameters.WorkArea;
                this.Width = workingArea.Right - 180;
                this.Height = workingArea.Bottom - 100;
                listShowx = listShow;
            }
        }
        //
        public List<string> listTab { get; set; } = new List<string> { "Tất cả", "Quản lý nhân lực", "Quản lý công việc", "Quản lý nội bộ", "Quản lý bán hàng" };
        public class ShowOffItem
        {
            public object Show { get; set; }
            public string Name { get; set; }
            public string Content { get; set; }
            public string Link { get; set; }
            public int Type { get; set; }
        }
        public List<ShowOffItem> listShow { get; set; } = new List<ShowOffItem>()
        {
            new ShowOffItem(){
                Show=App.Current.Resources["vbChamCong"],
                Name=App.Current.Resources["textAppChamCong"] as string,
                Content=App.Current.Resources["textAppChamCongContent"] as string,
                Link="https://chamcong.timviec365.vn/",
                Type=1,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbTinhLuong"],
                Name=App.Current.Resources["textAppTinhLuong"] as string,
                Content=App.Current.Resources["textAppTinhLuongContent"] as string,
                Link="https://tinhluong.timviec365.vn/",
                Type=1,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbNhanSu"],
                Name=App.Current.Resources["textAppNhanSua"] as string,
                Content=App.Current.Resources["textAppNhanSuaContent"] as string,
                Link="https://phanmemnhansu.timviec365.vn/",
                Type=1,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbPC365"],
                Name=App.Current.Resources["textAppPC365"] as string,
                Content=App.Current.Resources["textAppPC365Content"] as string,
                Type=1,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbDanhGiaNhanLuc"],
                Name=App.Current.Resources["textAppDanhGiaNhanLuc"] as string,
                Content=App.Current.Resources["textAppDanhGiaNhanLucContent"] as string,
                Link="https://phanmemdanhgiananglucnhanvien.timviec365.vn/",
                Type=1,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLCuocHop"],
                Name=App.Current.Resources["textAppQLCuocHop"] as string,
                Content=App.Current.Resources["textAppQLCuocHopContent"] as string,
                Type=1,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLTaiChinh"],
                Name=App.Current.Resources["textAppQLTaiChinh"] as string,
                Content=App.Current.Resources["textAppQLTaiChinhContent"] as string,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLKPI"],
                Name=App.Current.Resources["textAppQLKPI"] as string,
                Content=App.Current.Resources["textAppQLKPIContent"] as string,
                Link="https://kpi.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbGiaoViec"],
                Name=App.Current.Resources["textAppGiaoViec"] as string,
                Content=App.Current.Resources["textAppGiaoViecContent"] as string,
                Type=2,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbVanThu"],
                Name=App.Current.Resources["textAppVanThu"] as string,
                Content=App.Current.Resources["textAppVanThuContent"] as string,
                Link="https://vanthu.timviec365.vn/",
                Type=3,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbTruyenThong"],
                Name=App.Current.Resources["textAppTruyenThong"] as string,
                Content=App.Current.Resources["textAppTruyenThongContent"] as string,
                Link="https://truyenthongnoibo.timviec365.vn/",
                Type=3,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbGiongNoi"],
                Name=App.Current.Resources["textAppGiongNoi"] as string,
                Content=App.Current.Resources["textAppGiongNoiContent"] as string,
                Link="https://chuyenvanbanthanhgiongnoi.timviec365.vn/",
                Type=3,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLTaiSan"],
                Name=App.Current.Resources["textAppQLTaiSan"] as string,
                Content=App.Current.Resources["textAppQLTaiSanContent"] as string,
                Link="https://phanmemquanlytaisan.timviec365.vn/",
                Type=3,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbPhienDich"],
                Name=App.Current.Resources["textAppPhienDich"] as string,
                Content=App.Current.Resources["textAppPhienDichContent"] as string,
                Link="https://bienphiendich.timviec365.vn/",
                Type=3,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLBanHang"],
                Name=App.Current.Resources["textAppQLBanHang"] as string,
                Content=App.Current.Resources["textAppQLBanHangContent"] as string,
                Link="",
                Type=4,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbCRM"],
                Name=App.Current.Resources["textAppCRM"] as string,
                Content=App.Current.Resources["textAppCRMContent"] as string,
                Link="https://crm.timviec365.vn/",
                Type=4,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbDMS"],
                Name=App.Current.Resources["textAppDMS"] as string,
                Content=App.Current.Resources["textAppDMSContent"] as string,
                Link="https://dms.timviec365.vn/",
                Type=4,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbLoyalty"],
                Name=App.Current.Resources["textAppLoyalty"] as string,
                Content=App.Current.Resources["textAppLoyaltyContent"] as string,
                Link="",
                Type=4,
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbGara"],
                Name=App.Current.Resources["textAppGara"] as string,
                Content=App.Current.Resources["textAppGaraContent"] as string,
                Link="https://phanmemquanlygaraoto.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLCungUng"],
                Name=App.Current.Resources["textAppQLCungUng"] as string,
                Content=App.Current.Resources["textAppQLCungUngContent"] as string,
                Link="https://phanmemquanlycungung.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbQLCongTrinh"],
                Name=App.Current.Resources["textAppQLCongTrinh"] as string,
                Content=App.Current.Resources["textAppQLCongTrinhContent"] as string,
                Link="https://phanmemquanlycongtrinh.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbKhoXayDung"],
                Name=App.Current.Resources["textAppKhoXayDung"] as string,
                Content=App.Current.Resources["textAppKhoXayDungContent"] as string,
                Link="https://phanmemquanlykhoxaydung.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbLichBieu"],
                Name=App.Current.Resources["textAppLichBieu"] as string,
                Content=App.Current.Resources["textAppLichBieuContent"] as string,
                Link="https://lichbieu.timviec365.vn/",
                Type=2,
            },
        };

        private List<ShowOffItem> _listShowx;
        public List<ShowOffItem> listShowx
        {
            get { return _listShowx; }
            set { _listShowx = value; OnPropertyChanged(); }
        }

        public List<ShowOffItem> listDoanhNghiep { get; set; } = new List<ShowOffItem>
        {
            new ShowOffItem(){
                Show=App.Current.Resources["vbNhaTro"],
                Name=App.Current.Resources["textNhaTro"] as string,
                Content=App.Current.Resources["textNhaTroContent"] as string,
                Link="https://nhatro.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbViecLam"],
                Name=App.Current.Resources["textViecLam"] as string,
                Content=App.Current.Resources["textViecLamContent"] as string,
                Link="https://freelancer.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbViecLamTheoGioi"],
                Name=App.Current.Resources["textViecLamTheoGioi"] as string,
                Content=App.Current.Resources["textViecLamTheoGioiContent"] as string,
                Link="https://vieclamtheogio.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbGiaSu"],
                Name=App.Current.Resources["textGiaSu"] as string,
                Content=App.Current.Resources["textGiaSuContent"] as string,
                Link="https://giasu.timviec365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbKhoaHoc"],
                Name=App.Current.Resources["textKhoaHoc"] as string,
                Content=App.Current.Resources["textKhoaHocContent"] as string,
                Link="",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbTrangVang"],
                Name=App.Current.Resources["textTrangVang"] as string,
                Content=App.Current.Resources["textTrangVangContent"] as string,
                Link="https://timviec365.vn/trang-vang-doanh-nghiep.html",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbTraLuong"],
                Name=App.Current.Resources["textTraLuong"] as string,
                Content=App.Current.Resources["textTraLuongContent"] as string,
                Link="https://timviec365.vn/ssl/so-sanh-luong.html",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbGiaoVat"],
                Name=App.Current.Resources["textGiaoVat"] as string,
                Content=App.Current.Resources["textGiaoVatContent"] as string,
                Link="https://raonhanh365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbVeMayBay"],
                Name=App.Current.Resources["textVeMayBay"] as string,
                Content=App.Current.Resources["textVeMayBayContent"] as string,
                Link="https://raonhanh365.vn/",
            },
            new ShowOffItem(){
                Show=App.Current.Resources["vbThe"],
                Name=App.Current.Resources["textThe"] as string,
                Content=App.Current.Resources["textTheContent"] as string,
                Link="https://banthe24h.vn/",
            },
        };

        private double _ItemSize;
        public double ItemSize
        {
            get { return _ItemSize; }
            set { _ItemSize = value; OnPropertyChanged(); }
        }

        private Thickness _ItemTypeMargin = new Thickness(30, 0, 30, 0);
        public Thickness ItemTypeMargin
        {
            get { return _ItemTypeMargin; }
            set { _ItemTypeMargin = value; OnPropertyChanged(); }
        }

        private int _IsFull = 0;
        public int IsFull
        {
            get { return _IsFull; }
            set
            {
                _IsFull = value;
                var workingArea = System.Windows.SystemParameters.WorkArea;
                switch (IsFull)
                {
                    case 0:
                        this.WindowState = WindowState.Normal;
                        Width = workingArea.Right - 180;
                        Height = workingArea.Bottom - 100;
                        Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                        Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
                        this.ResizeMode = ResizeMode.CanResize;
                        break;
                    case 1:
                        this.WindowState = WindowState.Normal;
                        Left = workingArea.TopLeft.X;
                        Top = workingArea.TopLeft.Y;
                        Width = workingArea.Width;
                        Height = workingArea.Height;
                        this.ResizeMode = ResizeMode.NoResize;
                        break;
                    default:
                        break;
                }
                OnPropertyChanged();
            }
        }

        //
        private async void runLoginCom(string email, string pass)
        {
            try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("account", email);
                form.Add("password", pass);
                form.Add("type", "1");
                if (Properties.Settings.Default.ComTypePass == "1")
                {
                    form.Add("passtype", "1");
                }
                HttpClient httpClient = new HttpClient();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api = JsonConvert.DeserializeObject<QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result)
                {
                    var main = new QuanLyChung365TruocDangNhap.ChamCong365.MainWindow(api);

                    var workingArea = System.Windows.SystemParameters.WorkArea;
                    main.WindowState = WindowState.Normal;
                    main.Width = workingArea.Right - 180;
                    main.Height = workingArea.Bottom - 100;
                    main.Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                    main.Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
                    main.ResizeMode = ResizeMode.CanResize;
                    main.LogOut = () =>
                    {
                        this.IsFull = main.IsFull;
                        this.Width = main.Width;
                        this.Height = main.Height;
                        this.Left = main.Left;
                        this.Top = main.Top;
                        main.Close();
                        this.Show();

                    };

                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    InitializeComponent();
                    var workingArea = System.Windows.SystemParameters.WorkArea;
                    this.Width = workingArea.Right - 180;
                    this.Height = workingArea.Bottom - 100;
                    listShowx = listShow;
                    this.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                InitializeComponent();
                var workingArea = System.Windows.SystemParameters.WorkArea;
                this.Width = workingArea.Right - 180;
                this.Height = workingArea.Bottom - 100;
                listShowx = listShow;
                this.ShowDialog();
            }
        }
        private async void runLoginEp(string email, string pass)
        {
            try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("account", email);
                form.Add("password", pass);
                form.Add("type", "2");
                if(Properties.Settings.Default.EpTypePass == "1")
                {
                    form.Add("passtype", "1");
                }
                HttpClient httpClient = new HttpClient();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff.API_Login_Staff api = JsonConvert.DeserializeObject<QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff.API_Login_Staff>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.data.result)
                {
                    var main = new QuanLyChung365TruocDangNhap.ChamCong365.MainWindow(api);

                    var workingArea = System.Windows.SystemParameters.WorkArea;
                    main.WindowState = WindowState.Normal;
                    main.Width = workingArea.Right - 180;
                    main.Height = workingArea.Bottom - 100;
                    main.Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                    main.Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
                    main.ResizeMode = ResizeMode.CanResize;

                    main.LogOut = () =>
                    {
                        this.IsFull = main.IsFull;
                        this.Width = main.Width;
                        this.Height = main.Height;
                        this.Left = main.Left;
                        this.Top = main.Top;
                        main.Close();
                        this.Show();

                    };

                    this.Hide();
                    main.Show();
                }
                else
                {
                    InitializeComponent();
                    var workingArea = System.Windows.SystemParameters.WorkArea;
                    this.Width = workingArea.Right - 180;
                    this.Height = workingArea.Bottom - 100;
                    listShowx = listShow;
                    this.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                InitializeComponent();
                var workingArea = System.Windows.SystemParameters.WorkArea;
                this.Width = workingArea.Right - 180;
                this.Height = workingArea.Bottom - 100;
                listShowx = listShow;
                this.ShowDialog();
            }
        }
        //
        private void PopupSelectionPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PopupSelectionPage.Visibility == Visibility.Collapsed) PopupSelectionPage.NavigationService.Navigate(null);
        }
        private void ClickOutSidePopup(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }
        public void ShowPopup(object e)
        {
            PopupSelectionPage.Visibility = Visibility.Visible;
            PopupSelectionPage.NavigationService.Navigate(e);
        }
        public void ClosePopup()
        {
            PopupSelectionPage.Visibility = Visibility.Collapsed;
            PopupSelectionPage.NavigationService.Navigate(null);
        }
        private void WinMove(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) this.DragMove();
        }
        private void WinClose(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void WinMiniMize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void WinMaximize(object sender, MouseButtonEventArgs e)
        {
            //this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            //if (this.WindowState == WindowState.Normal)
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
        }
        private void SignIn(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://quanlychung.timviec365.vn/lua-chon-dang-ky.html");
        }
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollMain.ScrollToVerticalOffset(scrollMain.VerticalOffset - e.Delta);
        }
        private void ShowOffItemClick(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as Grid).Tag as ShowOffItem;
            if (z != null && !string.IsNullOrEmpty(z.Link))
            {
                System.Diagnostics.Process.Start(z.Link);
            }
            else System.Diagnostics.Process.Start("https://quanlychung.timviec365.vn/#");
        }
        private void typeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvType.SelectedIndex >= 1) listShowx = listShow.Where(i => i.Type == lvType.SelectedIndex).ToList();
            else listShowx = listShow;
        }
        private void LoginCom(object sender, MouseButtonEventArgs e)
        {
            var x = new LoginWindow(this);
            x.Type = LoginWindow.LoginTypes.Company;

            x.IsFull = this.IsFull;
            x.Width = this.ActualWidth;
            x.Height = this.ActualHeight;
            x.Left = this.Left;
            x.Top = this.Top;

            this.Hide();
            x.ShowDialog();
        }
        private void LoginEp(object sender, MouseButtonEventArgs e)
        {
            var x = new LoginWindow(this);
            x.Type = LoginWindow.LoginTypes.Employee;

            x.IsFull = this.IsFull;
            x.Width = this.ActualWidth;
            x.Height = this.ActualHeight;
            x.Left = this.Left;
            x.Top = this.Top;

            this.Hide();
            x.ShowDialog();
        }
        private void DownloadAndroidApp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=vn.hungha.time_keeping");
        }
        private void DownloadIOSdApp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://apps.apple.com/vn/app/ch%E1%BA%A5m-c%C3%B4ng-365-nh%E1%BA%ADn-di%E1%BB%87n-m%E1%BA%B7t/id1547974966");
        }
        private void LinkSiteMap(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/so-do-trang-web.html");
        }
        private void linkThongTinCanBiet(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/thong-tin-can-biet.html");
        }
        private void linkThoaThuan(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/thoa-thuan-su-dung.html");
        }
        private void linkBaoMat(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/quy-dinh-bao-mat.html");
        }
        private void linhTranhChap(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/giai-quyet-tranh-chap.html");
        }
        private void linhGov(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://online.gov.vn/Home/WebDetails/35979?AspxAutoDetectCookieSupport=1");
        }
        private void linkDMCA(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.dmca.com/Protection/Status.aspx?ID=5b1070f1-e6fb-4ba4-8283-84c7da8f8398&cdnrdr=1&refurl=https://quanlychung.timviec365.vn/");
        }
        private void linhAppTimViecUV(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=timviec365vn.timviec365_vn");
        }
        private void linhAppTimViecNTD(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=vn.timviec365.company");
        }
        private void linkAppCV(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=com.hungha.appcv365");
        }
        private void linkTimViecBanHang(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/viec-lam-ban-hang-c10v0");
        }
        private void linkTimViecDuLich(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/viec-lam-du-lich-c34v0");
        }
        private void linkTimViecQuanLy(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/viec-lam-quan-ly-dieu-hanh-c61v0");
        }
        private void linkTimViecKhachSan(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/viec-lam-khach-san-nha-hang-c8v0");
        }
        private void linkTimViecIT(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/viec-lam-it-phan-mem-c13v0");
        }
        private void linkTimViecPHP(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/tim-viec-lam-php-t11394.html");
        }
        private void LinkGioiThieuChung(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/gioi-thieu-chung.html");
        }
        private void linkFacebook(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/Timviec365.Vn/");
        }
        private void linkTwitter(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/timviec365vn");
        }
        private void linkYoutube(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCI6_mZYL8exLuvmtipBFrkg/videos");
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) IsFull = 1;

            if (this.ActualWidth >= 1366)
            {
                ItemSize = (lvApp.ActualWidth - 80) / 4;
                ItemTypeMargin = new Thickness(30, 0, 30, 0);
                gridBanerChamCong.ColumnDefinitions[1].Width = new GridLength(0.9, GridUnitType.Star);
                stackFooterQR.Visibility = Visibility.Visible;
                baner1.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
                imageBaner.Visibility = Visibility.Visible;
            }
            if (this.ActualWidth <= 1520)
            {
                ItemSize = (lvApp.ActualWidth - 80) / 4;
                ItemTypeMargin = new Thickness(30, 0, 30, 0);
                gridBanerChamCong.ColumnDefinitions[1].Width = new GridLength(0.9, GridUnitType.Star);
                stackFooterQR.Visibility = Visibility.Collapsed;
                baner1.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
                imageBaner.Visibility = Visibility.Visible;
            }
            if (this.ActualWidth <= 1024)
            {
                ItemSize = (lvApp.ActualWidth - 60) / 3;
                ItemTypeMargin = new Thickness(20, 0, 20, 0);
                gridBanerChamCong.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                stackFooterQR.Visibility = Visibility.Collapsed;
                baner1.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                imageBaner.Visibility = Visibility.Collapsed;
            }
            if (this.ActualWidth <= 768)
            {
                ItemSize = (lvApp.ActualWidth - 40) / 2;
                ItemTypeMargin = new Thickness(20, 0, 20, 0);
                gridBanerChamCong.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                stackFooterQR.Visibility = Visibility.Visible;
                baner1.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                imageBaner.Visibility = Visibility.Collapsed;
            }

            if (colFooter1.ActualWidth < stackFooterQRNTD.ActualWidth * 3 + 30)
            {
                stackFooterQR.Visibility = Visibility.Collapsed;
            }
            else stackFooterQR.Visibility = Visibility.Visible;

            if (this.ActualWidth >= 1100)
            {
                Grid.SetColumn(gridLeftFooter, 1);
                Grid.SetRow(gridLeftFooter, 0);
                gridFooter.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                gridFooter.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            }
            else if (this.ActualWidth < 1100)
            {
                stackFooterQR.Visibility = Visibility.Visible;
                Grid.SetColumn(gridLeftFooter, 0);
                Grid.SetRow(gridLeftFooter, 1);
                gridFooter.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                gridFooter.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
            }
        }
        private void wrapFooterQR_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = sender as WrapPanel;
            if (w.ActualWidth < borTimviecNTD.ActualWidth * 2 + 40) wrapFooterQRCV.Width = borTimviecNTD.ActualWidth + 20;
            else wrapFooterQRCV.Width = w.ActualWidth;
        }

		private void PopupSelectionPage_Navigated(object sender, NavigationEventArgs e)
		{

		}
	}
}
