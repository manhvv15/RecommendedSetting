using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using static QRCoder.PayloadGenerator;

namespace QuanLyChung365TruocDangNhap.ChamCong365
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
        public MainWindow(API_Login_Staff api)
        {
            InitializeComponent();
            this.DataContext = this;
            Type = 1;
            APIStaff = api;
            tblComName.Text = api.data.data.user_info.ep_name;
            siderbarUserName.Text = api.data.data.user_info.ep_name;
            imgComLogo.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/employee/" + api.data.data.user_info.ep_image));
            siderbarUserImage.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/employee/" + api.data.data.user_info.ep_image));
            tblComID.Text = "ID: " + api.data.data.user_info.ep_id;
            sidebar.Visibility = Visibility.Collapsed;
            sidebarNV.Visibility = Visibility.Visible;
            SideBarIndexNV = 0;
            loadNoti();
            getQuyenNgDung(api.data.data.user_info.ep_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    if (tt.Result.quyen.Contains("1")) SideBarItemsNV[2].Vis = Visibility.Visible;
                    if (tt.Result.quyen.Contains("2")) SideBarItemsNV[3].Vis = Visibility.Visible;
                    if (tt.Result.quyen.Contains("3")) SideBarItemsNV[4].Vis = Visibility.Visible;
                }
            }));

            getComDetail().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    getMK(tt.Result.data.data.com_pass_encrypt, tt.Result.data.data.from_source).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                    {
                        if (zz.Result != null)
                        {
                            getTokenCom(tt.Result.data.data.com_email, zz.Result).ContinueWith(cc => this.Dispatcher.Invoke(() =>
                            {
                                APICompany = cc.Result;
                            }));
                        }
                    }));
                }
            }));

            stackQuyenTruyCap.Visibility = Visibility.Collapsed;
            if (api.data.data.user_info.role_id == "1") stackQuyenTruyCap.Visibility = Visibility.Visible;
        }
        public MainWindow(API_Login_Company api)
        {
            InitializeComponent();
            this.DataContext = this;
            HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Home(this));
            sidebar.Visibility = Visibility.Visible;
            sidebarNV.Visibility = Visibility.Collapsed;
            stackQuyenTruyCap.Visibility = Visibility.Collapsed;
            Type = 2;
            APICompany = api;
            //SideBarIndex = 0;

            tblComID.Text = "ID: " + api.data.data.user_info.com_id;
            tblComName.Text = api.data.data.user_info.com_name;
            siderbarUserName.Text = api.data.data.user_info.com_name;
            imgComLogo.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + api.data.data.user_info.com_logo));
            if(string.IsNullOrEmpty(api.data.data.user_info.com_logo))
                imgComLogo.Source = new BitmapImage(new Uri("https://chamcong.timviec365.vn/images/logo_com.png"));
            siderbarUserImage.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + api.data.data.user_info.com_logo));
            /*loadNoti().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                ListNoti.ForEach(i =>
                {
                    if (i.is_seen == "0") NotiCount++;
                });
                NotiCount = NotiCount;
            }));*/
        }
        
        //
        public Action LogOut
        {
            get { return (Action)GetValue(LogOutProperty); }
            set { SetValue(LogOutProperty, value); }
        }
        public static readonly DependencyProperty LogOutProperty =
            DependencyProperty.Register("LogOut", typeof(Action), typeof(MainWindow));
        public int Type { get; set; }

        private List<Item_NotifyCom> _ListNoti;

        public List<Item_NotifyCom> ListNoti
        {
            get { return _ListNoti; }
            set { _ListNoti = value; OnPropertyChanged(); }
        }

        public API_Login_Staff APIStaff { get; set; }
        public API_Login_Company APICompany { get; set; }
        public API_NotifyCom APINotiCom { get; set; }
        public int NotiPage { get; set; } = 1;

        private int _NotiCount = 0;
        public int NotiCount
        {
            get { return _NotiCount; }
            set { _NotiCount = value; OnPropertyChanged(); }
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


        public async Task loadNoti(int page = 1)
        {
            HttpClient http = new HttpClient();
            var apilink = "";
            var pram = new List<string>();
            pram.Add("length=10");
            if (page - 1 <= 0) page = 1;
            var offset = (page - 1) * 10;
            pram.Add("off_set=" + offset.ToString());
            switch (Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIStaff.data.data.access_token);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APICompany.data.data.access_token);
                    apilink = "https://chamcong.24hpay.vn/service/get_notify_company.php";
                    pram.Add("com_id=" + APICompany.data.data.user_info.com_id);
                    break;
                default:
                    break;
            }

            if (pram.Count > 0)
            {
                apilink += "?" + string.Join("&", pram);
            }
            var respon = await http.GetAsync(apilink);
            API_NotifyCom api = JsonConvert.DeserializeObject<API_NotifyCom>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null)
            {
                if (ListNoti == null) ListNoti = new List<Item_NotifyCom>();
                ListNoti.AddRange(api.data.items);
                ListNoti = ListNoti.ToList();
            }
        }
        public class SideBarItem1 : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public object Icon { get; set; }
            public string Name { get; set; }
            public bool Child { get; set; }
            public bool HadSubMenu { get; set; } = false;

            private Visibility _Vis = Visibility.Visible;
            public Visibility Vis
            {
                get
                {
                    return _Vis;
                }
                set
                {
                    _Vis = value;
                    OnPropertyChanged();
                }
            }

        }
        private List<SideBarItem1> _SideBarItems = new List<SideBarItem1>() {
                new SideBarItem1(){ Icon=App.Current.Resources["iconHome"],Name=App.Current.Resources["textTrangChu"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["LogoChamCong365"],Name=App.Current.Resources["textChamCong"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconCauHinhChamCong"],Name=App.Current.Resources["textCauHinhChamCong"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["LogoChamCong365"],Name=App.Current.Resources["textCapNhatMat"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconCongCong"],Name=App.Current.Resources["textCongCong"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconQuanLyCongTy"],Name=App.Current.Resources["textQuanLyCongTy"] as string,HadSubMenu=true},
                new SideBarItem1() { Icon = null, Name = App.Current.Resources["textQuanLyCaLamViec"] as string,Vis=Visibility.Collapsed },
                new SideBarItem1() { Icon = null, Name = App.Current.Resources["textQuanLyCongTyCon"] as string,Vis=Visibility.Collapsed },
                new SideBarItem1() { Icon = null, Name = App.Current.Resources["textCauTrucPB"] as string ,Vis=Visibility.Collapsed},
                new SideBarItem1() { Icon = null, Name = App.Current.Resources["textQuyenTruyCap"] as string,Vis=Visibility.Collapsed },
                new SideBarItem1() { Icon = null, Name = App.Current.Resources["textDiMuon"] as string ,Vis=Visibility.Collapsed},
                new SideBarItem1(){ Icon=App.Current.Resources["iconQuanLyNhanVien"],Name=App.Current.Resources["textQuanLyNhanVien"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconLichSuDiemDanhNV"],Name=App.Current.Resources["textLichSuDiemDanh"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconLich"],Name=App.Current.Resources["textLichSuLV"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconCongCong"],Name=App.Current.Resources["textTKXuat"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconXuatCong"],Name=App.Current.Resources["textXuatCong"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconPQNgDung"],Name=App.Current.Resources["textPQNguoiDung"] as string},
                new SideBarItem1(){ Icon=App.Current.Resources["iconChuyenDoiSo"],Name=App.Current.Resources["textChuyenDoiSo"] as string},
        };
        public List<SideBarItem1> SideBarItems
        {
            get { return _SideBarItems; }
            set
            {
                _SideBarItems = value;
                OnPropertyChanged();
            }
        }

        private bool _OpenSubMenu;
        public bool OpenSubMenu
        {
            get { return _OpenSubMenu; }
            set { _OpenSubMenu = value; OnPropertyChanged(); }
        }


        private int _SideBarIndex=0;
        public int SideBarIndex
        {
            get { return _SideBarIndex; }
            set
            {
                switch (value)
                {
                    case 0:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Home(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        break;
                    case 1:
                        ShowPopup(new Views.Pages.ChamCong.ChamCong_Main(this));
                        sidebar.UnselectAll();
                        this.Title = App.Current.Resources["textChamCong"] as string;
                        break;
                    case 2:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.CauHinhChamCong.CauHinhChamCong_Main(this));
                        this.Title = App.Current.Resources["textCauHinhChamCong"] as string;
                        break;
                    case 3:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Cap_Nhap_Lai_Khuon_Mat(this));
                        this.Title = App.Current.Resources["textCapNhatMat"] as string;
                        break;
                    case 4:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Cong_Cong(this));
                        this.Title = App.Current.Resources["textCongCong"] as string;
                        break;
                    case 5:
                        SideBarItems[6].Vis = SideBarItems[6].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[7].Vis = SideBarItems[7].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[8].Vis = SideBarItems[8].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[9].Vis = SideBarItems[9].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        SideBarItems[10].Vis = SideBarItems[10].Vis == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        OpenSubMenu = true;
                        break;
                    case 6:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Shift_Manage(this));
                        this.Title = App.Current.Resources["textQuanLyCaLamViec"] as string;
                        break;
                    case 7:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Company_Child_Manage(this));
                        this.Title = App.Current.Resources["textQuanLyCongTyCon"] as string;
                        break;
                    case 8:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Cau_Truc_Phong_Ban(this));
                        this.Title = App.Current.Resources["textCauTrucPB"] as string;
                        break;
                    case 9:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Quan_Ly_Quyen_Truy_Cap(this));
                        this.Title = App.Current.Resources["textQuyenTruyCap"] as string;
                        break;
                    case 10:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + APICompany.data.data.user_info.com_id + "." + APICompany.data.data.user_info.com_pass + "&link=https://tinhluong.timviec365.vn/quan-ly-di-muon-ve-som.html");
                        break;
                    case 11:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.List_Stuff_All(this));
                        this.Title = App.Current.Resources["textQuanLyNhanVien"] as string;
                        break;
                    case 12:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.History_Attendence_Staff(this));
                        this.Title = App.Current.Resources["textLichSuDiemDanh"] as string;
                        break;
                    case 13:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + APICompany.data.data.user_info.com_id + "." + APICompany.data.data.user_info.com_pass + "&link=https://tinhluong.timviec365.vn/quan-ly-lich-lam-viec.html");
                        break;
                    case 14:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Thong_Ke_De_Xuat_Nghi(this));
                        this.Title = App.Current.Resources["textTKXuat"] as string;
                        break;
                    case 15:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Xuat_Cong.Xuat_Cong_CongTy(this));
                        this.Title = App.Current.Resources["textXuatCong"] as string;
                        break;
                    case 16:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Phan_Quyen_Nguoi_Dung(this));
                        this.Title = App.Current.Resources["textPQNguoiDung"] as string;
                        break;
                    case 17:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + APICompany.data.data.user_info.com_id + "." + APICompany.data.data.user_info.com_pass + "&link=https://quanlychung.timviec365.vn/quan-ly-ung-dung-cong-ty.html");
                        break;
                    default:
                        HomeSelectionPage.NavigationService.Navigate(null);
                        break;
                }
                var z = new List<int>() { 1, 5, 10, 13, 17 };
                if (!z.Contains(value)) _SideBarIndex = value;
                else if (value != 5)
                {
                    var h = sidebar.SelectedIndex;
                    sidebar.UnselectAll();
                    sidebar.SelectedIndex = h;
                }
                OnPropertyChanged("SideBarIndex");
            }
        }

        private List<SideBarItem1> _SideBarItemsNV = new List<SideBarItem1> {
                new SideBarItem1() { Icon = App.Current.Resources["iconHome"],Name = App.Current.Resources["textTrangChu"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconLichSuDiemDanhNV"],Name = App.Current.Resources["textQuanLyChamCong"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconXuatCong"],Name = App.Current.Resources["textQLNhanSu"] as string,Vis = Visibility.Collapsed},
                new SideBarItem1() { Icon = App.Current.Resources["iconXuatCong"],Name = App.Current.Resources["textQuanLyCaLamViec"] as string,Vis = Visibility.Collapsed},
                new SideBarItem1() { Icon = App.Current.Resources["iconXuatCong"],Name = App.Current.Resources["textXuatCongCTY"] as string,Vis = Visibility.Collapsed},
                new SideBarItem1() { Icon = App.Current.Resources["iconXuatCong"],Name = App.Current.Resources["textXuatCong"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconDeXuatCongCong"],Name = App.Current.Resources["textDeXuatCongCong"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconChuyenDoiSo"],Name = App.Current.Resources["textDangKyMat"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconLichSuDiemDanhNV"],Name = App.Current.Resources["textChamCongNV"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconPQNgDung"],Name = App.Current.Resources["textCaiDatChung"] as string},
                new SideBarItem1() { Icon = App.Current.Resources["iconChuyenDoiSo"],Name = App.Current.Resources["textChuyenDoiSo"] as string},
        };
        public List<SideBarItem1> SideBarItemsNV
        {
            get { return _SideBarItemsNV; }
            set
            {
                _SideBarItemsNV = value;
                OnPropertyChanged();
            }
        }

        private int _SideBarIndexNV=0;
        public int SideBarIndexNV
        {
            get { return _SideBarIndexNV; }
            set
            {
                switch (value)
                {
                    case 0:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.QL_Nhan_Vien.Home_Stuff(this));
                        this.Title = App.Current.Resources["textTrangChu"] as string;
                        break;
                    case 1:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.QL_Nhan_Vien.Timekeeping_Manage(this));
                        this.Title = App.Current.Resources["textQuanLyChamCong"] as string;
                        break;
                    case 2:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.List_Stuff_All(this));
                        this.Title = App.Current.Resources["textQLNhanSu"] as string;
                        break;
                    case 3:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Shift_Manage(this));
                        this.Title = App.Current.Resources["textQuanLyCaLamViec"] as string;
                        break;
                    case 4:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Xuat_Cong.Xuat_Cong_CongTy(this));
                        this.Title = App.Current.Resources["textXuatCongCTY"] as string;
                        break;
                    case 5:
                        HomeSelectionPage.NavigationService.Navigate(new Views.Pages.Xuat_Cong.Xuat_Cong_NhanVien(this));
                        this.Title = App.Current.Resources["textXuatCong"] as string;
                        break;
                    case 6:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + APIStaff.data.data.user_info.ep_id + "." + APIStaff.data.data.user_info.ep_pass + "&link=https://vanthu.timviec365.vn/tao-de-xuat.html");
                        break;
                    case 7:
                        ShowPopup(new Views.Pages.ChamCong.ChamCong_Main(this, Views.Pages.ChamCong.ChamCong_Main.EditType.UpdateFace));
                        this.Title = App.Current.Resources["textDangKyMat"] as string;
                        break;
                    case 8:
                        ShowPopup(new Views.Pages.ChamCong.ChamCong_Main(this));
                        this.Title = App.Current.Resources["textChamCong"] as string;
                        break;
                    case 9:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + APIStaff.data.data.user_info.ep_id + "." + APIStaff.data.data.user_info.ep_pass + "&link=https://quanlychung.timviec365.vn/quan-ly-ung-dung-nhan-vien.html");
                        break;
                    case 10:
                        Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + APIStaff.data.data.user_info.ep_id + "." + APIStaff.data.data.user_info.ep_pass + "&link=https://quanlychung.timviec365.vn/quan-ly-ung-dung-nhan-vien.html");
                        break;
                    default:
                        HomeSelectionPage.NavigationService.Navigate(null);
                        break;
                }
                var z = new List<int>() { 6, 7, 8, 9, 10 };
                if (!z.Contains(value)) _SideBarIndexNV = value;
                else
                {
                    var h = sidebarNV.SelectedIndex;
                    sidebarNV.UnselectAll();
                    sidebarNV.SelectedIndex = h;
                }
                OnPropertyChanged();
            }
        }
        private async Task<API_QuyenNguoiDung> getQuyenNgDung(string id)
        {
            try
            {
                HttpClient http = new HttpClient();
                switch (this.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APIStaff.data.data.access_token);
                        break;
                    default:
                        break;
                }
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("id_us", id);

                HttpResponseMessage respon = await http.PostAsync("https://api.timviec365.vn/api/qlc/permission/check_role", new FormUrlEncodedContent(form));
                var x = respon.Content.ReadAsStringAsync().Result;
                API_QuyenNguoiDung q = JsonConvert.DeserializeObject<API_QuyenNguoiDung>(x);
                return q;
            }
            catch { return null; }
            
        }
        private async Task<API_Com_ChiTiet> getComDetail()
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/company/info";
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                switch (Type)
                {
                    case 1:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APIStaff.data.data.access_token);
                        form.Add("com_id", APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APICompany.data.data.access_token);
                        form.Add("com_id", APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                int i = 0;
                List<ChildCompany> list = new List<ChildCompany>();
                
                var respon = await httpClient.PostAsync(apilink,new FormUrlEncodedContent(form));
                API_Com_ChiTiet api = JsonConvert.DeserializeObject<API_Com_ChiTiet>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null) return api;
                return null;
            }
            catch (Exception ex)
            {

                var x = new Views.PopUp.Notify1();
                x.Type = Views.PopUp.Notify1.NotifyType.Error;
                x.Message = ex.Message;
                ShowPopup(x);
                return null;
            }
        }
        private async Task<string> getMK(string pass, string key)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "https://chamcong.24hpay.vn/service/gen_pass.php";
                var pram = new List<string>();
                pram.Add("pass=" + pass);
                pram.Add("key=" + key);
                switch (Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APIStaff.data.data.access_token);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }

                HttpResponseMessage respon = await http.GetAsync(apilink);
                return respon.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {

                var x = new Views.PopUp.Notify1();
                x.Type = Views.PopUp.Notify1.NotifyType.Error;
                x.Message = ex.Message;
                ShowPopup(x);
                return null;
            }
        }
        private async Task<API_Login_Company> getTokenCom(string email, string pass)
        {
            try
            {
                HttpClient http = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("account", email);
                form.Add("password", pass);
                form.Add("type", "1");
                switch (Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APIStaff.data.data.access_token);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                API_Login_Company api = JsonConvert.DeserializeObject<API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    return api;
                }
                return null;
            }
            catch (Exception ex)
            {

                var x = new Views.PopUp.Notify1();
                x.Type = Views.PopUp.Notify1.NotifyType.Error;
                x.Message = ex.Message;
                ShowPopup(x);
                return null;
            }
        }

        public bool _isEmptyOrInvalid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            var jwtToken = new JwtSecurityToken(token);
            return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
        }
        public async void refreshToken()
        {
            try
            {
                HttpClient http = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                HttpResponseMessage respon;
                switch (Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APIStaff.data.data.access_token);
                        form.Add("refresh_token",APIStaff.data.data.refresh_token);
                        respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/employee/getNewToken", new FormUrlEncodedContent(form));
                        API_Login_Staff api = JsonConvert.DeserializeObject<API_Login_Staff>(respon.Content.ReadAsStringAsync().Result);
                        if (api.data != null && api.data.result == true)
                        {
                            APIStaff.data.data.access_token = api.data.data.access_token;
                        }
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", APICompany.data.data.access_token);
                        form.Add("refresh_token", APICompany.data.data.refresh_token);
                        respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/employee/getNewToken", new FormUrlEncodedContent(form));
                        API_Login_Staff api1 = JsonConvert.DeserializeObject<API_Login_Staff>(respon.Content.ReadAsStringAsync().Result);
                        if (api1.data != null && api1.data.result == true)
                        {
                            APICompany.data.data.access_token = api1.data.data.access_token;
                        }
                        break;
                    default:
                        break;
                }
                
                
            }
            catch (Exception ex)
            {

                var x = new Views.PopUp.Notify1();
                x.Type = Views.PopUp.Notify1.NotifyType.Error;
                x.Message = ex.Message;
                ShowPopup(x);
            }
        }
        //
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) this.DragMove();
        }
        private void Minimimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CloseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.Close();
            System.Environment.Exit(0);
            Application.Current.Shutdown();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) IsFull = 1;

            if (this.ActualWidth <= 900)
            {
                gridMain.ColumnDefinitions[0].Width = new GridLength(0);
                logo1.Visibility = Visibility.Visible;
                aa.Visibility = Visibility.Collapsed;
                stackUser.Visibility = Visibility.Collapsed;
                gridTop.Background = App.Current.Resources["#4C5BD4"] as SolidColorBrush;
                PopUp_Menu.Visibility = Visibility.Visible;
            }
            else
            {
                gridMain.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
                logo1.Visibility = Visibility.Collapsed;
                aa.Visibility = Visibility.Visible;
                stackUser.Visibility = Visibility.Visible;
                gridTop.Background = App.Current.Resources["#FFFFFF"] as SolidColorBrush;
                PopUp_Menu.Visibility = Visibility.Collapsed;
            }

            if (Grid.GetColumn(panelSideBar) == 1)
            {
                panelSideBar.Background = App.Current.Resources["#FFFFFF"] as SolidColorBrush;
                Grid.SetColumn(panelSideBar, 0);
                Grid.SetRow(panelSideBar, 2);
                Grid.SetRowSpan(panelSideBar, 1);
                siderbarUser.Visibility = Visibility.Collapsed;
                siderbarUserOutLine.Visibility = Visibility.Collapsed;
            }
        }
        private void Notify_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ListNoti != null && ListNoti.Count > 0) ShowPopup(new QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.NoTiFy(this));
        }
        private void PopupSelection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PopupSelection.NavigationService.Navigate(null);
            PopupSelection.Visibility = Visibility.Collapsed;
        }
        //ShowPopUp
        public void ShowPopup(object pop)
        {
            PopupSelection.Visibility = Visibility.Visible;
            PopupSelection.NavigationService.Navigate(pop);
        }
        public void ClosePopup()
        {
            PopupSelection.NavigationService.Navigate(null);
            PopupSelection.Visibility = Visibility.Collapsed;
        }
        public void NewPage(object page)
        {
            PopupSelection.Visibility = Visibility.Collapsed;
            HomeSelectionPage.NavigationService.Navigate(page);
        }

        public void RefreshComName(string name)
        {
            APICompany.data.data.user_info.com_name = name;
            tblComName.Text = APICompany.data.data.user_info.com_name;
        }
        public void RefreshEpName(string name)
        {
            APIStaff.data.data.user_info.ep_name = name;
            tblComName.Text = APIStaff.data.data.user_info.ep_name;
        }
        private void stackUser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowPopup(new Views.PopUp.popUp_MainWindow.dropDown(this));
        }
        private void Maximize(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
        }
        private void HomeSelectionPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PopupSelection.Visibility == Visibility.Collapsed)
            {
                PopupSelection.NavigationService.Navigate(null);
            }
        }
        private void linkGioiThieu(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chamcong.timviec365.vn/gioi-thieu.html");
        }
        private void linkHuongDan(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chamcong.timviec365.vn/huong-dan.html");
        }
        private void linkTinTuc(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://timviec365.vn/blog/c237/cham-cong");
        }
        private void linkDownload(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chamcong.timviec365.vn/download.html");
        }

        private void ckNhanVien_Checked(object sender, RoutedEventArgs e)
        {
            if (ckNhanSu != null) ckNhanSu.IsChecked = false;
            if (sidebar != null) sidebar.Visibility = Visibility.Collapsed;
            if (sidebarNV != null) sidebarNV.Visibility = Visibility.Visible;

            if (sidebar != null) sidebar.SelectedIndex = -1;
            if (sidebarNV != null) sidebarNV.SelectedIndex = 0;


            if (APIStaff != null)
            {
                this.Type = 1;
                tblComName.Text = APIStaff.data.data.user_info.ep_name;
                siderbarUserName.Text = APIStaff.data.data.user_info.ep_name;
                imgComLogo.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/employee/" + APIStaff.data.data.user_info.ep_image));
                siderbarUserImage.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/employee/" + APIStaff.data.data.user_info.ep_image));
                tblComID.Text = "ID: " + APIStaff.data.data.user_info.ep_id;


            }

        }

        private void ckNhanSu_Checked(object sender, RoutedEventArgs e)
        {
            if (ckNhanVien != null) ckNhanVien.IsChecked = false;
            if (sidebar != null) sidebar.Visibility = Visibility.Visible;
            if (sidebarNV != null) sidebarNV.Visibility = Visibility.Collapsed;

            if (sidebarNV != null) sidebarNV.SelectedIndex = -1;
            if (sidebar != null) sidebar.SelectedIndex = 0;


            if (APICompany != null)
            {
                this.Type = 2;
                tblComID.Text = "ID: " + APICompany.data.data.user_info.com_id;
                tblComName.Text = APICompany.data.data.user_info.com_name;
                siderbarUserName.Text = APICompany.data.data.user_info.com_name;
                imgComLogo.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + APICompany.data.data.user_info.com_logo));
                siderbarUserImage.Source = new BitmapImage(new Uri("https://chamcong.24hpay.vn/upload/company/logo/" + APICompany.data.data.user_info.com_logo));
            }
        }

        private void PopUp_Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Grid.GetColumn(panelSideBar) == 0)
            {
                panelSideBar.Background = App.Current.Resources["#FFFFFF"] as SolidColorBrush;
                Grid.SetColumn(panelSideBar, 1);
                Grid.SetRow(panelSideBar, 1);
                Grid.SetRowSpan(panelSideBar, 2);
                siderbarUser.Visibility = Visibility.Visible;
                siderbarUserOutLine.Visibility = Visibility.Visible;
                //var z = new Rectangle() { Fill=new SolidColorBrush(Colors.Transparent)};
                //ShowPopup(z);
                return;
            }
            else
            {
                Grid.SetColumn(panelSideBar, 0);
                panelSideBar.Background = App.Current.Resources["#FFFFFF"] as SolidColorBrush;
                Grid.SetColumn(panelSideBar, 1);
                Grid.SetRow(panelSideBar, 1);
                Grid.SetRowSpan(panelSideBar, 2);
                siderbarUser.Visibility = Visibility.Collapsed;
                siderbarUserOutLine.Visibility = Visibility.Collapsed;
                return;
            }
        }

        private void siderbarUserOutLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Grid.GetColumn(panelSideBar) == 1)
            {
                panelSideBar.Background = App.Current.Resources["#FFFFFF"] as SolidColorBrush;
                Grid.SetColumn(panelSideBar, 0);
                Grid.SetRow(panelSideBar, 2);
                Grid.SetRowSpan(panelSideBar, 1);
                siderbarUser.Visibility = Visibility.Collapsed;
                siderbarUserOutLine.Visibility = Visibility.Collapsed;
            }
        }

        private void siderbarUserShowPopup(object sender, MouseButtonEventArgs e)
        {
            ShowPopup(new Views.PopUp.popUp_MainWindow.dropDown(this));
        }

        private void ckNhanVien_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckNhanVien.IsChecked == true && ckNhanSu.IsChecked == false) e.Handled = true;
        }

        private void ckNhanSu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckNhanVien.IsChecked == false && ckNhanSu.IsChecked == true) e.Handled = true;

        }
    }
}
